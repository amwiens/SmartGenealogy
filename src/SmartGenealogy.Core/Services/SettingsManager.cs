﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.Json;

using AsyncAwaitBestPractices;

using CompiledExpressions;

using Injectio.Attributes;

using Microsoft.Extensions.Logging;

using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Models;
using SmartGenealogy.Core.Models.FileInterfaces;
using SmartGenealogy.Core.Models.Settings;

namespace SmartGenealogy.Core.Services;

[RegisterSingleton<ISettingsManager, SettingsManager>]
public class SettingsManager(ILogger<SettingsManager> logger) : ISettingsManager
{
    private static string GlobalSettingsPath => Path.Combine(Compat.AppDataHome, "global.json");

    private readonly SemaphoreSlim fileLock = new(1, 1);

    private bool isLoaded;

    private DirectoryPath? libraryDirOverride;



    private DirectoryPath? libraryDir;
    public DirectoryPath LibraryDir
    {
        get
        {
            if (libraryDir is null)
            {
                throw new InvalidOperationException("LibraryDir is not set");
            }

            return libraryDir;
        }
        private set
        {
            var isChanged = libraryDir != value;

            libraryDir = value;

            // Only invoke if different
            if (isChanged)
            {
                LibraryDirChanged?.Invoke(this, value);
            }
        }
    }

    [MemberNotNullWhen(true, nameof(libraryDir))]
    public bool IsLibraryDirSet => libraryDir is not null;

    // Dynamic paths from library
    private FilePath SettingsFile => LibraryDir.JoinFile("settings.json");

    public string DownloadsDirectory => Path.Combine(LibraryDir, ".downloads");


    public Settings Settings { get; private set; } = new();



    /// <inheritdoc/>
    public event EventHandler<string>? LibraryDirChanged;

    /// <inheritdoc />
    public event EventHandler<RelayPropertyChangedEventArgs>? SettingsPropertyChanged;

    /// <inheritdoc />
    public event EventHandler? Loaded;

    /// <inheritdoc />
    public void SetLibraryDirOverride(DirectoryPath path)
    {
        libraryDirOverride = path;
    }

    /// <inheritdoc />
    public void RegisterOnLibraryDirSet(Action<string> handler)
    {
        if (IsLibraryDirSet)
        {
            handler(LibraryDir);
            return;
        }

        LibraryDirChanged += Handler;

        return;

        void Handler(object? sender, string dir)
        {
            LibraryDirChanged -= Handler;
            handler(dir);
        }
    }

    /// <inheritdoc />
    public SettingsTransaction BeginTransaction()
    {
        if (!IsLibraryDirSet)
        {
            throw new InvalidOperationException("LibraryDir not set when BeginTransaction was called");
        }
        return new SettingsTransaction(this, () => SaveSettings(), () => SaveSettingsAsync());
    }

    /// <inheritdoc />
    public void Transaction(Action<Settings> func, bool ignoreMissingLibraryDir = false)
    {
        if (!IsLibraryDirSet)
        {
            if (ignoreMissingLibraryDir)
            {
                func(Settings);
                return;
            }
            throw new InvalidOperationException("LibraryDir not set when Transaction was called");
        }
        using var transaction = BeginTransaction();
        func(transaction.Settings);
    }

    /// <inheritdoc />
    public void Transaction<TValue>(Expression<Func<Settings, TValue>> expression, TValue value)
    {
        var accessor = CompiledExpression.CreateAccessor(expression);

        // Set value
        using var transaction = BeginTransaction();
        accessor.Set(transaction.Settings, value);

        // Invoke property changed event
        SettingsPropertyChanged?.Invoke(this, new RelayPropertyChangedEventArgs(accessor.FullName));
    }

