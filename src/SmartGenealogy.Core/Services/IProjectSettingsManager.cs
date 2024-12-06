using System.ComponentModel;
using System.Linq.Expressions;

using SmartGenealogy.Core.Models;
using SmartGenealogy.Core.Models.FileInterfaces;
using SmartGenealogy.Core.Models.Settings;

namespace SmartGenealogy.Core.Services;

public interface IProjectSettingsManager
{
    DirectoryPath ProjectDir { get; }

    ProjectSettings Settings { get; }

    /// <summary>
    /// Event fired when the library directory is changed
    /// </summary>
    event EventHandler<string>? ProjectDirChanged;

    /// <summary>
    /// Event fired when a property of Settings is changed
    /// </summary>
    event EventHandler<RelayPropertyChangedEventArgs>? SettingsPropertyChanged;

    /// <summary>
    /// Event fired when Settings are loaded from disk
    /// </summary>
    event EventHandler? Loaded;

    /// <summary>
    /// Register a handler that fires once when ProjectDir is first set.
    /// Will fire instantly if it is already set.
    /// </summary>
    void RegisterOnProjectDirSet(Action<string> handler);

    /// <summary>
    /// Return a ProjectSettingsTransaction that can be used to modify Settings
    /// Saves on Dispose
    /// </summary>
    ProjectSettingsTransaction BeginTransaction();

    /// <summary>
    /// Execute a function that modifies ProjectSettings
    /// Commits changes after the function returns.
    /// </summary>
    /// <param name="func">Function accepting ProjectSettings to modify</param>
    /// <param name="ignoreMissingProjectDir">Ignore missing project dir when committing changes</param>
    void Transaction(Action<ProjectSettings> func, bool ignoreMissingProjectDir = false);

    /// <summary>
    /// Modify a settings property by expression and commit changes.
    /// this will notify listeners of SettingsPropertyChanged.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="expression"></param>
    /// <param name="value"></param>
    void Transaction<TValue>(Expression<Func<ProjectSettings, TValue>> expression, TValue value);

    /// <summary>
    /// Register a source observable object and property to be relayed to ProjectSettings
    /// </summary>
    IDisposable RelayPropertyFor<T, TValue>(
        T source,
        Expression<Func<T, TValue>> sourceProperty,
        Expression<Func<ProjectSettings, TValue>> settingsProperty,
        bool setInitial = false,
        TimeSpan? delay = null)
        where T : INotifyPropertyChanged;

    /// <summary>
    /// Register an Action to be called on change of the settings property.
    /// </summary>
    IDisposable RegisterPropertyChangedHandler<T>(
        Expression<Func<ProjectSettings, T>> settingsProperty,
        Action<T> onPropertyChanged);

    /// <summary>
    /// Attempts to locate and set the project path
    /// Return true if found, false otherwise
    /// </summary>
    /// <param name="forceReload">Force reload even if project is already set</param>
    bool TryFindProject(bool forceReload = false);

    /// <summary>
    /// Save a new library path to %APPDATA%/SmartGenealogy/*.sgproj
    /// </summary>
    /// <param name="path"></param>
    void SetProjectPath(string path);
}