using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGenealogy.Mobile.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    public HomeViewModel()
    {

    }

    internal async Task InitializeAsync()
    {
        // Delay on first load until window loads
        await Task.Delay(1000);
        await FetchAsync();
    }

    private async Task FetchAsync()
    {

    }
}