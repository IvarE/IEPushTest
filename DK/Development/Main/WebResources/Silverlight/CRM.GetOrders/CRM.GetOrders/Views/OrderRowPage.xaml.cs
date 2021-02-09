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

using CRM.GetOrders.GetOrdersServiceReference;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;

namespace CRM.GetOrders.Views
{
    public partial class OrderRowPage : ChildWindow
    {

        private OrderHeader_ViewObject _orderHeader;
        private MainPage _mainpage;
        private GetOrdersServiceClient _orderclient;

        private string _accountId;
        private string _contactId;

        private ObservableCollection<CreditOrderRow> _savedOrderRows;
        private bool _saved = false;

        private void _setWaitingTrue()
        {
            Dispatcher.BeginInvoke(() => { bsyWait.IsBusy = true; });
        }

        private void _setWaitingFalse()
        {
            Dispatcher.BeginInvoke(() => { bsyWait.IsBusy = false; });
        }

        public OrderRowPage(
            MainPage mainpage,
            OrderHeader_ViewObject OrderHeader,
            GetOrdersServiceClient client,
            string AccountId, string ContactId,
            ObservableCollection<CreditOrderRow> SavedRows,
            ObservableCollection<Language> language
            )
        {
            //if (string.IsNullOrEmpty(AccountId) == false || string.IsNullOrEmpty(ContactId) == false)
            //{
            InitializeComponent();

            this.Title = string.Format("Ändra order : {0}", OrderHeader.OrderNumber);

            _mainpage = mainpage;
            _orderHeader = OrderHeader;
            _orderclient = client;
            _accountId = AccountId;
            _contactId = ContactId;
            _savedOrderRows = SavedRows;

            grdOrderRows.Columns[0].Header = GetTranslation("grd3Code", ".error.", language);
            grdOrderRows.Columns[1].Header = GetTranslation("grd3Discount", ".error.", language);
            grdOrderRows.Columns[2].Header = GetTranslation("grd3Name", ".error.", language);
            grdOrderRows.Columns[3].Header = GetTranslation("grd3Price", ".error.", language);
            grdOrderRows.Columns[4].Header = GetTranslation("grd3Quantity", ".error.", language);
            grdOrderRows.Columns[5].Header = GetTranslation("grd3RowTotal", ".error.", language);
            grdOrderRows.Columns[6].Header = GetTranslation("grd3Change", ".error.", language);
            grdOrderRows.Columns[7].Header = "Orsak";//GetTranslation("grd3Reason", ".error.", language);

            try
            {
                // Here we check if there are a credit order row for the order that was selected in main window ( clicked upon )
                // That is used to decide if its possible to credit the order.
                // BTW this will be changed.
                //_saved = false;
                /*
                if (_savedOrderRows != null)
                {
                    CreditOrderRow _savedorder = _savedOrderRows.FirstOrDefault(x => x.OrderNumber == _orderHeader.OrderNumber);
                    if (_savedorder != null)
                    {
                        _saved = true;
                        OKButton.IsEnabled = false;
                    }
                }
                */

                // These are the order rows that we see in the gui list, and that could be credited
                /*
                foreach (orderrow _row in _orderHeader.OrderItems)
                {
                    if (_savedOrderRows != null)
                    {
                        _row.AmountChange = "";

                        // This is a possible credit for corresponding order row
                        CreditOrderRow _savedrow = _savedOrderRows.FirstOrDefault(x => x.OrderNumber == _orderHeader.OrderNumber && x.Productnumber == _row.Code);
                        if (_savedrow != null)
                        {
                            _row.AmountChange = _savedrow.Sum; // THIS PROB NEEDS TO BE OUTCOMMENTED
                        }
                        /*
                        if (_saved != false)
                        {
                            _row.IsRowEnabled = false;
                        }
                        else
                        {
                        */
                //_row.AmountChange = "";
                //_row.IsRowEnabled = true;
                //}
                //}
                //else
                //{
                //_row.AmountChange = "";
                //_row.IsRowEnabled = true;
                //}

                // WILL ABOVE BE A PROLEM?? ASK MAX. SINCE A ORDERROW, THAT HAS BEEN CREDITED SHOWS UP, ENABLED, WITH AMOUNT THAT WAS! CREDITED. DOESNT RELLY FIT INTO NEW MODEL? NO IT DOESNT
                // Q1 DOES THIS SYS HANDLE MULTIPLE CREDITS WHEN SAVED? YES
                // Q2 WILL THIS SYS AVOID THE OLD CREDIT AMOUNTS IF LEFT? NO
                // Q3 WILL WE NEED NOT TO SHOW EARLIER CREDIT AMOUNTS? GUESS SO YES
                //}

                //_orderHeader.OrderItems.ToList().ForEach(x => x.IsRowEnabled = true);


                // Clear rows cells if window opened again after a credit has been done.
                if (_savedOrderRows != null)
                {
                    foreach (OrderRow_ViewObject _row in _orderHeader.OrderItems)
                    {
                        _row.AmountChange = "";
                        _row.Reason = "";
                    }
                }

                grdOrderRows.ItemsSource = _orderHeader.OrderItems;
            }
            catch (Exception ex)
            {
                _mainpage.ShowError(ex, "OrderRowPage");
            }
            //}
            //else
            //{
            //_mainpage.ShowErrorMessage("Parametrarna accountid och contactid är tomma. En av dem behöver vara initialiserad.", "OrderRowPage");
            //}
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (_saved == false)
                {
                    if (_checkSaveOk())
                    {
                        this.OKButton.IsEnabled = false;
                        _setWaitingTrue();
                        _creditOrder();
                    }
                }
            }
            catch (Exception ex)
            {
                _setWaitingFalse();
                _mainpage.ShowError(ex, "OKButton_Click");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private bool _checkSaveOk()
        {
            bool _returnValue = true;

            try
            {
                decimal _totalAmount = _orderHeader.AmountThatCanBeCredit;
                decimal _totalCreditAmount = 0;

                // This is the value we entered in GUI to credit per row
                foreach (OrderRow_ViewObject _row in _orderHeader.OrderItems)
                {
                    if (!string.IsNullOrEmpty(_row.AmountChange))
                    {
                        decimal _amount = 0;
                        decimal.TryParse(_row.AmountChange, out _amount);
                        _totalCreditAmount += _amount;
                    }
                }

                if (_totalCreditAmount == 0)
                {
                    _mainpage.ShowErrorMessage("Finns inget belopp att kreditera", "Belopp");
                    _returnValue = false;
                }

                if (_totalCreditAmount > _totalAmount)
                {
                    _mainpage.ShowErrorMessage(string.Format("Totalt krediterat belopp {0} är större än belopp som kan krediteras {1}\nOBS Du kan ej kreditera värdekoder/ersättningar.", _totalCreditAmount.ToString(), _totalAmount.ToString()), "Belopp");
                    _returnValue = false;
                }
            }
            catch (Exception ex)
            {
                _returnValue = false;
                _setWaitingFalse();
                _mainpage.ShowError(ex, "_checkSaveOk");
            }

            return _returnValue;
        }

        private void _creditOrder()
        {
            try
            {
                CreditOrderRequest _request = new CreditOrderRequest();
                _request.CreditRows = new ObservableCollection<CreditRow>();
                foreach (OrderRow_ViewObject _row in _orderHeader.OrderItems)
                {
                    if (!string.IsNullOrEmpty(_row.AmountChange))
                    {
                        CreditRow _cr = new CreditRow();
                        _cr.OrderNumber = _orderHeader.OrderNumber;
                        _cr.ProductNumber = _row.Code;
                        _cr.Sum = _row.AmountChange;
                        _cr.Quantity = _row.Quantity.ToString();
                        _cr.AccountId = _accountId;
                        _cr.ContactId = _contactId;
                        _cr.Reason = _row.Reason; // TODO is this wrong place?
                        _cr.CreatedBy = _mainpage.UserFullName;
                        _request.CreditRows.Add(_cr);
                    }
                }

                _orderclient.CreditOrderCompleted += _orderclient_CreditOrderCompleted;
                _orderclient.CreditOrderAsync(_request);
            }
            catch (Exception ex)
            {
                _setWaitingFalse();
                _mainpage.ShowError(ex, "_creditOrder");
            }
        }

        private void _orderclient_CreditOrderCompleted(object sender, CreditOrderCompletedEventArgs e)
        {
            try
            {
                bool _error = false;

                if (e.Result != null)
                {
                    GetCreditOrderResponse _response = e.Result;
                    foreach (CreditOrderMessage _message in _response.CreditOrderMessage)
                    {
                        if (_message.Success.ToUpper() == "FALSE")
                        {
                            string _display = string.Format("Produkten {0}\n\nEhandel fel:\n{1}", _message.ProductNumber, _message.Message);
                            _mainpage.ShowErrorMessage(_display, "Kreditera ordernr:" + _message.OrderNumber);
                            _error = true;
                        }
                    }
                }

                _setWaitingFalse();
                if (_error == false)
                    this.DialogResult = true;
            }
            catch (Exception ex)
            {
                _setWaitingFalse();
                _mainpage.ShowError(ex, "_orderclient_CreditOrderCompleted");
            }
            finally
            {
                _orderclient.CreditOrderCompleted -= _orderclient_CreditOrderCompleted;
            }
        }

        private string GetTranslation(string tag, string error, ObservableCollection<Language> localizedList)
        {
            if (localizedList == null)
                return "error";

            Language found = localizedList.Where<Language>(x => x.Tag == tag).FirstOrDefault();

            if (found != null)
                return found.Name;
            else
                return error;
        }

        private void BT_Payment_Click(object sender, RoutedEventArgs e)
        {
            OrderRowPaymentPage _window = new OrderRowPaymentPage(_orderHeader);
            _window.Show();
        }

        private void BT_ShippingAddress_Click(object sender, RoutedEventArgs e)
        {
            ShippingAddressPage _window = new ShippingAddressPage(_orderHeader);
            _window.Show();
        }


    }

    public class DecimalConverter : IValueConverter
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

