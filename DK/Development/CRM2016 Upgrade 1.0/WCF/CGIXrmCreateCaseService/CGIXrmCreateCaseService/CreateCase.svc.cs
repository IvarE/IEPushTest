using System;
using System.Threading;
using CGIXrmCreateCaseService.Case;
using CGIXrmCreateCaseService.Case.Models;
using log4net.Config;

namespace CGIXrmCreateCaseService
{
    public class CreateCase : ICreateCase
    {
        log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Public Methods ----------------------------------------------------------------------------------------

        public void RequestCreateCase(CreateCaseRequest request)
        {
            try
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                _log.Info($"Th={threadId} - ===================== RequestCreateCase called. =====================\n");

                CreateCaseManager caseManager = new CreateCaseManager();
                caseManager.RequestCreateCase(threadId, request);
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
                int threadId = Thread.CurrentThread.ManagedThreadId;
                _log.Info($"Th={threadId} - ===================== RequestCreateAutoRGCase called. =====================\n");

                CreateCaseManager caseManager = new CreateCaseManager();
                return caseManager.RequestCreateAutoRgCase(threadId, request);
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

                _log.Debug(string.Format("============================================"));
                _log.Debug(string.Format("Start RequestUpdateAutoRGCaseTravelInformations"));


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
