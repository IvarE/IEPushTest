using System;
using CGIXrmCreateCaseService.Case;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    public class CreateCase : ICreateCase
    {
        #region Public Methods ----------------------------------------------------------------------------------------

        public void RequestCreateCase(CreateCaseRequest request)
        {
            try
            {
                CreateCaseManager caseManager = new CreateCaseManager();
                caseManager.RequestCreateCase(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AutoRgCaseResponse RequestCreateAutoRGCase(AutoRgCaseRequest request)
        {
            try
            {
                CreateCaseManager caseManager = new CreateCaseManager();
                return caseManager.RequestCreateAutoRgCase(request);
            }
            catch (Exception ex)
            {
                return new AutoRgCaseResponse() { ErrorMessage = ex.Message, Success = false };

            }
        }

        public UpdateAutoRgResponse RequestUpdateAutoRGCaseTravelInformations(UpdateAutoRgCaseTravelInformationsRequest request)
        {
            try
            {
                CreateCaseManager caseManager = new CreateCaseManager();

                return caseManager.RequestCreateTravelInformationLinkedToCase(request);
            }
            catch (Exception ex)
            {
                return new UpdateAutoRgResponse() { ErrorMessage = ex.Message, Success = false };
            }
        }

        public UpdateAutoRgResponse RequestUpdateAutoRGCaseBiffTransactions(UpdateAutoRgCaseBiffTransactionsRequest request)
        {
            try
            {
                var caseManager = new CreateCaseManager();

                return caseManager.RequestCreateBiffTransactionsLinkedToCase(request);
            }
            catch (Exception ex)
            {
                return new UpdateAutoRgResponse() { ErrorMessage = ex.Message, Success = false };
            }
        }

        public UpdateAutoRgCaseResponse RequestUpdateAutoRGCaseRefundDecision(UpdateAutoRgCaseRequest request)
        {
            // NOTE: its up to callers to make sure the following methods are called before this one ( we do not check )
            // * RequestUpdateAutoRGCaseTravelInformation
            // * RequestUpdateAutoRGCaseBiffTransactions
            try
            {
                CreateCaseManager caseManager = new CreateCaseManager();

                return caseManager.RequestCreateAutoRgRefundDecision(request);
            }
            catch (Exception ex)
            {
                return new UpdateAutoRgCaseResponse() { ErrorMessage = ex.Message, Success = false };
            }
        }
        #endregion
    }
}
