﻿using System;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void 目标邮箱_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void 发送_Click(object sender, RoutedEventArgs e)
        {
            SendProgressBar.Visibility = Visibility.Visible;
            //添加按钮点击事件处理程序↓

            //-----------------------------
            SendProgressBar.Visibility = Visibility.Hidden;
        }
    }
}
