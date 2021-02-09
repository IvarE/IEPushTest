using System;
using System.Net.Http;
using Microsoft.Azure.AppService;

namespace CGIXrmCreateCaseService.CRMPlusAPI
{
    public static class SkanetrafikenCrmAppServiceExtensions
    {
        public static SkanetrafikenCrm CreateSkanetrafikenCrm(this IAppServiceClient client)
        {
            return new SkanetrafikenCrm(client.CreateHandler());
        }

        public static SkanetrafikenCrm CreateSkanetrafikenCrm(this IAppServiceClient client, params DelegatingHandler[] handlers)
        {
            return new SkanetrafikenCrm(client.CreateHandler(handlers));
        }

        public static SkanetrafikenCrm CreateSkanetrafikenCrm(this IAppServiceClient client, Uri uri, params DelegatingHandler[] handlers)
        {
            return new SkanetrafikenCrm(uri, client.CreateHandler(handlers));
        }

        public static SkanetrafikenCrm CreateSkanetrafikenCrm(this IAppServiceClient client, HttpClientHandler rootHandler, params DelegatingHandler[] handlers)
        {
            return new SkanetrafikenCrm(rootHandler, client.CreateHandler(handlers));
        }
    }
}
