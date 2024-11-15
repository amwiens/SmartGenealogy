using System.ComponentModel;
using System.Linq.Expressions;

using SmartGenealogy.Core.Models;
using SmartGenealogy.Core.Models.FileInterfaces;
using SmartGenealogy.Core.Models.Settings;

namespace SmartGenealogy.Core.Services;

public interface ISettingsManager
{
    bool IsPortableMode { get; }
    DirectoryPath LibraryDir { get; }
    bool IsLibraryDirSet { get; }

    string DownloadsDirectory { get; }

    Settings Settings { get; }

    /// <summary>
    /// Event fired when the library directory is changed
    /// </summary>
    event EventHandler<string>? LibraryDirChanged;

    /// <summary>
    /// Event fired when a property of Settings is changed
    /// </summary>
    event EventHandler<RelayPropertyChangedEventArgs>? SettingsPropertyChanged;

    /// <summary>
    /// Event fired when Settings are loaded from disk
    /// </summary>
    event EventHandler? Loaded;



    /// <summary>
    /// Return a SettingsTransaction that can be used to modify Settings
    /// Saves on Dispose
    /// </summary>
    SettingsTransaction BeginTransaction();

    /// <summary>
    /// Execute a function that modifies Settings
    /// Commits changes after the function returns.
    /// </summary>
    /// <param name="func">Function accepting Settings to modify</param>
    /// <param name="ignoreMissingLibraryDir">Ignore missing library dir when committing changes</param>
    void Transaction(Action<Settings> func, bool ignoreMissingLibraryDir = false);

    /// <summary>
    /// Modify a settings property by expression and commit changes.
    /// This will notify listeners of SettingsPropertyChanged.
    /// </summary>
    void Transaction<TValue>(Expression<Func<Settings, TValue>> expression, TValue value);

    /// <summary>
    /// Register a source observable object and property to be relayed to Settings
    /// </summary>
    IDisposable RelayPropertyFor<T, TValue>(
        T source,
        Expression<Func<T, TValue>> sourceProperty,
        Expression<Func<Settings, TValue>> settingsProperty,
        bool setInitial = false,
        TimeSpan? delay = null)
        where T : INotifyPropertyChanged;

    /// <summary>
    /// Register an Action to be called on change of the settings property.
    /// </summary>
    IDisposable RegisterPropertyChangedHandler<T>(
        Expression<Func<Settings, T>> settingsProperty,
        Action<T> onPropertyChanged);

    /// <summary>
    /// Attempts to locate and set the library path
    /// Return true if found, false otherwise
    /// </summary>
    /// <param name="forceReload">Force reload even if library is already set</param>
    bool TryFindLibrary(bool forceReload = false);

    /// <summary>
    /// Save a new library path to %APPDATA%/SmartGenealogy/library.json
    /// </summary>
    /// <param name="path"></param>
    void SetLibraryPath(string path);

    /// <summary>
    /// Enable and create settings files for portable mode
    /// Creates the ./Data directory and the `.sg-portable` marker file
    /// </summary>
    void SetPortableMode();

    //void SaveLaunchArgs(Guid packageId, IEnumerable<LaunchOption> launchArgs);
    bool IsEulaAccepted();
    void SetEulaAccepted();
}