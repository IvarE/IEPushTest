using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find;
using System.Configuration;
using CGIXrmExtConnectorService;
using Microsoft.Xrm.Sdk;
using CGIXrmWin;
using Microsoft.Xrm.Sdk.Query;

namespace CGIXrmKBArticleRepublish
{
    public class KBArticleRepublisher
    {
        string externalWebConfigSection = "KbExternalWeb";
        string intranetConfigSection = "KbIntranet";
        CMSIndexManager cmsIndexManager = new CMSIndexManager();


        public void Run()
        {
            Console.WriteLine("ENTERING Run()");


            //OBS!!
            //ALL KOD SOM ANROPAR CRM ÄR OTESTAD
            //ALL KOD SOM TAR BORT ELLER LÄGGER TILL ARTIKLAR I INDEXET ÄR OTESTAD
            //Det enda som är verifierat är att man får ut 450 poster ur indexet med nedanstående anrop


            //hämta alla artiklar i indexet av typen CGIXrmExtConnectorService.KBArticle
            SearchResults<CGIXrmExtConnectorService.KBArticle> result = GetAllArticlesFromEpi();
           
            //gå igenom alla poster i Index
            foreach (CGIXrmExtConnectorService.KBArticle article in result)
            {
                //Analysera vilka poster vi får ut
                Console.Write(string.Format("article.Title={0} | ", article.Title));
                Console.Write(string.Format("KnowledgeBaseArticleId={0} | ", article.KnowledgeBaseArticleId));
                Console.WriteLine();
                
                ////Använd nästa rad för att ta bort posten
                //DeleteKBArticleIntranet(article.KnowledgeBaseArticleId);
            }

            
            
            //Hämta alla artiklar som ska publiceras på intranätet (villkor analyserade från KBArticlePlugin)
            QueryExpression query = new QueryExpression() { ColumnSet = new ColumnSet(true), EntityName = "kbarticle" }; //returnera alla kolumner

            //Status ska vara 3-published
            query.Criteria.AddCondition("statuscode", ConditionOperator.Equal, 3);
            
            //Kontrollera att den är godkänd för publicering
            query.Criteria.AddCondition("cgi_appoval", ConditionOperator.In, 
                new int[] { 285050001/*Pending*/
                , 285050002/*Internal publishing*/
                , 285050003/*Approved*/});


            CGIXrmWin.XrmManager manager = _initManager();
            Entity[] kbarticles = manager.Get((QueryBase)query);
            foreach (Entity e in kbarticles)
            {
                bool isIntranetArticle = true;
                Article a = GetKBArticle(e, isIntranetArticle); //konvertera till den typ som används i CGIXrmExtConnectorService
                
                //analysera artikeln
                Console.Write(string.Format("Title={0} | ", a.Title));
                Console.Write(string.Format("KnowledgeBaseArticleId={0} | ", a.KnowledgeBaseArticleId));
                Console.WriteLine();



                ////Skapa artikeln på intranätet
                //string cgi_intranetindex = CreateKBArticleIntranet(a );
                ////Uppdatera artikeln i CRM med det returnerade idt.
                //Entity EntityToUpdate = new Entity("kbarticle");
                //EntityToUpdate.Id = e.Id;
                //EntityToUpdate["cgi_intranetindex"] = cgi_intranetindex;
                //manager.Update(EntityToUpdate);

            }
            Console.WriteLine("EXITING Run()");
        }

        private SearchResults<CGIXrmExtConnectorService.KBArticle> GetAllArticlesFromEpi()
        {
            IClient kbIntranetClient = CreateClientFromConfig(intranetConfigSection);
            ITypeSearch<CGIXrmExtConnectorService.KBArticle> search = kbIntranetClient.Search<CGIXrmExtConnectorService.KBArticle>();//det går att lägga till fler kriterier i denna search om urvalet inte är rätt
            int count = search.Count();

            return search.Take(count).GetResult();
        }
        private void DeleteKBArticleIntranet(string _IntranetIndexId)
        {
            //Koden är helt oprövad men använder funktionen från CGIXrmExtConnectorService
            RemoveIndexRequest request = new RemoveIndexRequest() { ExternalWebIndexId = "", IntranetIndexId = _IntranetIndexId };
            RemoveIndexResponse response = cmsIndexManager.RemoveIndex(request);
        }
        private string CreateKBArticleIntranet(Article _KbArticleForIntranet)
        {
            //Koden är helt oprövad men använder funktionen från CGIXrmExtConnectorService
            CreateIndexRequest request = new CreateIndexRequest() { KbArticleForExternalWeb = null, KbArticleForIntranet = _KbArticleForIntranet, RequestActionType = ActionType.Intranet };
            CreateIndexResponse response = cmsIndexManager.CreateIndex(request);
            return response.IntranetIndexId;
        }

        public IClient CreateClientFromConfig(string sectionName)
        {
            EPiServer.Find.Configuration config = (EPiServer.Find.Configuration)ConfigurationManager.GetSection(sectionName);
            IClient client = new Client(config.ServiceUrl, config.DefaultIndex);
            return client;
        }

        public Article GetKBArticle(Entity e, bool isIntranetArticle)
        {
            Article kbArticle = new Article
            {
                Title = e.Attributes["title"].ToString(),
                KnowledgeBaseArticleId = e.Attributes["number"].ToString(),
                ExternalText =
                    e.Attributes.Contains("cgi_externaltext")
                        ? e.Attributes["cgi_externaltext"].ToString().Replace("___", "")
                        : string.Empty,
                Authors = new[] { ((EntityReference)e.Attributes["modifiedby"]).Name },
                PublishDate = (DateTime)e.Attributes["createdon"],
                Category = e.Attributes["keywords"].ToString(),
                UpdateDate = (DateTime)e.Attributes["modifiedon"],
                Summary =
                    e.Attributes.Contains("subjectid")
                        ? ((EntityReference)e.Attributes["subjectid"]).Name
                        : string.Empty
            };

            if (isIntranetArticle)
                kbArticle.InternalText = e.Attributes.Contains("content") ? e.Attributes["content"].ToString().Replace("___", "") : string.Empty;

            return kbArticle;
        }

        private XrmManager _initManager()
        {
            try
            {
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                return xrmMgr;
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

    }
}
