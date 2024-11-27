using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Core.Services;
using SmartGenealogy.ViewModels.Base;
using SmartGenealogy.Views.Dialogs;

namespace SmartGenealogy.ViewModels.Dialogs;

[View(typeof(UpdateDialog))]
[ManagedService]
[Singleton]
public partial class UpdateViewModel : ContentDialogViewModelBase
{
    private readonly ILogger<UpdateViewModel> logger;
    private readonly ISettingsManager settingsManager;


    private bool isLoaded;
}