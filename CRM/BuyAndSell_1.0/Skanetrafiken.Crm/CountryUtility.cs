using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Caching;

namespace Skanetrafiken.Crm
{
    public class CountryUtility
    {
        private static string _countryList = "CountryList";
        
        public static EntityReference GetEntityRefForCountryCode(Endeavor.Crm.Plugin.LocalPluginContext localContext, string CountryCode)
        {
            List<CountryPlaceholder> placeholderList = null;
            
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<CountryPlaceholder>));

                string stringGuidList = httpContext.Cache.Get(_countryList) as string;
                if (stringGuidList == null)
                {
                    IList<CountryEntity> list = XrmRetrieveHelper.RetrieveMultiple<CountryEntity>(localContext, new ColumnSet(CountryEntity.Fields.edp_CountryCode), new FilterExpression());
                    placeholderList = new List<CountryPlaceholder>();
                    foreach (CountryEntity c in list)
                    {
                        placeholderList.Add(new CountryPlaceholder()
                        {
                            iso = c.edp_CountryCode,
                            guid = c.Id.ToString()
                        });
                    }

                    MemoryStream ms = new MemoryStream();
                    serializer.WriteObject(ms, placeholderList);
                    ms.Position = 0;
                    StreamReader sr = new StreamReader(ms);
                    stringGuidList = sr.ReadToEnd();
                    sr.Close();
                    ms.Close();

                    httpContext.Cache.Insert(_countryList, stringGuidList, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
                }
                if (placeholderList == null)
                {
                    MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(stringGuidList));
                    object obj = serializer.ReadObject(ms);
                    placeholderList = (List<CountryPlaceholder>)obj;
                    ms.Close();
                }

                //CountryPlaceholder placeholder = placeholderList.Find(x => x.iso.Equals(CountryCode));
                CountryPlaceholder placeholder = placeholderList.FirstOrDefault(x => x.iso == CountryCode);

                if (placeholder == null || string.IsNullOrWhiteSpace(placeholder.guid))
                    return null;
                return new EntityReference
                {
                    Id = new Guid(placeholder.guid),
                    LogicalName = CountryEntity.EntityLogicalName
                };
            }
            else // No HttpContext Found
            {
                CountryEntity c = XrmRetrieveHelper.RetrieveFirst<CountryEntity>(localContext, new ColumnSet(false),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(CountryEntity.Fields.edp_CountryCode, ConditionOperator.Equal, CountryCode)
                        }
                    });
                if (c != null)
                    return c.ToEntityReference();
                return null;
            }
        }

        public static string GetIsoCodeForCountry(Plugin.LocalPluginContext localContext, Guid countryId)
        {
            HttpContext httpContext = HttpContext.Current;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<CountryPlaceholder>));

            string stringGuidList = httpContext.Cache.Get(_countryList) as string;
            List<CountryPlaceholder> placeholderList = null;
            if (stringGuidList == null)
            {
                List<CountryEntity> list = XrmRetrieveHelper.RetrieveMultiple<CountryEntity>(localContext, new ColumnSet(CountryEntity.Fields.edp_CountryCode), new FilterExpression()) as List<CountryEntity>;
                placeholderList = new List<CountryPlaceholder>();
                foreach (CountryEntity c in list)
                {
                    placeholderList.Add(new CountryPlaceholder()
                    {
                        iso = c.edp_CountryCode,
                        guid = c.Id.ToString()
                    });
                }

                MemoryStream ms = new MemoryStream();
                serializer.WriteObject(ms, placeholderList);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                stringGuidList = sr.ReadToEnd();
                sr.Close();
                ms.Close();

                httpContext.Cache.Insert(_countryList, stringGuidList, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            }
            if (placeholderList == null)
            {
                MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(stringGuidList));
                placeholderList = (List<CountryPlaceholder>)serializer.ReadObject(ms);
                ms.Close();
            }

            string isoCode = placeholderList.Find(x => x.guid.Equals(countryId.ToString())).iso;
            return isoCode;
        }

        [DataContract]
        internal class CountryPlaceholder
        {
            [DataMember(IsRequired = true)]
            public string iso { get; set; }

            [DataMember(IsRequired = true)]
            public string guid { get; set; }
        }
    }
}