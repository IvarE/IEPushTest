using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM2013.SkanetrafikenPlugins
{
    public class incident_Close_Pre : IPlugin
    {
        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            plugindata _data = new plugindata(serviceProvider);
            //StringBuilder sb = new StringBuilder();
            //try
            //{
            Entity incidentResolution = (Entity)_data.Context.InputParameters["IncidentResolution"];
            EntityReference incidentReference = (EntityReference)incidentResolution["incidentid"];
            Guid incidentId = incidentReference.Id;
            Entity incident = _data.Service.Retrieve("incident", incidentId, new Microsoft.Xrm.Sdk.Query.ColumnSet("cgi_notravelinfo", "cgi_casdet_row1_cat3id", "cgi_casdet_row2_cat3id", "cgi_casdet_row3_cat3id", "cgi_casdet_row4_cat3id"));



            //kontrollera ifall användaren explicit angivit att ingen trafikinformation ska registreras, avbryt kontrollen i så fall
            if (incident.Contains("cgi_notravelinfo") && incident["cgi_notravelinfo"] != null)
            {
                if ((bool)incident["cgi_notravelinfo"])
                    return;
            }
            //Kontrollera om trafikinfo finns
            QueryByAttribute _query = new QueryByAttribute("cgi_travelinformation");
            _query.Attributes.Add("cgi_caseid");
            _query.Values.Add(incidentId);
            _query.ColumnSet = new ColumnSet();
            EntityCollection _travelinforecords = _data.Service.RetrieveMultiple(_query);

            //avbryt kontrollen ifall det finns trafikinfo
            if (_travelinforecords.Entities.Count > 0)
                return;

            //Ingen trafikinfo finns så här kontrolleras om ärendets kategori kräver trafikinfo
            if (incident.Contains("cgi_casdet_row1_cat3id") && incident["cgi_casdet_row1_cat3id"] != null)
            {
                EntityReference categoryDetailReference = (EntityReference)incident["cgi_casdet_row1_cat3id"];
                Entity categoryDetail = _data.Service.Retrieve(categoryDetailReference.LogicalName, categoryDetailReference.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet("cgi_requirestravelinfo"));

                //ge ett felmeddelande ifall ärendets kategori kräver trafikinfo
                if (categoryDetail.Contains("cgi_requirestravelinfo") && categoryDetail["cgi_requirestravelinfo"] != null && (bool)categoryDetail["cgi_requirestravelinfo"])
                    throw new InvalidPluginExecutionException("Ärendet kan inte stängas utan trafikinformation. Ange trafikinformation eller kryssa för att ärendet ska vara 'Utan trafikinfo'!");
            }

            //ingen trafikinfo finns och det behövs inte heller enligt ärendets kategori så här sätts 'Utan trafikinfo' automatiskt
            incident["cgi_notravelinfo"] = true;
            _data.Service.Update(incident);

            //}
            //catch (Exception ex)
            //{
            //    sb.AppendFormat("#EX:{0}#", ex.Message);
            //    throw new InvalidPluginExecutionException("Ärendet kan inte stängas utan trafikinformation." + sb.ToString());
            //}
        }
    }
}
