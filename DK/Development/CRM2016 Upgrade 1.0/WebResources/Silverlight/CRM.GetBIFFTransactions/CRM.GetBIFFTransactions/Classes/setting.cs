﻿using System;
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

namespace CRM.GetBIFFTransactions
{
    public class setting
    {
        private string _crmcardservice;
        [Xrm("cgi_crmcardservice")]
        public string Crmcardservice
        {
            get { return _crmcardservice; }
            set { _crmcardservice = value; }
        }

		private string _crmuri;
		[Xrm("cgi_crmuri")]
		public string CrmUri {
		  get { return _crmuri; }
		  set { _crmuri = value; }
		}
    }
}
