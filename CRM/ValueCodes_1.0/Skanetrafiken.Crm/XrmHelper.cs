using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk.Messages;

namespace Endeavor.Crm
{
    /// <summary>
    /// The XrmHelper class simplifies the retrieval, update and create of records in Crm.
    /// </summary>
    public static class XrmHelper
    {
        /// <summary>
        /// Creates a specified record.
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="de">The new record</param>
        /// <returns></returns>
        public static Guid Create(IOrganizationService service, Entity de)
        {
            CreateRequest request = new CreateRequest();
            request.Target = de;

            CreateResponse response = (CreateResponse)service.Execute(request);
            return response.id;
        }

        /// <summary>
        /// Creates a specified record.
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="de">The new record</param>
        /// <returns></returns>
        public static Guid Create(Plugin.LocalPluginContext context, Entity de)
        {
            return Create(context.OrganizationService, de);
        }

        /// <summary>
        /// Updates a specified record
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="de">Target record, with the updated values</param>
        public static UpdateResponse Update(IOrganizationService service, Entity de)
        {
            UpdateRequest request = new UpdateRequest();
            request.Target = de;

            return (UpdateResponse)service.Execute(request);
        }

        /// <summary>
        /// Updates a specified record
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="de">Target record, with the updated values</param>
        public static UpdateResponse Update(Plugin.LocalPluginContext context, Entity de)
        {
            return Update(context.OrganizationService, de);
        }

        /// <summary>
        /// Deletes a specified record.
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="target">Target record</param>
        public static DeleteResponse Delete(IOrganizationService service, EntityReference target)
        {
            DeleteRequest request = new DeleteRequest();
            request.Target = target;

            return (DeleteResponse)service.Execute(request);
        }

        /// <summary>
        /// Deletes a specified record.
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="target">Target record</param>
        public static DeleteResponse Delete(Plugin.LocalPluginContext context, EntityReference target)
        {
            return Delete(context.OrganizationService, target);
        }

        ///// <summary>
        ///// Upserts a specified record.
        ///// </summary>
        ///// <param name="service">The organization service</param>
        ///// <param name="target">Target record</param>
        //public static UpsertResponse Upsert(IOrganizationService service, Entity de)
        //{
        //    UpsertRequest request = new UpsertRequest();
        //    request.Target = de;

        //    return (UpsertResponse)service.Execute(request);
        //}

        ///// <summary>
        ///// Upserts a specified record.
        ///// </summary>
        ///// <param name="context">Plugin context</param>
        ///// <param name="target">Target record</param>
        //public static UpsertResponse Upsert(Plugin.LocalPluginContext context, Entity de)
        //{
        //    return Upsert(context.OrganizationService, de);
        //}

        /// <summary>
        /// Retrieves a specified record
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="entity">The logical name</param>
        /// <param name="id">The record id</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <returns>Target record</returns>
        public static Entity Retrieve(IOrganizationService service, string entity, Guid id, ColumnSet columns)
        {
            EntityReference target = new EntityReference();
            target.Id = id;
            target.LogicalName = entity;

            RetrieveRequest request = new RetrieveRequest();
            request.ColumnSet = columns;
            request.Target = target;

            RetrieveResponse response = (RetrieveResponse)service.Execute(request);
            return response.Entity;
        }

        /// <summary>
        /// Retrieves a specified record
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="entity">The logical name</param>
        /// <param name="id">The record id</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <returns>Target record</returns>
        public static Entity Retrieve(Plugin.LocalPluginContext context, string entity, Guid id, ColumnSet columns)
        {
            return Retrieve(context.OrganizationService, entity, id, columns);
        }

        /// <summary>
        /// Retrieves the first found record
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="entity">The logical name</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter the recordset (optional)</param>
        /// <param name="orderby">Order the recordset (optional)</param>
        /// <returns>Target record</returns>
        public static Entity RetrieveFirst(IOrganizationService service, string entity, string[] columns, FilterExpression filter = null, Dictionary<string, OrderType> orderby = null)
        {
            return RetrieveFirst(service, entity, new ColumnSet(columns), filter, orderby);
        }

