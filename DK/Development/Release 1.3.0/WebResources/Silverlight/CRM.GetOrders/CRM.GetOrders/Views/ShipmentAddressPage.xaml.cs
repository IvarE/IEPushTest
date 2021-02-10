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

using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;

namespace CRM.GetOrders.Views
{
    public partial class ShippingAddressPage : ChildWindow
    {

        private OrderHeader_ViewObject _orderheader = new OrderHeader_ViewObject();

        public ShippingAddressPage(OrderHeader_ViewObject OrderHeader)
        {
            InitializeComponent();
            _orderheader = OrderHeader;
            grdShippingAddressRows.ItemsSource = _orderheader.ShippingAddress;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void grdShippingAddressRows_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}

