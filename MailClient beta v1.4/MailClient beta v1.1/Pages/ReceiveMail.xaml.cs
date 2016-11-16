using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MailClient_beta_v1._1.Pages
{
    /// <summary>
    /// Interaction logic for ReceiveMail.xaml
    /// </summary>
    public partial class ReceiveMail : UserControl
    {
        public ReceiveMail()
        {
            InitializeComponent();
        }

        private void default_Loaded(object sender, RoutedEventArgs e)
        {
            if(defaultpage.SelectedSource.Equals("/Content/ReceiveMailDefault.xaml"))
            {
                answer.Visibility = Visibility.Hidden;
                forward.Visibility = Visibility.Hidden;
            }
            else
            {
                answer.Visibility = Visibility.Visible;
                forward.Visibility = Visibility.Visible;
            }
        }

    }
    
}
