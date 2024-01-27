using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassiopeia.VM
{
    public class MainVM
    {
        Application App;
        public ObservableCollections.ObservableList<int> List { get; set; }
        public Command CommandClose { get; set; }
        public MainVM(Application app)
        {
            CommandClose = new Command(CloseApplication);
            App = app;
            List = [1, 2, 3, 4,5,6,7];
        }

        private void CloseApplication(object obj)
        {
            App.Quit();
        }
    }
}