        /// <summary>
        /// Retrieves the first found record
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="entity">The logical name</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter the recordset (optional)</param>
        /// <param name="orderby">Order the recordset (optional)</param>
        /// <returns>Target record</returns>
        public static Entity RetrieveFirst(Plugin.LocalPluginContext context, string entity, string[] columns, FilterExpression filter = null, Dictionary<string, OrderType> orderby = null)
        {
            return RetrieveFirst(context, entity, new ColumnSet(columns), filter, orderby);
        }

        /// <summary>
        /// Retrieves the first found record
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="entity">The logical name</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter the recordset (optional)</param>
        /// <param name="orderby">Order the recordset (optional)</param>
        /// <returns>Target record</returns>
        public static Entity RetrieveFirst(IOrganizationService service, string entity, ColumnSet columns, FilterExpression filter = null, Dictionary<string, OrderType> orderby = null)
        {
            QueryExpression query = new QueryExpression(entity);
            query.ColumnSet = columns;
            if (filter != null)
                query.Criteria = filter;
            if (orderby != null)
            {
                foreach (string attribute in orderby.Keys)
                {
                    query.AddOrder(attribute, orderby[attribute]);
                }
            }

            query.TopCount = 1;

            EntityCollection result = service.RetrieveMultiple(query);

            if (result.Entities.Count > 0)
                return result.Entities[0];
            return null;
        }

        /// <summary>
        /// Retrieves the first found record
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="entity">The logical name</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter the recordset (optional)</param>
        /// <param name="orderby">Order the recordset (optional)</param>
        /// <returns>Target record</returns>
        public static Entity RetrieveFirst(Plugin.LocalPluginContext context, string entity, ColumnSet columns, FilterExpression filter = null, Dictionary<string, OrderType> orderby = null)
        {
            return RetrieveFirst(context.OrganizationService, entity, columns, filter, orderby);
        }

        /// <summary>
        /// Retrieves the first found record, by attribute value(s)
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="entity">The logical name</param>
        /// <param name="attributevalues">Filter the recordset on attribute values</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="orderby">Order the recordset (optional)</param>
        /// <returns>Target record</returns>
        public static Entity RetrieveFirstByAttribute(IOrganizationService service, string entity, Dictionary<string, object> attributevalues, string[] columns, Dictionary<string, OrderType> orderby = null)
        {
            QueryByAttribute query = new QueryByAttribute(entity);
            query.ColumnSet = new ColumnSet(columns);
            foreach (string key in attributevalues.Keys)
            {
                query.AddAttributeValue(key, attributevalues[key]);
            }
            if (orderby != null)
            {
                foreach (string attribute in orderby.Keys)
                {
                    query.AddOrder(attribute, orderby[attribute]);
                }
            }

            query.TopCount = 1;

            EntityCollection result = service.RetrieveMultiple(query);

            if (result.Entities.Count > 0)
                return result.Entities[0];
            return null;
        }

        /// <summary>
        /// Retrieves the first found record, by attribute value(s)
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="entity">The logical name</param>
        /// <param name="attributevalues">Filter the recordset on attribute values</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="orderby">Order the recordset (optional)</param>
        /// <returns>Target record</returns>
        public static Entity RetrieveFirstByAttribute(Plugin.LocalPluginContext context, string entity, Dictionary<string, object> attributevalues, string[] columns, Dictionary<string, OrderType> orderby = null)
        {
            return RetrieveFirstByAttribute(context.OrganizationService, entity, attributevalues, columns, orderby);
        }

        /// <summary>
        /// Retrieves <b>all</b> records, based on filter
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="entity">The logical name</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter (optional)</param>
        /// <returns>Target records</returns>
        public static List<Entity> RetrieveMultiple(IOrganizationService service, string entity, string[] columns, FilterExpression filter = null)
        {
            return RetrieveMultiple(service, entity, new ColumnSet(columns), filter);
        }

