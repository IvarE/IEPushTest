using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CGI.CRM2013.Skanetrafiken.KBArticlePluginUnitTests.Models;
using CGIXrmWin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CGI.CRM2013.Skanetrafiken.KBArticlePlugin.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class KbArticlePluginUnitTest
    {
        #region Declarations ----------------------------------------------------------------------------------------

        private readonly XrmManager _manager;
        private readonly KBArticlePlugin _kbArticlePlugin;

        #endregion

        #region Public Methods --------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public KbArticlePluginUnitTest()
        {
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var domain = ConfigurationManager.AppSettings["Domain"];
            var serverAdress = ConfigurationManager.AppSettings["CrmAdress"];

            _manager = new XrmManager(serverAdress, domain, username, password);

            _kbArticlePlugin = new KBArticlePlugin();
        }


        /// <summary>
        /// Test method retrieving multiple knowledge base articles from CRM.
        /// </summary>
        [TestMethod]
        public void GetArticlesUnitTest()
        {
            var query = new QueryExpression("kbarticle");
            query.ColumnSet.AddColumns("number", "subjectid", "title", "modifiedby", "kbarticleid");

            var results = _manager.Service.RetrieveMultiple(query);

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Entities);
            var count = results.Entities.Count;
            Assert.AreNotEqual(count, 0);
        }


        /// <summary>
        /// Method creating a knowledge base article through Xrm-engine.
        /// </summary>
        [TestMethod]
        public void CreateArticleUnitTest()
        {
            var kbArticleEntity = new Entity("kbarticle")
            {
                Attributes = new AttributeCollection()
            };

            var user = GetUser();

            kbArticleEntity.Attributes.Add("title", "Unit Test Article");
            kbArticleEntity.Attributes.Add("number", "1");
            kbArticleEntity.Attributes.Add("content", "Unit Test Article");
            kbArticleEntity.Attributes.Add("cgi_externaltext", "Unit Test Article");
            kbArticleEntity.Attributes.Add("modifiedby", new EntityReference("systemuser", user.Id));
            kbArticleEntity.Attributes.Add("createdon", "");
            kbArticleEntity.Attributes.Add("keywords", "");
            kbArticleEntity.Attributes.Add("modifiedon", "");
            kbArticleEntity.Attributes.Add("kbarticletemplateid", 
                new EntityReference("kbarticletemplate", GetKbArticleEntityTemplateId()));
            kbArticleEntity.Attributes.Add("subjectid", new EntityReference("subject", GetSubjectId()));

            var kbArticleId = _manager.Service.Create(kbArticleEntity);
            DeleteKbArticle(kbArticleId);

         
            Assert.IsNotNull(kbArticleId);
        }


        /// <summary>
        /// Test method retrieving a knowledge base article for intranet.
        /// </summary>
        [TestMethod]
        public void GetKbArticleIntranetUnitTest()
        {
            var query = new QueryExpression("kbarticle");
            query.ColumnSet.AddColumns(
                "title", 
                "number", 
                "content", 
                "createdon", 
                "keywords", 
                "modifiedon", 
                "subjectid", 
                "modifiedby");

            var articles = _manager.Service.RetrieveMultiple(query);

            var kbArticle = (from n in articles.Entities 
                             where n.LogicalName == "kbarticle" 
                             select n).FirstOrDefault();

            var response = _kbArticlePlugin.GetKBArticleIntranet(kbArticle);

            Assert.IsNotNull(response);
        }


        /// <summary>
        /// Test method retrieving a knowledge base article for external webpage.
        /// </summary>
        [TestMethod]
        public void GetKbArticleExternalWebUnitTest()
        {
            var query = new QueryExpression("kbarticle");
            query.ColumnSet.AddColumns(
                "title", 
                "number", 
                "content", 
                "createdon", 
                "keywords", 
                "modifiedon", 
                "subjectid", 
                "modifiedby");

            var articles = _manager.Service.RetrieveMultiple(query);

            var kbArticle = (from n in articles.Entities where n.LogicalName == "kbarticle" select n).FirstOrDefault();
            
            var response = _kbArticlePlugin.GetKBArticleExternalWeb(kbArticle);

            Assert.IsNotNull(response);
        }

        #endregion

        #region Private Methods -------------------------------------------------------------------------------------

        /// <summary>
        /// Method deleting a KB Article entity record based on entity ID.
        /// </summary>
        /// <param name="kbArticleId">KB Article ID</param>
        private void DeleteKbArticle(Guid kbArticleId)
        {
            var kbArticleEntity = new Entity("kbarticle");

            _manager.Service.Delete(kbArticleEntity.LogicalName, kbArticleId);
        }


        /// <summary>
        /// Method acquiring a knowledge base article template Id
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Method acquiring a CRM system user based on given parameters.
        /// </summary>
        /// <returns></returns>
        private UserEntity GetUser()
        {
            var currentUser = _manager.GetCurrentUser();

            var query = new QueryByAttribute("systemuser")
            {
                ColumnSet = new ColumnSet("fullname", "systemuserid", "domainname", "isdisabled")
            };

            query.Attributes.AddRange("systemuserid");
            query.Values.AddRange(currentUser.SystemUserId);
            
            var users = _manager.Service.RetrieveMultiple(query);

            var userEntity = new UserEntity();

            foreach (var user in users.Entities)
            {
                if (user != null)
                {
                    userEntity.FullName = (user.Attributes["fullname"] ?? string.Empty).ToString();
                    userEntity.Username = (user.Attributes["domainname"] ?? string.Empty).ToString();
                    userEntity.Id = new Guid(user.Attributes["systemuserid"].ToString());
                    userEntity.Status = (bool)user.Attributes["isdisabled"];
                }
                break;
            }

            return userEntity;
        }


        /// <summary>
        /// Method acquiring a subject id
        /// </summary>
        /// <returns></returns>
        private Guid GetSubjectId()
        {
            var query = new QueryExpression("subject")
            {
                ColumnSet = new ColumnSet("subjectid")
            };

            var subjects = _manager.Service.RetrieveMultiple(query);

            var subjectId = (from n in subjects.Entities 
                where n.LogicalName == "subject" 
                select new Guid(n.Attributes["subjectid"].ToString())).FirstOrDefault();
            
            return subjectId;
        }

        #endregion
    }
}
