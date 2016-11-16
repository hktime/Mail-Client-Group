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
    /// Interaction logic for ReceiveMailSettings.xaml
    /// </summary>
    public partial class ReceiveMailSettings : UserControl
    {
        public ReceiveMailSettings()
        {
            InitializeComponent();
        }

        private void 立即从服务器更新邮件到本地客户端_Click(object sender, RoutedEventArgs e)
        {
            ReceiveProgressBar.Visibility = Visibility.Visible;
            //添加按钮点击事件处理程序↓

            //-----------------------------
            ReceiveProgressBar.Visibility = Visibility.Hidden;
        }
    }
}
