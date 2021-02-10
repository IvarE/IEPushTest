using CGIXrmExtConnectorService.CMSIndex.Models;

namespace CGIXrmExtConnectorService.CMSIndex
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExtConnectorService" in code, svc and config file together.
    public partial class ExtConnectorService : ICMSIndexService
    {
        #region Public Methods
        public CreateIndexResponse CreateIndex(CreateIndexRequest createIndexRequest)
        {
            CmsIndexManager cmsIndexManager = new CmsIndexManager();
            return cmsIndexManager.CreateIndex(createIndexRequest);
        }

        public RemoveIndexResponse RemoveIndex(RemoveIndexRequest removeIndexRequest)
        {
            CmsIndexManager cmsIndexManager = new CmsIndexManager();
            return cmsIndexManager.RemoveIndex(removeIndexRequest);
        }
        #endregion
    }
}
