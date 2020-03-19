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

using CGIXrm;
using System.Collections.ObjectModel;
using CRM.GetOrders.GetOrdersServiceReference;
using System.Reflection;
using System.Linq;

namespace CRM.GetOrders
{
    public class ShippingAddress_ViewObject : XrmBaseNotify
    {
        /*
         *    <Address>Gata 1B</Address>
       <City>Stad</City>
       <Co>C/O</Co>
       <CompanyName>Företagsnamn (om beställaren är en företagskund)</CompanyName>
       <Country>Land</Country>
       <CellPhoneNumber>0701234567</CellPhoneNumber>
       <Email>E-post</Email>
       <FirstName>Förnamn</FirstName>
       <LastName>Efternamn</LastName>
       <PostalCode>43125</PostalCode>
       <ExtraInfo>Här kan extra information läggas framöver</ExtraInfo>
         * */

        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; OnPropertyChanged("Address"); }
        }

        private string _City;

        public string City
        {
            get { return _City; }
            set { _City = value; OnPropertyChanged("City"); }
        }

        private string _Co;

        public string Co
        {
            get { return _Co; }
            set { _Co = value; OnPropertyChanged("Co"); }
        }

        private string _CompanyName;

        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; OnPropertyChanged("CompanyName"); }
        }

        private string _Country;

        public string Country
        {
            get { return _Country; }
            set { _Country = value; OnPropertyChanged("Address"); }
        }

        private string _CellPhoneNumber;

        public string CellPhoneNumber
        {
            get { return _CellPhoneNumber; }
            set { _CellPhoneNumber = value; OnPropertyChanged("CellPhoneNumber"); }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged("Email"); }
        }

        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; OnPropertyChanged("FirstName"); }
        }

        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; OnPropertyChanged("LastName"); }
        }

        private string _PostalCode;

        public string PostalCode
        {
            get { return _PostalCode; }
            set { _PostalCode = value; OnPropertyChanged("PostalCode"); }
        }

        private string _ExtraInfo;

        public string ExtraInfo
        {
            get { return _ExtraInfo; }
            set { _ExtraInfo = value; OnPropertyChanged("ExtraInfo"); }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }
}
