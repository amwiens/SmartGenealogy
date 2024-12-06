using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using CompiledExpressions;

using Microsoft.Extensions.Logging;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Models;
using SmartGenealogy.Core.Models.FileInterfaces;
using SmartGenealogy.Core.Models.Settings;

namespace SmartGenealogy.Core.Services;

[Singleton(typeof(IProjectSettingsManager))]
public class ProjectSettingsManager(ILogger<ProjectSettingsManager> logger) : IProjectSettingsManager
{
    private readonly SemaphoreSlim fileLock = new(1, 1);

    private bool isLoaded;

    private DirectoryPath? projectDir;
    public DirectoryPath ProjectDir
    {
        get
        {
            if (projectDir is null)
            {
                throw new InvalidOperationException("ProjectDir is not set");
            }

            return projectDir;
        }
        private set
        {
            var isChanged = projectDir != value;

            projectDir = value;

            // Only invoke if different
            if (isChanged)
            {
                ProjectDirChanged?.Invoke(this, value);
            }
        }
    }

    [MemberNotNullWhen(true, nameof(projectDir))]
    public bool IsProjectDirSet => projectDir is not null;

    // Dynamic paths from project
    private FilePath ProjectSettingsFile => ProjectDir.JoinFile($"{Settings.Name}.sgproj");

    public string DownloadsDirectory => Path.Combine(ProjectDir, ".downloads");

    public ProjectSettings Settings { get; private set; } = new();

    /// <inheritdoc />
    public event EventHandler<string>? ProjectDirChanged;

    /// <inheritdoc />
    public event EventHandler<RelayPropertyChangedEventArgs>? SettingsPropertyChanged;

    /// <inheritdoc />
    public event EventHandler? Loaded;

    /// <inheritdoc />
    public void RegisterOnProjectDirSet(Action<string> handler)
    {
        if (IsProjectDirSet)
        {
            handler(ProjectDir);
            return;
        }

        ProjectDirChanged += Handler;

        return;

        void Handler(object? sender, string dir)
        {
            ProjectDirChanged -= Handler;
            handler(dir);
        }
    }

    /// <inheritdoc />
    public ProjectSettingsTransaction BeginTransaction()
    {
        if (!IsProjectDirSet)
        {
            throw new InvalidOperationException("ProjectDir not set when BeginTransaction was called");
        }
        return new ProjectSettingsTransaction(this, () => SaveProjectSettings(), () => SaveProjectSettingsAsync());
    }

    /// <inheritdoc />
    public void Transaction(Action<ProjectSettings> func, bool ignoreMissingProjectDir = false)
    {
        if (!IsProjectDirSet)
        {
            if (ignoreMissingProjectDir)
            {
                func(Settings);
                return;
            }
            throw new InvalidOperationException("ProjectDir not set when Transaction was called");
        }
        using var transaction = BeginTransaction();
        func(transaction.ProjectSettings);
    }

    /// <inheritdoc />
    public void Transaction<TValue>(Expression<Func<ProjectSettings, TValue>> expression, TValue value)
    {
        var accessor = CompiledExpression.CreateAccessor(expression);

        // Set value
        using var transaction = BeginTransaction();
        accessor.Set(transaction.ProjectSettings, value);

        // Invoke property changed event
        SettingsPropertyChanged?.Invoke(this, new RelayPropertyChangedEventArgs(accessor.FullName));
    }

    /// <inheritdoc />
    public IDisposable RelayPropertyFor<T, TValue>(T source, Expression<Func<T, TValue>> sourceProperty, Expression<Func<ProjectSettings, TValue>> settingsProperty, bool setInitial = false, TimeSpan? delay = null) where T : INotifyPropertyChanged
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IDisposable RegisterPropertyChangedHandler<T>(Expression<Func<ProjectSettings, T>> settingsProperty, Action<T> onPropertyChanged)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Attempts to locate and set the project path
    /// Return true if found, false otherwise
    /// </summary>
    public bool TryFindProject(bool forceReload = false)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Save a new project path to %APPDATA%/SmartGenealogy/library.json
    /// </summary>
    /// <param name="path"></param>
    public void SetProjectPath(string path)
    {
        new DirectoryPath(path).Create();
        var projectJsonFile = new DirectoryPath(path).JoinFile($"{Settings.Name}.sgproj");

        var project = new ProjectSettings { DatabasePath = path, Name = Settings.Name };
        var projectJson = JsonSerializer.Serialize(
            project,
            new JsonSerializerOptions { WriteIndented = true });
        projectJsonFile.WriteAllText(projectJson);
    }


    protected virtual void LoadProjectSettings(CancellationToken cancellationToken = default)
    {

    }

    protected virtual async Task LoadProjectSettingsAsync(CancellationToken cancellationToken = default)
    {

    }

    protected virtual void SaveProjectSettings(CancellationToken cancellationToken = default)
    {

    }

    protected virtual async Task SaveProjectSettingsAsync(CancellationToken cancellationToken = default)
    {

    }
}