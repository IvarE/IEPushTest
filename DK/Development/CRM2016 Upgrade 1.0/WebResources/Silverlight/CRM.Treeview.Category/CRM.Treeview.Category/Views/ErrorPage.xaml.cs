using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CRM.Treeview.Category.Views
{
    public partial class ErrorPage : ChildWindow
    {
        public ErrorPage(string caption, Exception ex)
        {
            InitializeComponent();

            this.Title = "Error!";
            this.txtErrorMessage.Text = string.Format("{0}\n\n{1}", caption, ex.Message);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

