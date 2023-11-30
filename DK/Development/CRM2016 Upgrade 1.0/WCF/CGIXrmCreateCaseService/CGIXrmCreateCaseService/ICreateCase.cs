using System.ServiceModel;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    // TODO.. very long names..
    // still long names..
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

        [OperationContract]
        UpdateAutoRgCaseResponse RequestCreateDecision(UpdateAutoRgCaseRequest request);

        [OperationContract]
        CloseCaseResponse RequestCloseCase(CloseCaseRequest request);
        #endregion
    }

}
