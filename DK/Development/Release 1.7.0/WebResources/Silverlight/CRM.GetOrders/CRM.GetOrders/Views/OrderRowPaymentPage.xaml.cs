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
    public partial class OrderRowPaymentPage : ChildWindow
    {

        private OrderHeader_ViewObject _orderheader = new OrderHeader_ViewObject();

        public OrderRowPaymentPage(OrderHeader_ViewObject OrderHeader)
        {
            InitializeComponent();
            _orderheader = OrderHeader;
            grdPaymentRows.ItemsSource = _orderheader.Payments;
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

    public class DecimalSumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string _returnValue = "";

            if (value != null)
            {
                if (value.ToString() == "0")
                {
                    _returnValue = "0,00";
                }
                else
                {
                    string _svalue = value.ToString();
                    decimal _dvalue = 0.0M;
                    decimal.TryParse(_svalue, out _dvalue);
                    _returnValue = _dvalue.ToString("0.00");
                }
            }

            return _returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