        /// <summary>
        /// Retrieves <b>all</b> records, based on filter
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="entity">The logical name</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter (optional)</param>
        /// <returns>Target records</returns>
        public static List<Entity> RetrieveMultiple(Plugin.LocalPluginContext context, string entity, string[] columns, FilterExpression filter = null)
        {
            return RetrieveMultiple(context, entity, new ColumnSet(columns), filter);
        }

        /// <summary>
        /// Retrieves <b>all</b> records, based on filter
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="entity">The logical name</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter (optional)</param>
        /// <returns>Target records</returns>
        public static List<Entity> RetrieveMultiple(IOrganizationService service, string entity, ColumnSet columns, FilterExpression filter = null)
        {
            PagingInfo page = new PagingInfo();
            page.Count = 500;
            page.PageNumber = 1;

            QueryExpression query = new QueryExpression(entity);
            query.ColumnSet = columns;
            if (filter != null)
                query.Criteria = filter;
            query.PageInfo = page;

            return RetrieveMultiple(service, query, page);
        }

        /// <summary>
        /// Retrieves <b>all</b> records, based on filter
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="entity">The logical name</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter (optional)</param>
        /// <returns>Target records</returns>
        public static List<Entity> RetrieveMultiple(Plugin.LocalPluginContext context, string entity, ColumnSet columns, FilterExpression filter = null)
        {
            return RetrieveMultiple(context.OrganizationService, entity, columns, filter);
        }

        /// <summary>
        /// Retrieves the record count of an entity.
        /// </summary>
        /// <param name="service">The orgnization service</param>
        /// <param name="entity">The entity</param>
        /// <param name="filter">Optional filter</param>
        /// <returns>The number of records</returns>
        public static Int32 RetrieveRecordCount(IOrganizationService service, String entity, FilterExpression filter = null)
        {
            QueryExpression query = new QueryExpression(entity);
            query.PageInfo = new PagingInfo()
            {
                Count = 1000,
                PageNumber = 1
            };

            if (filter != null)
            {
                query.Criteria = filter;
            }

            return RetrieveMultiple(service, query).Count;
        }

        /// <summary>
        /// Retrieves the record count of an entity.
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="entity">The entity</param>
        /// <param name="filter">Optional filter</param>
        /// <returns>The number of records</returns>
        public static Int32 RetrieveRecordCount(Plugin.LocalPluginContext context, String entity, FilterExpression filter = null)
        {
            return RetrieveRecordCount(context.OrganizationService, entity, filter);
        }

        /// <summary>
        /// Retrieves <b>all</b> records, based on query
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="query">Query</param>
        /// <param name="page">Paging info</param>
        /// <returns>Target records</returns>
        public static List<Entity> RetrieveMultiple(IOrganizationService service, QueryExpression query, PagingInfo page = null)
        {
            List<Entity> records = new List<Entity>();

            RetrieveMultipleResponse response;
            do
            {
                RetrieveMultipleRequest request = new RetrieveMultipleRequest();
                request.Query = query;

                response = (RetrieveMultipleResponse)service.Execute(request);

                if (response.EntityCollection.Entities.Count > 0)
                    records.AddRange(response.EntityCollection.Entities);

                if (response.EntityCollection.MoreRecords)
                {
                    query.PageInfo.PageNumber++;
                    query.PageInfo.PagingCookie = response.EntityCollection.PagingCookie;
                }
            } while (response.EntityCollection.MoreRecords);

            return records;
        }

        /// <summary>
        /// Retrieves <b>all</b> records, based on query
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="query">Query</param>
        /// <param name="page">Paging info</param>
        /// <returns>Target records</returns>
        public static List<Entity> RetrieveMultiple(Plugin.LocalPluginContext context, QueryExpression query, PagingInfo page = null)
        {
            return RetrieveMultiple(context.OrganizationService, query, page);
        }

