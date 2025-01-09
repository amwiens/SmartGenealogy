namespace SmartGenealogy.Views.Popups;

public partial class SelectItemDialogPopup : Popup
{
    #region Properties

    private object _InitialValue { get; }

    #region Bindable Properties

    public object ReturnValue
    {
        get => (object)GetValue(ReturnValueProperty);
        set => SetValue(ReturnValueProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    #region Bindable Properties Initializers

    public static readonly BindableProperty ReturnValueProperty = BindableProperty.Create(nameof(ReturnValue), typeof(object), typeof(SelectItemDialogPopup), null);
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(SelectItemDialogPopup), null);

    #endregion Bindable Properties Initializers

    #endregion Bindable Properties

    #region Commands

    public ICommand CloseDialogCommand { get; set; }
    public ICommand CancelDialogCommand { get; set; }
    public ICommand ResetReturnValueCommand { get; set; }

    #endregion Commands

    #endregion Properties

    #region Constructors

    public SelectItemDialogPopup(object InitialValue, IEnumerable ItemsSource)
    {
        CloseDialogCommand = new Command(() => Close(ReturnValue));
        CancelDialogCommand = new Command(CancelDialog);
        ResetReturnValueCommand = new Command(ResetReturnValue);

        InitializeComponent();

        _InitialValue = InitialValue;
        this.ItemsSource = ItemsSource;

        ResetReturnValue();
        ResultWhenUserTapsOutsideOfPopup = _InitialValue;
    }

    #endregion Constructors

    #region Methods

    public void CancelDialog()
    {
        Close(_InitialValue);
    }

    public void ResetReturnValue()
    {
        ReturnValue = _InitialValue;
    }

    #endregion Methods
}