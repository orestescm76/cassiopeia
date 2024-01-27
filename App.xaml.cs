using Cassiopeia.VM;
using Microsoft.Maui.Platform;
using System.Diagnostics;
using System.Globalization;

namespace Cassiopeia
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainVM VM = new MainVM(this);
            MainPage = new AppShell();
            MainPage.BindingContext = VM;
        }
    }
}
