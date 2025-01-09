using XCalendar.Core.Collections;

namespace SmartGenealogy.Views.Popups;

public partial class ConstructListDialogPopup : Popup
{
    #region Properties

    private List<object> _InitialItems { get; }

    #region Bindable Properties

    public ObservableRangeCollection<object> ReturnValueItems
    {
        get => (ObservableRangeCollection<object>)GetValue(ReturnValueItemsProperty);
        set => SetValue(ReturnValueItemsProperty, value);
    }

    public List<object> AvailableItems
    {
        get => (List<object>)GetValue(AvailableItemsProperty);
        set => SetValue(AvailableItemsProperty, value);
    }

    #region Bindable Properties Initializers

    public static readonly BindableProperty ReturnValueItemsProperty = BindableProperty.Create(nameof(ReturnValueItems), typeof(object), typeof(ConstructListDialogPopup), defaultValueCreator: ReturnValueItemsDefaultValueCreator);
    public static readonly BindableProperty AvailableItemsProperty = BindableProperty.Create(nameof(AvailableItems), typeof(IEnumerable), typeof(ConstructListDialogPopup));

    #endregion Bindable Properties Initializers

    #endregion Bindable Properties

    #region Commands

    public ICommand CloseDialogCommand { get; set; }
    public ICommand CancelDialogCommand { get; set; }
    public ICommand ResetReturnValueItemsCommand { get; set; }
    public ICommand AddItemCommand { get; set; }
    public ICommand RemoveItemCommand { get; set; }
    public ICommand ClearReturnValueItemsCommand { get; set; }

    #endregion Commands

    #endregion Properties

    #region Constructors

    public ConstructListDialogPopup(IEnumerable availableItems)
        : this(new List<object>(), availableItems)
    {
    }

    public ConstructListDialogPopup(IEnumerable initialItems, IEnumerable availableItems)
    {
        CloseDialogCommand = new Command(() => Close(new List<object>(ReturnValueItems)));
        CancelDialogCommand = new Command(CancelDialog);
        ResetReturnValueItemsCommand = new Command(ResetReturnValueItems);
        AddItemCommand = new Command<object>(AddItem);
        RemoveItemCommand = new Command<object>(RemoveItem);
        ClearReturnValueItemsCommand = new Command(ClearReturnValueItems);

        InitializeComponent();

        _InitialItems = initialItems.Cast<object>().ToList(); ;
        this.AvailableItems = availableItems.Cast<object>().ToList();

        ResetReturnValueItems();
        ResultWhenUserTapsOutsideOfPopup = _InitialItems;
    }

    #endregion Constructors

    #region Methods

    public void CancelDialog()
    {
        Close(_InitialItems);
    }

    public void ResetReturnValueItems()
    {
        ReturnValueItems.ReplaceRange(_InitialItems);
    }

    public void AddItem(object item)
    {
        if (item != null)
        {
            ReturnValueItems.Add(item);
        }
    }

    public void RemoveItem(object item)
    {
        if (item != null)
        {
            ReturnValueItems.Remove(item);
        }
    }

    public void ClearReturnValueItems()
    {
        ReturnValueItems.Clear();
    }

    #region Bindable Properties Methods

    private static object ReturnValueItemsDefaultValueCreator(BindableObject bindable)
    {
        return new ObservableRangeCollection<object>();
    }

    #endregion Bindable Properties Methods

    #endregion Methods
}