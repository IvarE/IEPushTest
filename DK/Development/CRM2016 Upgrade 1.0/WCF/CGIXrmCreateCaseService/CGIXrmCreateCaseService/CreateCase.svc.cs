using System;
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
                if (DateTime.Now < new DateTime(2019, 09, 06))
                {
                    _log.Debug(string.Format("debug. ============================================"));
                    _log.Debug(string.Format("debug. Start RequestCreateAutoRgCase"));
                }

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
                // Do debug-tracing. Johan Endeavor
                if (DateTime.Now < new DateTime(2017, 12, 14))
                {
                    _log.Debug(string.Format("============================================"));
                    _log.Debug(string.Format("Start RequestCreateAutoRgCase"));
                    //LogMessage(_logLocation, string.Format("============================================"));
                    //LogMessage(_logLocation, string.Format("Start RequestCreateAutoRgCase"));
                }

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
