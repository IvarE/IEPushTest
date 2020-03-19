
// This file is maintained through Endeavor NuGet. Please do not modify it directly in your project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk;
using Endeavor.Crm.Extensions;



namespace Endeavor.Crm
{
    public static class XrmRetrieveHelper
    {
        /// <summary>
        /// Retrieves <b>all</b> records, based on query
        /// </summary>
        /// <typeparam name="T">Typed entity class to return.</typeparam>
        /// <param name="localContext">Plugin Context</param>
        /// <param name="query">Query to send</param>
        /// <param name="page">Paging Info.</param>
        /// <returns>Target records</returns>
        public static List<T> RetrieveMultiple<T>(Plugin.LocalPluginContext localContext, QueryExpression query, PagingInfo page = null) where T : Entity
        {
            List<Entity> entities = XrmHelper.RetrieveMultiple(
                localContext.OrganizationService,
                query,
                page
                );

            List<T> typedEntities = new List<T>();
            entities.ForEach(p => typedEntities.Add(p.ToEntity<T>()));

            return typedEntities;
        }

        /// <summary>
        /// Retrieves <b>all</b> records, based on filter
        /// </summary>
        /// <param name="localContext">Plugin Context</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter (optional)</param>
        /// <returns>Target records</returns>
        public static IList<T> RetrieveMultiple<T>(Plugin.LocalPluginContext localContext, ColumnSet columns, FilterExpression filter = null) where T : Entity, new()
        {
            List<Entity> entities = XrmHelper.RetrieveMultiple(
                localContext.OrganizationService,
                new T().LogicalName,
                columns,
                filter);

            List<T> typedEntities = new List<T>();
            entities.ForEach(p => typedEntities.Add(p.ToEntity<T>()));

            return typedEntities;
        }

        /// <summary>
        /// Retrieves <b>all</b> records, based on filter
        /// </summary>
        /// <param name="localContext">Plugin Context</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter (optional)</param>
        /// <returns>Target records</returns>
        public static IList<T> RetrieveMultiple<T>(Plugin.LocalPluginContext localContext, string entityLogicalName, ColumnSet columns, FilterExpression filter = null) where T : Entity
        {
            List<Entity> entities = XrmHelper.RetrieveMultiple(
                localContext.OrganizationService,
                entityLogicalName,
                columns,
                filter);

            List<T> typedEntities = new List<T>();
            entities.ForEach(p => typedEntities.Add(p.ToEntity<T>()));

            return typedEntities;
        }

        /// <summary>
        /// Retrieves a specified record
        /// </summary>
        /// <param name="localContext">Plugin Context</param>
        /// <param name="id">The record id</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <returns>Target record</returns>
        public static T Retrieve<T>(Plugin.LocalPluginContext localContext, string entityLogicalName, Guid id, ColumnSet columns) where T : Entity
        {
            return XrmHelper.Retrieve(
                localContext.OrganizationService,
                entityLogicalName,
                id,
                columns).ToEntityNull<T>();
        }

        /// <summary>
        /// Retrieves a specified record
        /// </summary>
        /// <param name="localContext">Plugin Context</param>
        /// <param name="id">The record id (EntityReference)</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <returns>Target record</returns>
        public static T Retrieve<T>(Plugin.LocalPluginContext localContext, EntityReference id, ColumnSet columns) where T : Entity
        {
            return XrmHelper.Retrieve(
                localContext.OrganizationService,
                id.LogicalName,
                id.Id,
                columns).ToEntityNull<T>();
        }

        /// <summary>
        /// Retrieves a specified record
        /// </summary>
        /// <param name="localContext">Plugin Context</param>
        /// <param name="id">The record id</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <returns>Target record</returns>
        public static T Retrieve<T>(Plugin.LocalPluginContext localContext, Guid id, ColumnSet columns) where T : Entity, new()
        {
            return XrmHelper.Retrieve(
                localContext.OrganizationService,
                new T().LogicalName,
                id,
                columns).ToEntityNull<T>();
        }

        /// <summary>
        /// Retrieves the first found record
        /// </summary>
        /// <param name="localContext">Plugin Context</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter the recordset (optional)</param>
        /// <param name="orderby">Order the recordset (optional)</param>
        /// <returns>Target record</returns>
        public static T RetrieveFirst<T>(Plugin.LocalPluginContext localContext, string entityLogicalName, ColumnSet columns, FilterExpression filter = null, Dictionary<string, OrderType> orderby = null) where T : Entity
        {
            return XrmHelper.RetrieveFirst(
                localContext.OrganizationService,
                entityLogicalName,
                columns,
                filter,
                orderby).ToEntityNull<T>();
        }

        /// <summary>
        /// Retrieves the first found record
        /// </summary>
        /// <param name="localContext">Plugin Context</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="filter">Filter the recordset (optional)</param>
        /// <param name="orderby">Order the recordset (optional)</param>
        /// <returns>Target record</returns>
        public static T RetrieveFirst<T>(Plugin.LocalPluginContext localContext, ColumnSet columns, FilterExpression filter = null, Dictionary<string, OrderType> orderby = null) where T : Entity, new()
        {
            return XrmHelper.RetrieveFirst(
                localContext.OrganizationService,
                new T().LogicalName,
                columns,
                filter,
                orderby).ToEntityNull<T>();
        }

        /// <summary>
        /// Retrieves the first found record
        /// </summary>
        /// <param name="localContext">Plugin Context</param>
        /// <param name="query">Query to send</param>
        /// <returns>Target record</returns>
        public static T RetrieveFirst<T>(Plugin.LocalPluginContext localContext, QueryExpression query) where T : Entity
        {
            PagingInfo page = new PagingInfo();
            page.Count = 1;
            page.PageNumber = 1;
            query.PageInfo = page;

            List<Entity> entities = XrmHelper.RetrieveMultiple(
                localContext.OrganizationService,
                query);

            if (entities.Count > 0)
                return entities[0].ToEntity<T>();
            
            return null;
        }

    }
}