    /// <inheritdoc />
    public IDisposable RelayPropertyFor<T, TValue>(
        T source,
        Expression<Func<T, TValue>> sourceProperty,
        Expression<Func<Settings, TValue>> settingsProperty,
        bool setInitial = false,
        TimeSpan? delay = null)
        where T : INotifyPropertyChanged
    {
        var sourceInstanceAccessor = CompiledExpression.CreateAccessor(sourceProperty).WithInstance(source);
        var settingsAccessor = CompiledExpression.CreateAccessor(settingsProperty);

        var sourcePropertyPath = sourceInstanceAccessor.FullName;
        var settingsPropertyPath = settingsAccessor.FullName;

        var sourceTypeName = source.GetType().Name;

        // Update source when settings change
        void OnSettingsPropertyChanged(object? sender, RelayPropertyChangedEventArgs args)
        {
            if (args.PropertyName != settingsPropertyPath)
                return;

            // Skip if event is relay and the sender is the source, to prevent duplicate
            if (args.IsRelay && ReferenceEquals(sender, source))
                return;

            logger.LogTrace(
                "[RelayPropertyFor] " + "Settings.SettingsProperty:l} => {SourceType:l}.{SourceProperty:l}",
                settingsPropertyPath,
                sourceTypeName,
                sourcePropertyPath);

            sourceInstanceAccessor.Set(source, settingsAccessor.Get(Settings));
        }

        // Set and Save settings when source changes
        void OnSourcePropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != sourcePropertyPath)
                return;

            logger.LogTrace(
                "[RelayPropertyFor] {SourceType:l}.{SourceProperty:l} -> Settings.{SettingsProperty:l}",
                sourceTypeName,
                sourcePropertyPath,
                settingsPropertyPath);

            settingsAccessor.Set(Settings, sourceInstanceAccessor.Get());

            if (IsLibraryDirSet)
            {
                if (delay != null)
                {
                    SaveSettingsDelayed(delay.Value).SafeFireAndForget();
                }
                else
                {
                    SaveSettingsAsync().SafeFireAndForget();
                }
            }
            else
            {
                logger.LogWarning(
                    "[RelayPropertyFor] LibraryDir not set when saving ({SourceType:l}.{SourceProperty:l} -> Settings.{SettingsProperty:l})",
                    sourceTypeName,
                    sourcePropertyPath,
                    settingsPropertyPath);
            }

