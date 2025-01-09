namespace SmartGenealogy.Views.Popups;

public partial class ColorDialogPopup : Popup
{
    #region Properties

    private Color InitialSelectedColor { get; }

    #region Bindable Properties

    public Color SelectedColor
    {
        get { return (Color)GetValue(SelectedColorProperty); }
        set { SetValue(SelectedColorProperty, value); }
    }

    #region Bindable Properties Initializers

    public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(object), typeof(ColorDialogPopup), null);

    #endregion Bindable Properties Initializers

    #endregion Bindable Properties

    #region Commands

    public ICommand CancelDialogCommand { get; set; }
    public ICommand DismissDialogCommand { get; set; }
    public ICommand ResetReturnValueCommand { get; set; }

    #endregion Commands

    #endregion Properties

    #region Constructors

    public ColorDialogPopup()
        : this(new Color())
    {
    }

    public ColorDialogPopup(Color InitialSelectedColor)
    {
        CancelDialogCommand = new Command(CancelDialog);
        DismissDialogCommand = new Command(DismissDialog);
        ResetReturnValueCommand = new Command(ResetReturnValue);

        InitializeComponent();

        this.InitialSelectedColor = InitialSelectedColor;
        ResultWhenUserTapsOutsideOfPopup = InitialSelectedColor;
        SelectedColor = InitialSelectedColor;
    }

    #endregion Constructors

    #region Methods

    public void CancelDialog()
    {
        Close(InitialSelectedColor);
    }

    public void DismissDialog()
    {
        Close(SelectedColor);
    }

    public void ResetReturnValue()
    {
        SelectedColor = InitialSelectedColor;
    }

    #endregion Methods
}