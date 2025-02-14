﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CRM2013.SkanetrafikenPlugins
{
    public class incident_Close_Pre : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);
            Entity incidentResolution = (Entity)data.Context.InputParameters["IncidentResolution"];
            EntityReference incidentReference = (EntityReference)incidentResolution["incidentid"];
            Guid incidentId = incidentReference.Id;
            Entity incident = data.Service.Retrieve("incident", incidentId, new ColumnSet("cgi_notravelinfo", "cgi_casdet_row1_cat3id", "cgi_casdet_row2_cat3id", "cgi_casdet_row3_cat3id", "cgi_casdet_row4_cat3id"));

            //kontrollera ifall användaren explicit angivit att ingen trafikinformation ska registreras, avbryt kontrollen i så fall
            if (incident.Contains("cgi_notravelinfo") && incident["cgi_notravelinfo"] != null)
            {
                if ((bool)incident["cgi_notravelinfo"])
                    return;
            }
            //Kontrollera om trafikinfo finns
            QueryByAttribute query = new QueryByAttribute("cgi_travelinformation");
            query.Attributes.Add("cgi_caseid");
            query.Values.Add(incidentId);
            query.ColumnSet = new ColumnSet();
            EntityCollection travelinforecords = data.Service.RetrieveMultiple(query);

            //avbryt kontrollen ifall det finns trafikinfo
            if (travelinforecords.Entities.Count > 0)
                return;

            //Ingen trafikinfo finns så här kontrolleras om ärendets kategori kräver trafikinfo
            if (incident.Contains("cgi_casdet_row1_cat3id") && incident["cgi_casdet_row1_cat3id"] != null)
            {
                EntityReference categoryDetailReference = (EntityReference)incident["cgi_casdet_row1_cat3id"];
                Entity categoryDetail = data.Service.Retrieve(categoryDetailReference.LogicalName, categoryDetailReference.Id, new ColumnSet("cgi_requirestravelinfo"));

                //ge ett felmeddelande ifall ärendets kategori kräver trafikinfo
                if (categoryDetail.Contains("cgi_requirestravelinfo") && categoryDetail["cgi_requirestravelinfo"] != null && (bool)categoryDetail["cgi_requirestravelinfo"])
                    throw new InvalidPluginExecutionException("Ärendet kan inte stängas utan trafikinformation. Ange trafikinformation eller kryssa för att ärendet ska vara 'Utan trafikinfo'!");
            }

            //ingen trafikinfo finns och det behövs inte heller enligt ärendets kategori så här sätts 'Utan trafikinfo' automatiskt
            incident["cgi_notravelinfo"] = true;
            data.Service.Update(incident);

        }
        #endregion
    }
}