            // Invoke property changed event, passing along sender
            SettingsPropertyChanged?.Invoke(
                sender,
                new RelayPropertyChangedEventArgs(settingsPropertyPath, true));
        }

        var subscription = Disposable.Create(() =>
        {
            source.PropertyChanged -= OnSourcePropertyChanged;
            SettingsPropertyChanged -= OnSettingsPropertyChanged;
        });

        try
        {
            SettingsPropertyChanged += OnSettingsPropertyChanged;
            source.PropertyChanged += OnSourcePropertyChanged;

            // Set initial value if requested
            if (setInitial)
            {
                sourceInstanceAccessor.Set(settingsAccessor.Get(Settings));
            }
        }
        catch
        {
            subscription.Dispose();
            throw;
        }

        return subscription;
    }

    /// <inheritdoc />
    public IDisposable RegisterPropertyChangedHandler<T>(
        Expression<Func<Settings, T>> settingsProperty,
        Action<T> onPropertyChanged)
    {
        var handlerName = onPropertyChanged.Method.Name;
        var settingsAccessor = CompiledExpression.CreateAccessor(settingsProperty);

        return Observable
            .FromEventPattern<EventHandler<RelayPropertyChangedEventArgs>, RelayPropertyChangedEventArgs>(
                h => SettingsPropertyChanged += h,
                h => SettingsPropertyChanged -= h)
            .Where(args => args.EventArgs.PropertyName == settingsAccessor.FullName)
            .Subscribe(_ =>
            {
                logger.LogTrace(
                    "[RegisterPropertyChangedHandler] Settings.{SettingsProperty:l} -> Handler ({Action})",
                    settingsAccessor.FullName,
                    handlerName);

                onPropertyChanged(settingsAccessor.Get(Settings));
            });
    }

    /// <summary>
    /// Attempts to locate and set the library path
    /// Return true if found, false otherwise
    /// </summary>
    public bool TryFindLibrary(bool forceReload = false)
    {
        if (IsLibraryDirSet && !forceReload)
            return true;

        // 0. Check Override
        if (libraryDirOverride is not null)
        {
            logger.LogInformation("Using library override path {Path}", libraryDirOverride.FullPath);

            LibraryDir = libraryDirOverride;
            SetStaticLibraryPaths();
            LoadSettings();
            return true;
        }

        // 1. Check portable mode
        //var appDir = Compat.AppCurrentDir;
        //IsPortableMode = File.Exists(Path.Combine(appDir, "Data", ".sm-portable"));
        //if (IsPortableMode)
        //{
        //    LibraryDir = appDir + "Data";
        //    SetStaticLibraryPaths();
        //    LoadSettings();
        //    return true;
        //}

        // 2. Check %APPDATA%/SmartGenealogy/library.json
        FilePath libraryJsonFile = Compat.AppDataHome + "library.json";
        if (!libraryJsonFile.Exists)
            return false;

        try
        {
            var libraryJson = libraryJsonFile.ReadAllText();
            var librarySettings = JsonSerializer.Deserialize<LibrarySettings>(libraryJson);

            if (!string.IsNullOrWhiteSpace(librarySettings?.LibraryPath)
                && Directory.Exists(librarySettings.LibraryPath))
            {
                LibraryDir = librarySettings.LibraryPath;
                SetStaticLibraryPaths();
                LoadSettings();
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning("Failed to read library.json in AppData: {Message}", ex.Message);
        }
        return false;
    }

    // Set static classes requiring library path
    private void SetStaticLibraryPaths()
    {
        GlobalConfig.LibraryDir = LibraryDir;
        //ArchiveHelper.HomeDir = LibraryDir;
    }

    /// <summary>
    /// Save a new library path to %APPDATA%/SmartGenealogy/library.json
    /// </summary>
    public void SetLibraryPath(string path)
    {
        Compat.AppDataHome.Create();
        var libraryJsonFile = Compat.AppDataHome.JoinFile("library.json");

        var library = new LibrarySettings { LibraryPath = path };
        var libraryJson = JsonSerializer.Serialize(
            library,
            new JsonSerializerOptions { WriteIndented = true });
        libraryJsonFile.WriteAllText(libraryJson);

        // actually create the LibraryPath directory
        Directory.CreateDirectory(path);
    }

    public bool IsEulaAccepted()
    {
        if (!File.Exists(GlobalSettingsPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(GlobalSettingsPath)!);
            File.Create(GlobalSettingsPath).Close();
            File.WriteAllText(GlobalSettingsPath, "{}");
            return false;
        }

        var json = File.ReadAllText(GlobalSettingsPath);
        var globalSettings = JsonSerializer.Deserialize<GlobalSettings>(json);
        return globalSettings?.EulaAccepted ?? false;
    }

    public void SetEulaAccepted()
    {
        var globalSettings = new GlobalSettings { EulaAccepted = true };
        var json = JsonSerializer.Serialize(globalSettings);
        File.WriteAllText(GlobalSettingsPath, json);
    }

    /// <summary>
    /// Loads settings from the settings file. Continues without loading if the file does not exist or is empty.
    /// Will set <see cref="isLoaded"/> to true when finished in any case.
    /// </summary>
    /// <param name="cancellationToken"></param>
    protected virtual void LoadSettings(CancellationToken cancellationToken = default)
    {
        fileLock.Wait(cancellationToken);

        try
        {
            if (!SettingsFile.Exists)
            {
                return;
            }

            using var fileStream = SettingsFile.Info.OpenRead();

            if (fileStream.Length == 0)
            {
                logger.LogWarning("Settings file is empty, using default settings");
                return;
            }

            var loadedSettings = JsonSerializer.Deserialize(
                fileStream,
                SettingsSerialzerContext.Default.Settings);

            if (loadedSettings is not null)
            {
                Settings = loadedSettings;
            }
        }
        finally
        {
            fileLock.Release();

            isLoaded = true;

            Loaded?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Loads settings from the settings file. Continues without loading if the file does not exist or is empty.
    /// Will set <see cref="isLoaded"/> to true when finished in any case.
    /// </summary>
    protected virtual async Task LoadSettingsAsync(CancellationToken cancellationToken = default)
    {
        await fileLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (!SettingsFile.Exists)
            {
                return;
            }

            await using var fileStream = SettingsFile.Info.OpenRead();

            if (fileStream.Length == 0)
            {
                logger.LogWarning("Settings file is empty, using default settings");
                return;
            }

            var loadedSettings = await JsonSerializer
                .DeserializeAsync(fileStream, SettingsSerialzerContext.Default.Settings, cancellationToken)
                .ConfigureAwait(false);

            if (loadedSettings is not null)
            {
                Settings = loadedSettings;
            }

            Loaded?.Invoke(this, EventArgs.Empty);
        }
        finally
        {
            fileLock.Release();

            isLoaded = true;

            Loaded?.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void SaveSettings(CancellationToken cancellationToken = default)
    {
        // Skip saving if not loaded yet
        if (!isLoaded)
            return;

        fileLock.Wait(cancellationToken);

        try
        {
            // Create empty settings file if it doesn't exist
            if (!SettingsFile.Exists)
            {
                SettingsFile.Directory?.Create();
                SettingsFile.Create();
            }

            // Check disk space
            if (SystemInfo.GetDiskFreeSpaceBytes(SettingsFile) is < 1 * SystemInfo.Mebibyte)
            {
                logger.LogWarning("Not enough disk space to save settings");
                return;
            }

            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(
                Settings,
                SettingsSerialzerContext.Default.Settings);

            if (jsonBytes.Length == 0)
            {
                logger.LogError("JsonSerializer returned empty bytes for some reason");
                return;
            }

            using var fs = File.Open(SettingsFile, FileMode.Open);
            if (fs.CanWrite)
            {
                fs.Write(jsonBytes, 0, jsonBytes.Length);
                fs.Flush();
                fs.SetLength(jsonBytes.Length);
            }
        }
        finally
        {
            fileLock.Release();
        }
    }

    protected virtual async Task SaveSettingsAsync(CancellationToken cancellationToken = default)
    {
        // Skip saving if not loaded yet
        if (!isLoaded)
            return;

        await fileLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            // Create empty settings file if it doesn't exist
            if (!SettingsFile.Exists)
            {
                SettingsFile.Directory?.Create();
                SettingsFile.Create();
            }

            // Check disk space
            if (SystemInfo.GetDiskFreeSpaceBytes(SettingsFile) is < 1 * SystemInfo.Mebibyte)
            {
                logger.LogWarning("Not enough disk space to save settings");
                return;
            }

            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(
                Settings,
                SettingsSerialzerContext.Default.Settings);

            if (jsonBytes.Length == 0)
            {
                logger.LogError("JsonSerializer returned empty bytes for some reason");
                return;
            }

            await using var fs = File.Open(SettingsFile, FileMode.Open);
            if (fs.CanWrite)
            {
                await fs.WriteAsync(jsonBytes, cancellationToken).ConfigureAwait(false);
                await fs.FlushAsync(cancellationToken).ConfigureAwait(false);
                fs.SetLength(jsonBytes.Length);
            }
        }
        finally
        {
            fileLock.Release();
        }
    }

    private volatile CancellationTokenSource? delayedSaveCts;

    private Task SaveSettingsDelayed(TimeSpan delay)
    {
        var cts = new CancellationTokenSource();

        var oldCancellationToken = Interlocked.Exchange(ref delayedSaveCts, cts);

        try
        {
            oldCancellationToken?.Cancel();
        }
        catch (ObjectDisposedException) { }

        return Task.Run(
            async () =>
            {
                try
                {
                    await Task.Delay(delay, cts.Token).ConfigureAwait(false);

                    await SaveSettingsAsync(cts.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException) { }
                finally
                {
                    cts.Dispose();
                }
            }, CancellationToken.None);
    }

    public Task FlushAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        if (!isLoaded)
        {
            return Task.CompletedTask;
        }

        // Cancel any delayed save tasks
        try
        {
            Interlocked.Exchange(ref delayedSaveCts, null)?.Cancel();
        }
        catch (ObjectDisposedException) { }

        return SaveSettingsAsync(cancellationToken);
    }
}