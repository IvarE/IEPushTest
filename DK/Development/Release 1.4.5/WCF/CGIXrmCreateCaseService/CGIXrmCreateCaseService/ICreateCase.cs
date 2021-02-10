using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    // TODO.. very long names..
    [ServiceContract]
    public interface ICreateCase
    {
        #region Public Methods
        [OperationContract]
        void RequestCreateCase(CreateCaseRequest request);

        [OperationContract]
        AutoRgCaseResponse RequestCreateAutoRGCase(AutoRgCaseRequest request);

        [OperationContract]
        UpdateAutoRgResponse RequestUpdateAutoRGCaseTravelInformations(UpdateAutoRgCaseTravelInformationsRequest request);

        [OperationContract]
        UpdateAutoRgResponse RequestUpdateAutoRGCaseBiffTransactions(UpdateAutoRgCaseBiffTransactionsRequest request);

        [OperationContract]
        UpdateAutoRgCaseResponse RequestUpdateAutoRGCaseRefundDecision(UpdateAutoRgCaseRequest request);
        #endregion
    }

}
