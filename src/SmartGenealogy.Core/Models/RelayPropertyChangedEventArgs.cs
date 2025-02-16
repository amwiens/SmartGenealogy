using System.ComponentModel;

namespace SmartGenealogy.Core.Models;

public class RelayPropertyChangedEventArgs : PropertyChangedEventArgs
{
    public bool IsRelay { get; }

    /// <inheritdoc />
    public RelayPropertyChangedEventArgs(string? propertyName, bool isRelay = false)
        : base(propertyName)
    {
        IsRelay = isRelay;
    }
}