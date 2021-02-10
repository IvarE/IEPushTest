using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using CGIXrmExtConnectorService;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CGIXrmExtConnectorServiceTest
{
    [TestClass]
    public class CMSIndexServiceTest
    {
        CMSIndexManager _cmsIndexManager;
        private readonly XrmManager _manager;

        public CMSIndexServiceTest()
        {
            _cmsIndexManager = new CMSIndexManager();
            //Logga in mot CRM
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var domain = ConfigurationManager.AppSettings["Domain"];
            var serverAdress = ConfigurationManager.AppSettings["CrmServerUrl"];

            _manager = new XrmManager(serverAdress, domain, username, password);
        }

        [TestMethod]
        public void CreateIndexTest()
        {
            //Check with API before proceeding
            //CreateIndexRequest createIndexRequest = new CreateIndexRequest();
            //createIndexRequest.RequestActionType = CGIXrmExtConnectorService.Shared.Models.ActionType.Intranet;
            //createIndexRequest.KbArticleForIntranet = CreateArticleTest();
            //CreateIndexResponse createIndexResponse =_cmsIndexManager.CreateIndex(createIndexRequest);
            //Assert.IsNotNull(createIndexResponse.IntranetIndexId);
            //Assert.AreNotEqual(createIndexResponse.IntranetIndexStatus, false);
            //Assert.AreEqual(createIndexResponse.Status, CGIXrmExtConnectorService.Shared.Models.ProcessingStatus.Success);
            //RemoveIndexRequest removeIndexRequest = new RemoveIndexRequest();
            //removeIndexRequest.IntranetIndexId = createIndexResponse.IntranetIndexId;
            //RemoveIndexResponse removeIndexResponse = _cmsIndexManager.RemoveIndex(removeIndexRequest);
            //Assert.AreEqual(removeIndexResponse.Status, CGIXrmExtConnectorService.Shared.Models.ProcessingStatus.Success);
        }

        [TestMethod]
        public void RemoveIndexTest()
        {

        }

        private Article CreateArticleTest()
        {
            Article article = new Article();
            List<string> authors = new List<string>();
            authors.Add("AuthorUnitTest");
            article.Authors = authors;
            article.Category = "CategoryUnitTest";
            article.ExternalText = "This is a UnitTest article (External)";
            article.InternalText = "This is a UnitTest article (Internal)";
            article.KnowledgeBaseArticleId = new EntityReference("kbarticletemplate", GetKbArticleEntityTemplateId()).ToString();
            List<string> keywords = new List<string>();
            keywords.Add("Unit");
            keywords.Add("Test");
            article.Keywords = keywords;
            article.PublishDate = DateTime.Today.ToUniversalTime();
            article.Summary = "This is a UnitTest summary";
            article.Title = "This is an article created from a UnitTest";
            article.UpdateDate = DateTime.Today.ToUniversalTime();
            return article;
        }

        private Guid GetKbArticleEntityTemplateId()
        {
            var kbArticleTemplateId = Guid.Empty;

            var query = new QueryExpression("kbarticletemplate");
            query.ColumnSet.AddColumns("title", "isactive", "languagecode", "kbarticletemplateid");

            var templates = _manager.Service.RetrieveMultiple(query);

            kbArticleTemplateId = (from n in templates.Entities
                                   where n.LogicalName == "kbarticletemplate"
                                   select new Guid(n.Attributes["kbarticletemplateid"].ToString())).FirstOrDefault();

            return kbArticleTemplateId;
        }

    }
}
