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
    public class RechargeCard
    {
        //<Success>false</Success>
        private bool _success;
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        //<Message>Card has no expired charges.</Message>
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        //<ErrorMessage/>
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        //<StatusCode>200</StatusCode>
        private int _statusCode;
        public int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }
    }
}
