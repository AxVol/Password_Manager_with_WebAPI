using System.Windows;

namespace Desktop_client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DIcontainer.Init();

            base.OnStartup(e);
        }
    }
}
