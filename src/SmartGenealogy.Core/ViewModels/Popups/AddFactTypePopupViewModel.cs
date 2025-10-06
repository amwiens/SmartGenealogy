namespace SmartGenealogy.Core.ViewModels.Popups;

/// <summary>
/// Add fact type popup view model.
/// </summary>
/// <param name="factTypeRepository">Fact type repository</param>
/// <param name="popupService">PopupService</param>
public partial class AddFactTypePopupViewModel(FactTypeRepository factTypeRepository, IPopupService popupService) : ObservableObject
{
    [ObservableProperty]
    private string? _name = string.Empty;

    [ObservableProperty]
    private string? _abbreviation = string.Empty;

    [ObservableProperty]
    private string? _gedcomTag = string.Empty;

    [ObservableProperty]
    private bool _useValue = false;

    [ObservableProperty]
    private bool _useDate = false;

    [ObservableProperty]
    private bool _usePlace = false;

    [ObservableProperty]
    private string? _sentence = string.Empty;

    /// <summary>
    /// Cancel
    /// </summary>
    [RelayCommand]
    private async Task Add()
    {
        var factType = new FactType
        {
            Name = Name,
            Abbreviation = Abbreviation,
            GedcomTag = GedcomTag,
            UseValue = UseValue,
            UseDate = UseDate,
            UsePlace = UsePlace,
            Sentence = Sentence,
            IsBuiltIn = false
        };

        var factTypeId = await factTypeRepository.SaveItemAsync(factType);

        await popupService.ClosePopupAsync(Shell.Current);
    }

    /// <summary>
    /// Cancel
    /// </summary>
    [RelayCommand]
    private async Task Cancel()
    {
        await popupService.ClosePopupAsync(Shell.Current);
    }
}