        /// <summary>
        /// Retrieves the logical attribute names of an entity
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="entity">The logical name</param>
        /// <param name="onlyValidForAdvancedFind">Only return the attributes that is valid for advanced find (optional, true by default)</param>
        /// <returns>A list of attributes</returns>
        public static List<string> RetrieveEntityAttributeNames(IOrganizationService service, string entity, bool onlyValidForAdvancedFind = true)
        {
            RetrieveEntityRequest request = new RetrieveEntityRequest();
            request.LogicalName = entity;
            request.EntityFilters = EntityFilters.Attributes;

            RetrieveEntityResponse response = (RetrieveEntityResponse)service.Execute(request);

            List<string> attributes = new List<string>();
            foreach (AttributeMetadata attribute in response.EntityMetadata.Attributes)
            {
                if (attribute.IsValidForAdvancedFind.Value)
                {
                    attributes.Add(attribute.LogicalName);
                }
                else if (!onlyValidForAdvancedFind)
                {
                    attributes.Add(attribute.LogicalName);
                }
            }
            return attributes;
        }

        /// <summary>
        /// Retrieves the logical attribute names of an entity
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="entity">The logical name</param>
        /// <param name="onlyValidForAdvancedFind">Only return the attributes that is valid for advanced find (optional, true by default)</param>
        /// <returns>A list of attributes</returns>
        public static List<string> RetrieveEntityAttributeNames(Plugin.LocalPluginContext context, string entity, bool onlyValidForAdvancedFind = true)
        {
            return RetrieveEntityAttributeNames(context.OrganizationService, entity, onlyValidForAdvancedFind);
        }

        /// <summary>
        /// Creates an array of activityparties (to use with party list fields such as e-mail "to").
        /// </summary>
        /// <param name="references">List of participating references</param>
        /// <returns>Entity[] as party lists</returns>
        public static Entity[] CreatePartyListFromReferences(List<EntityReference> references)
        {
            List<Entity> partylist = new List<Entity>();
            foreach (EntityReference reference in references)
            {
                Entity party = new Entity("activityparty");
                party.Attributes.Add("partyid", reference);
                partylist.Add(party);
            }
            return partylist.ToArray();
        }

        /// <summary>
        /// Creates and optionally sends an e-mail.
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="email">The e-mail to create</param>
        /// <param name="TrackingToken">The tracking token, if any</param>
        /// <param name="IssueSend">True for sending the created e-mail, false for just create</param>
        /// <returns>The id of the created E-mail</returns>
        public static Guid CreateAndSendEmail(IOrganizationService service, Entity email, String TrackingToken = "", bool IssueSend = false)
        {
            // Create the E-mail
            Guid id = service.Create(email);

            // Issue send
            if (IssueSend)
            {
                SendEmailRequest request = new SendEmailRequest();
                request.EmailId = id;
                request.IssueSend = true;
                request.TrackingToken = TrackingToken;

                SendEmailResponse response = (SendEmailResponse)service.Execute(request);
            }
            return id;
        }

        /// <summary>
        /// Creates and optionally sends an e-mail.
        /// </summary>
        /// <param name="context">Plugin context</param>
        /// <param name="email">The e-mail to create</param>
        /// <param name="TrackingToken">The tracking token, if any</param>
        /// <param name="IssueSend">True for sending the created e-mail, false for just create</param>
        /// <returns>The id of the created E-mail</returns>
        public static Guid CreateAndSendEmail(Plugin.LocalPluginContext context, Entity email, String TrackingToken = "", bool IssueSend = false)
        {
            return CreateAndSendEmail(context.OrganizationService, email, TrackingToken, IssueSend);
        }

