using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MailClient_beta_v1._1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            LoginWindow window = new LoginWindow();
            bool? dialogResult = window.ShowDialog();
            if ((dialogResult.HasValue == true) &&
                (dialogResult.Value == true))
            {
                base.OnStartup(e);
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            }
            else
            {
                this.Shutdown();
            }
        }
    }
}
