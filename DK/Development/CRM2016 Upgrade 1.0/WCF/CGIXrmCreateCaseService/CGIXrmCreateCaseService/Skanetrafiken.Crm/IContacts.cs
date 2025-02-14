﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CGIXrmCreateCaseService.CRMPlusAPI.Models;
using Microsoft.Rest;

namespace CGIXrmCreateCaseService.CRMPlusAPI
{
    public partial interface IContacts
    {
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> GetWithOperationResponseAsync(CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <param name='idOrEmail'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> GetLatestLinkGuidWithOperationResponseAsync(string idOrEmail, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <param name='idOrEmail'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> GetWithIdOrEmailWithOperationResponseAsync(string idOrEmail, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <param name='info'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> PostWithOperationResponseAsync(CustomerInfo info, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <param name='time'>
        /// Required.
        /// </param>
        /// <param name='price'>
        /// Required.
        /// </param>
        /// <param name='info'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> Post2ParamWithOperationResponseAsync(string time, int price, CustomerInfo info, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <param name='idOrEmail'>
        /// Required.
        /// </param>
        /// <param name='info'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> PutWithOperationResponseAsync(string idOrEmail, CustomerInfo info, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
}
