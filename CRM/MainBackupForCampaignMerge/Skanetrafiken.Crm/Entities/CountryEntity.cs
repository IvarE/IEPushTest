using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class CountryEntity : Generated.edp_Country
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public static EntityReference GetEntityRefForCountryCode(Endeavor.Crm.Plugin.LocalPluginContext localContext, string countryCode)
        {
            FilterExpression filter = new FilterExpression();
            filter.AddCondition(CountryEntity.Fields.edp_CountryCode, ConditionOperator.Equal, countryCode);
            filter.AddCondition(CountryEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.edp_CountryState.Active);

            CountryEntity country = XrmRetrieveHelper.RetrieveFirst<CountryEntity>(localContext, new ColumnSet(false), filter);

            if (country != null)
            {
                return country.ToEntityReference();
            }
            return null;
        }

        public static EntityReference GetEntityRefForCountryCode(IList<CountryEntity> countries, string CountryCode)
        {
            foreach (CountryEntity country in countries)
            {
                if (country.edp_CountryCode == CountryCode)
                {
                    return country.ToEntityReference();
                }
            }
            return null;
        }

        public static string GetIsoCodeForCountry(Endeavor.Crm.Plugin.LocalPluginContext localContext, Guid countryId)
        {
            CountryEntity country = XrmRetrieveHelper.Retrieve<CountryEntity>(localContext, CountryEntity.EntityLogicalName, countryId, new ColumnSet(CountryEntity.Fields.edp_CountryCode));
            return country.edp_CountryCode;
        }


        // Below is only for use with Caching

        //public static EntityReference GetEntityRefForCountryCode(Endeavor.Crm.Plugin.LocalPluginContext localContext, string CountryCode)
        //{
        //    HttpContext httpContext = HttpContext.Current;
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<CountryPlaceholder>));

        //    string stringGuidList = httpContext.Cache.Get(_countryList) as string;
        //    List<CountryPlaceholder> placeholderList = null;
        //    if (stringGuidList == null)
        //    {
        //        IList<CountryEntity> list = XrmRetrieveHelper.RetrieveMultiple<CountryEntity>(localContext, new ColumnSet(CountryEntity.Fields.edp_CountryCode), new FilterExpression());
        //        placeholderList = new List<CountryPlaceholder>();
        //        foreach (CountryEntity c in list)
        //        {
        //            placeholderList.Add(new CountryPlaceholder()
        //            {
        //                iso = c.edp_CountryCode,
        //                guid = c.Id.ToString()
        //            });
        //        }

        //        MemoryStream ms = new MemoryStream();
        //        serializer.WriteObject(ms, placeholderList);
        //        ms.Position = 0;
        //        StreamReader sr = new StreamReader(ms);
        //        stringGuidList = sr.ReadToEnd();
        //        sr.Close();
        //        ms.Close();

        //        httpContext.Cache.Insert(_countryList, stringGuidList, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
        //    }
        //    if (placeholderList == null)
        //    {
        //        MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(stringGuidList));
        //        object obj = serializer.ReadObject(ms);
        //        placeholderList = (List<CountryPlaceholder>)obj;
        //        ms.Close();
        //    }

        //    CountryPlaceholder placeholder = placeholderList.Find(x => x.iso.Equals(CountryCode));
        //    if (placeholder == null || string.IsNullOrWhiteSpace(placeholder.guid))
        //        return null;
        //    return new EntityReference
        //    {
        //        Id = new Guid(placeholder.guid),
        //        LogicalName = CountryEntity.EntityLogicalName
        //    };
        //}

        //public static string GetIsoCodeForCountry(Plugin.LocalPluginContext localContext, Guid countryId)
        //{
        //    HttpContext httpContext = HttpContext.Current;
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<CountryPlaceholder>));

        //    string stringGuidList = httpContext.Cache.Get(_countryList) as string;
        //    List<CountryPlaceholder> placeholderList = null;
        //    if (stringGuidList == null)
        //    {
        //        List<CountryEntity> list = XrmRetrieveHelper.RetrieveMultiple<CountryEntity>(localContext, new ColumnSet(CountryEntity.Fields.edp_CountryCode), new FilterExpression()) as List<CountryEntity>;
        //        placeholderList = new List<CountryPlaceholder>();
        //        foreach(CountryEntity c in list)
        //        {
        //            placeholderList.Add(new CountryPlaceholder()
        //            {
        //                iso = c.edp_CountryCode,
        //                guid = c.Id.ToString()
        //            });
        //        }

        //        MemoryStream ms = new MemoryStream();
        //        serializer.WriteObject(ms, placeholderList);
        //        ms.Position = 0;
        //        StreamReader sr = new StreamReader(ms);
        //        stringGuidList = sr.ReadToEnd();
        //        sr.Close();
        //        ms.Close();

        //        httpContext.Cache.Insert(_countryList, stringGuidList, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
        //    }
        //    if (placeholderList == null)
        //    {
        //        MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(stringGuidList));
        //        placeholderList = (List<CountryPlaceholder>)serializer.ReadObject(ms);
        //        ms.Close();
        //    }

        //    string isoCode = placeholderList.Find(x => x.guid.Equals(countryId.ToString())).iso;
        //    return isoCode;
        //}

        //[DataContract]
        //internal class CountryPlaceholder
        //{
        //    [DataMember(IsRequired = true)]
        //    public string iso { get; set; }

        //    [DataMember(IsRequired = true)]
        //    public string guid { get; set; }        
        //}
    }

    
}