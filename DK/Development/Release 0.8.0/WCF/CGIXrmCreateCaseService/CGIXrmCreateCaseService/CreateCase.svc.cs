using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CGIXrmCreateCaseService
{
    
    public class CreateCase : ICreateCase
    {

        public void RequestCreateCase(CreateCaseRequest request)
        {
            try
            {
                CreateCaseManager _caseManager = new CreateCaseManager();
                _caseManager.RequestCreateCase(request);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public CreateAutoRGCaseResponse RequestCreateAutoRGCase(CreateAutoRGCaseRequest request)
        {
            try
            {
                CreateCaseManager _caseManager = new CreateCaseManager();
                return _caseManager.RequestCreateAutoRGCase(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UpdateAutoRGCaseResponse RequestUpdateAutoRGCase(UpdateAutoRGCaseRequest request)
        {
            try
            {
                CreateCaseManager _caseManager = new CreateCaseManager();

                // Step 1: RequestCreateTravelInformationLinkedToCase
                UpdateAutoRGCaseResponse response =_caseManager.RequestCreateTravelInformationLinkedToCase(request);
                if (response.Success != true) { return response; }

                // Step 2: RequestCreateAutoRGRefundDecision
                return _caseManager.RequestCreateAutoRGRefundDecision(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
