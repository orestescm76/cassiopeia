namespace Cassiopeia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void Shell_BindingContextChanged(object sender, EventArgs e)
        {
            //set binding manually
            MainPage.BindingContext = BindingContext;
        }
    }
}
