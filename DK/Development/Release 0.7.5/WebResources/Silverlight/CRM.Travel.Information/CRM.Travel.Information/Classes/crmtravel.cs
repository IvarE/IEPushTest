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

namespace CRM.Travel.Information
{
    public class crmtravel : XrmBaseEntity
    {
        
        //<attribute name="cgi_travelinformationid"/>
        [Xrm("cgi_travelinformationid")]
        public Guid TravelInformationId { get; set; }

        //<attribute name="cgi_travelinformation"/>
        [Xrm("cgi_travelinformation")]
        public string Name { get; set; }

        //<attribute name="cgi_transport"/>
        [Xrm("cgi_transport")]
        public string Transport { get; set; }

        //<attribute name="cgi_tour"/>
        [Xrm("cgi_tour")]
        public string Tour { get; set; }

        //<attribute name="cgi_stop"/>
        [Xrm("cgi_stop")]
        public string Stop { get; set; }

        //<attribute name="cgi_start"/>
        [Xrm("cgi_start")]
        public string Start { get; set; }

        //<attribute name="cgi_startplanned"/>
        [Xrm("cgi_startplanned")]
        public DateTime Startplanned { get; set; }

        //<attribute name="cgi_startactual"/>
        [Xrm("cgi_startactual")]
        public string Startactual { get; set; }

        //<attribute name="cgi_line"/>
        [Xrm("cgi_line")]
        public string Line { get; set; }

        //<attribute name="cgi_directiontext"/>
        [Xrm("cgi_directiontext")]
        public string Directiontext { get; set; }

        //<attribute name="cgi_contractor"/>
        [Xrm("cgi_contractor")]
        public string Contractor { get; set; }

        //<attribute name="cgi_city"/>
        [Xrm("cgi_city")]
        public string City { get; set; }

        //<attribute name="cgi_caseid"/>
        [Xrm("cgi_caseid", DecodePart=XrmDecode.Value)]
        public Guid Caseid { get; set; }

        //<attribute name="cgi_arivalplanned"/>
        [Xrm("cgi_arivalplanned")]
        public DateTime Arivalplanned { get; set; }

        //<attribute name="cgi_arivalactual"/>
        [Xrm("cgi_arivalactual")]
        public string Arivalactual { get; set; }

        //<attribute name='cgi_deviationmessage'/>
        [Xrm("cgi_deviationmessage")]
        public string Deviationmessage { get; set; }

        //<attribute name='cgi_displaytext'/>
        [Xrm("cgi_displaytext")]
        public string Displaytext { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string GetDateStartplanned
        {
            get
            {
                if (Startplanned != null)
                {
                    DateTime _dt = new DateTime();
                    string _sdt = Startplanned.ToString();
                    DateTime.TryParse(_sdt, out _dt);
                    return _dt.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public string GetDateArivalplanned
        {
            get
            {
                if (Arivalplanned != null)
                {
                    DateTime _dt = new DateTime();
                    string _sdt = Arivalplanned.ToString();
                    DateTime.TryParse(_sdt, out _dt);
                    return _dt.ToString();
                }
                else
                {
                    return "";
                }
            }
        }




    }
}
