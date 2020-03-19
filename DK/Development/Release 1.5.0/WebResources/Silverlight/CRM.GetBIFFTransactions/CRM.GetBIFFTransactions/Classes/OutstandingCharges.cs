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

namespace CRM.GetBIFFTransactions
{
    public class OutstandingCharges
    {

        //<Message>Found no registered card in e-commerce with cardnumber 3296369962.</Message>
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        //<HasOutstandingCharge>false</HasOutstandingCharge>
        private bool _hasOutstandingCharge;
        public bool HasOutstandingCharge
        {
            get { return _hasOutstandingCharge; }
            set { _hasOutstandingCharge = value; }
        }

        //<HasExpiredCharge>false</HasExpiredCharge>
        private bool _hasExpiredCharge;
        public bool HasExpiredCharge
        {
            get { return _hasExpiredCharge; }
            set { _hasExpiredCharge = value; }
        }

        //<Amount>0</Amount>
        private string _amount;
        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        //<ErrorMessage>Found no registered card in e-commerce with cardnumber 3296369962.</ErrorMessage>
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        //<StatusCode>400</StatusCode>
        private int _statusCode;
        public int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public string ShowMessage
        {
            get
            {
                if (_amount == "0")
                {
                    return string.Format("Nej. Belopp : {0} kr.", _amount);
                }
                else
                {
                    return string.Format("Ja. Belopp : {0} kr.", _amount);
                }
            }
        }

    }
}
