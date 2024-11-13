using SmartGenealogy.Core.Attributes;
using SmartGenealogy.Views;

namespace SmartGenealogy.ViewModels;

[View(typeof(MainWindow))]
public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
}