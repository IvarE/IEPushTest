
using System;
using System.ServiceModel;

namespace CGIXrmExtConnectorService
    {
        // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExtConnectorService" in code, svc and config file together.
        public partial class ExtConnectorService : ICallGuideService
        {

           
            
            public string GetChat(string interactionId, Guid callerId)
            {
                return "";
            }

            public string GetFBMessage(string interactionId, Guid callerId)
            {
                return "";
            }

            public CallGuideResponse ExecuteNewCallRequest(CallGuideRequest callguideRequest, CallGuideRequestType callguideRequestType, Guid callerId)
            {
                return new CallGuideResponse();
            }

            public Guid ExecuteNewCallRequest(CallGuideRequest callguideRequest,Guid callerId)
            {
                try
                {
                    CallGuideManager callguideManager = new CallGuideManager(callerId);
                    return callguideManager.ExecuteNewCallRequest(callguideRequest,callerId);

                }
                catch (Exception Ex)
                {
                    throw new FaultException<ExtConnectorServiceFault>(new ExtConnectorServiceFault
                    {
                        ApplicationName = "CGIXrmExtConnectorService",
                        Message = Ex.Message,
                        Detail = Ex.InnerException!=null?Ex.InnerException.Message:string.Empty,
                        Source = "GetCaseUrl"
                    });
                }
            }

            public Guid ExecuteCallTransferRequest(string callguidSessionId, Guid callerId)
            {
                try
                {
                    CallGuideManager callguideManager = new CallGuideManager(callerId);
                    return callguideManager.ExecuteCallTransferRequest(callguidSessionId);

                }
                catch (Exception Ex)
                {
                    throw new FaultException<ExtConnectorServiceFault>(new ExtConnectorServiceFault
                    {
                        ApplicationName = "CGIXrmExtConnectorService",
                        Message = Ex.Message,
                        Detail = Ex.InnerException != null ? Ex.InnerException.Message : string.Empty,
                        Source = "GetCaseUrlBySessionId"
                    });
                }
            }

            public Guid ExecuteChatRequest(CallGuideRequest callguideChatRequest, Guid callerId)
            {
                try
                {
                    CallGuideManager callguideManager = new CallGuideManager(callerId);
                    return callguideManager.ExecuteChatRequest(callguideChatRequest, callerId);

                }
                catch (Exception Ex)
                {
                    throw new FaultException<ExtConnectorServiceFault>(new ExtConnectorServiceFault
                    {
                        ApplicationName = "CGIXrmExtConnectorService",
                        Message = Ex.Message,
                        Detail = Ex.InnerException != null ? Ex.InnerException.Message : string.Empty,
                        Source = "GetCaseUrlBySessionId"
                    });
                }
            }

            public Guid ExecuteFBRequest(CallGuideRequest callguideFBRequest, Guid callerId)
            {
                try
                {
                    CallGuideManager callguideManager = new CallGuideManager(callerId);
                    return callguideManager.ExecuteFBRequest(callguideFBRequest, callerId);

                }
                catch (Exception Ex)
                {
                    throw new FaultException<ExtConnectorServiceFault>(new ExtConnectorServiceFault
                    {
                        ApplicationName = "CGIXrmExtConnectorService",
                        Message = Ex.Message,
                        Detail = Ex.InnerException != null ? Ex.InnerException.Message : string.Empty,
                        Source = "GetCaseUrlBySessionId"
                    });
                }
            }
        }
    }
