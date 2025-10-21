namespace SmartGenealogy.Extensions;

public class FlowDirectionManager : INotifyPropertyChanged
{
    private FlowDirection _flowDirection = FlowDirection.LeftToRight;
    public FlowDirection this[string key] => FlowDirection;

    public static FlowDirectionManager Instance { get; }

    static FlowDirectionManager()
    {
        Instance = new FlowDirectionManager();
    }

    public FlowDirection FlowDirection
    {
        get => _flowDirection;
        set
        {
            if (value == _flowDirection)
            {
                return;
            }
            _flowDirection = value;
            FlowDirectionManager.Instance.FlowDirection = value;
            NotifyPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}