using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartGenealogy.ViewModels;

public partial class ConsoleViewModel : ObservableObject, IDisposable, IAsyncDisposable
{


    public void Dispose()
    {
    }

    public async ValueTask DisposeAsync()
    {
    }
}