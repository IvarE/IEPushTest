
using System;
namespace CGIXrmExtConnectorService
    {
        // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExtConnectorService" in code, svc and config file together.
        public partial class ExtConnectorService : ICMSIndexService
        {
            public CreateIndexResponse CreateIndex(CreateIndexRequest createIndexRequest)
            {
                CMSIndexManager cmsIndexManager = new CMSIndexManager();
                return cmsIndexManager.CreateIndex(createIndexRequest);
            }

            public RemoveIndexResponse RemoveIndex(RemoveIndexRequest removeIndexRequest)
            {
                CMSIndexManager cmsIndexManager = new CMSIndexManager();
                return cmsIndexManager.RemoveIndex(removeIndexRequest);
            }
        }
    }
