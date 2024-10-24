using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using SmartGenealogy.Core.Attributes;

namespace SmartGenealogy.ViewModels.Dialogs;

[Singleton]
public partial class UpdateViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isUpdateAvailable;
}