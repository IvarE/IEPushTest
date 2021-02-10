using CGIXrm;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CRM.GetOrders
{
    public class CustomerOrder
    {

        public string OrderNumber { get; set; }
        public DateTime Orderdate { get; set; }
        public string strOrderDate { get { return Orderdate.Date.ToString("yyyy-MM-dd"); } }
        public string CustomerId { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal OtherPayment { get; set; }
        public decimal OrderTotelVAT { get; set; }
        public int OrderStatus { get; set; }

        private decimal _CreditAmount;
        public decimal CreditAmount 
        {
            get { return _CreditAmount; }
            set
            {
                if (value != 0)
                {
                    if (IsCellEnabled)
                        IsNewValue = true;
                    _CreditAmount = value;
                }
            }
        }

        public string ReferenceId { get; set; } //Return value from service
        public bool IsCellEnabled { get { return CreditAmount == 0; } }

        private bool _IsNewValue = false;
        public bool IsNewValue
        {
            get { return _IsNewValue; }
            set { _IsNewValue = value; }
        }

    }
}
