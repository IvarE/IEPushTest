using System.ServiceModel;
using CGIXrmExtConnectorService.CMSIndex.Models;

namespace CGIXrmExtConnectorService.CMSIndex
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtConnectorService" in both code and config file together.
    [ServiceContract]
    public interface ICMSIndexService
    {
        #region Public methods
        [OperationContract]
        CreateIndexResponse CreateIndex(CreateIndexRequest createIndexRequest);

        [OperationContract]
        RemoveIndexResponse RemoveIndex(RemoveIndexRequest removeIndexRequest);
        #endregion
    }
    
}