        /// <summary>
        /// Parses and returns parsed value as string
        /// </summary>
        /// <param name="CrmAttribute">The attribute value to parse</param>
        /// <param name="FormatString">If format should be applied (eg on Number, Date, or Money)</param>
        /// <param name="IfOptionSetOrEntityRefReturnKeyNotName">If the guid or option set value should be returned instead of the record name if object is entity reference</param>
        /// <returns>The value (formatted if applicable) or empty string</returns>
        public static String ParseCrmTypesToString(Entity Entity, String CrmAttributeName, String FormatString = "", bool IfOptionSetOrEntityRefReturnKeyNotName = false)
        {
            String Value = String.Empty;
            if (!Entity.Contains(CrmAttributeName))
                return Value;

            Object CrmAttribute = Entity[CrmAttributeName];

            if (CrmAttribute != null)
            {
                // Return the formatted value if one exists
                if (String.IsNullOrWhiteSpace(FormatString) && Entity.FormattedValues.Contains(CrmAttributeName) &&
                   !(CrmAttribute is OptionSetValue) && !(CrmAttribute is EntityReference))
                    return Entity.FormattedValues[CrmAttributeName];

                if (CrmAttribute is String)
                {
                    return (String)CrmAttribute;
                }
                else if (CrmAttribute is int)
                {
                    if (String.IsNullOrWhiteSpace(FormatString))
                        return ((int)CrmAttribute).ToString();
                    else
                        return ((int)CrmAttribute).ToString(FormatString);
                }
                else if (CrmAttribute is decimal)
                {
                    if (String.IsNullOrWhiteSpace(FormatString))
                        return ((decimal)CrmAttribute).ToString();
                    else
                        return ((decimal)CrmAttribute).ToString(FormatString);
                }
                else if (CrmAttribute is double)
                {
                    if (String.IsNullOrWhiteSpace(FormatString))
                        return ((double)CrmAttribute).ToString();
                    else
                        return ((double)CrmAttribute).ToString(FormatString);
                }
                else if (CrmAttribute is Int16)
                { // SmallInt
                    if (String.IsNullOrWhiteSpace(FormatString))
                        return ((Int16)CrmAttribute).ToString();
                    else
                        return ((Int16)CrmAttribute).ToString(FormatString);
                }
                else if (CrmAttribute is Int64)
                { // BigInt
                    if (String.IsNullOrWhiteSpace(FormatString))
                        return ((Int64)CrmAttribute).ToString();
                    else
                        return ((Int64)CrmAttribute).ToString(FormatString);
                }
                else if (CrmAttribute is Money)
                {
                    if (String.IsNullOrWhiteSpace(FormatString))
                        return ((Money)CrmAttribute).Value.ToString();
                    else
                        return ((Money)CrmAttribute).Value.ToString(FormatString);
                }
                else if (CrmAttribute is DateTime)
                {
                    if (String.IsNullOrWhiteSpace(FormatString))
                        return ((DateTime)CrmAttribute).ToString();
                    else
                        return ((DateTime)CrmAttribute).ToString(FormatString);
                }
                else if (CrmAttribute is Guid)
                {
                    if (String.IsNullOrWhiteSpace(FormatString))
                        return ((Guid)CrmAttribute).ToString();
                    else
                        return ((Guid)CrmAttribute).ToString(FormatString);
                }
                else if (CrmAttribute is OptionSetValue)
                {
                    if (IfOptionSetOrEntityRefReturnKeyNotName)
                    {
                        return ((OptionSetValue)Entity[CrmAttributeName]).Value.ToString();
                    }
                    else
                    {
                        if (Entity.FormattedValues.Contains(CrmAttributeName))
                            return Entity.FormattedValues[CrmAttributeName];
                        else
                            return ((OptionSetValue)Entity[CrmAttributeName]).Value.ToString();
                    }
                }
                else if (CrmAttribute is EntityReference)
                {
                    if (IfOptionSetOrEntityRefReturnKeyNotName)
                    {
                        return ((EntityReference)Entity[CrmAttributeName]).Id.ToString();
                    }
                    else
                    {
                        if (Entity.FormattedValues.Contains(CrmAttributeName))
                            return Entity.FormattedValues[CrmAttributeName];
                        else
                            return ((EntityReference)Entity[CrmAttributeName]).Name.ToString();
                    }
                }
                else if (CrmAttribute is Entity[])
                {
                    throw new NotImplementedException("Party lists are not implemented (yet) in ParseCrmTypesToString");
                }
            }
            return Value;
        }
    }
}
