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

    #endregion

    #endregion

    #region Commands

    public ICommand CancelDialogCommand { get; set; }
    public ICommand DismissDialogCommand { get; set; }
    public ICommand ResetReturnValueCommand { get; set; }

    #endregion

    #endregion

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

    #endregion

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

    #endregion
}