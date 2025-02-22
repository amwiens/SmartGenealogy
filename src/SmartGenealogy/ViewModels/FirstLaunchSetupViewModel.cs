using CommunityToolkit.Mvvm.ComponentModel;

using Injectio.Attributes;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views;

namespace SmartGenealogy.ViewModels;

[View(typeof(FirstLaunchSetupWindow))]
[ManagedService]
[RegisterSingleton<FirstLaunchSetupViewModel>]
public partial class FirstLaunchSetupViewModel : DisposableViewModelBase
{
    [ObservableProperty]
    private bool eulaAccepted;

    public FirstLaunchSetupViewModel(ISettingsManager settingsManager)
    {

    }

    public override void OnLoaded()
    {
        base.OnLoaded();
    }
}