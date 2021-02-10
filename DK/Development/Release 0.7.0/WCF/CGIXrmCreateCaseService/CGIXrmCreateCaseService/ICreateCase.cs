using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CGIXrmCreateCaseService
{
    
    [ServiceContract]
    public interface ICreateCase
    {

        [OperationContract]
        void RequestCreateCase(CreateCaseRequest request);

        [OperationContract]
        CreateAutoRGCaseResponse RequestCreateAutoRGCase(CreateAutoRGCaseRequest request);

        [OperationContract]
        UpdateAutoRGCaseResponse RequestUpdateAutoRGCase(UpdateAutoRGCaseRequest request);

    }


    
}
