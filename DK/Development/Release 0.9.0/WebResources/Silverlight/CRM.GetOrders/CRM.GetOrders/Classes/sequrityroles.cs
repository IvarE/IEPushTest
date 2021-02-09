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
    public class sequrityroles : XrmBaseEntity
    {

        //<attribute name="roleid" />
        private Guid _roleid;
        [Xrm("roleid")]
        public Guid Roleid
        {
            get { return _roleid; }
            set { _roleid = value; }
        }

        //<attribute name="name" />
        private string _name;
        [Xrm("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

    }
}
