using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EPiServer.Find;
using EPiServer.Find.UnifiedSearch;
using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService
{
    public class KBArticle : ISearchContent
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>
        /// The authors.
        /// </value>
        public IEnumerable<string> Authors { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        public IEnumerable<string> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the publish date.
        /// </summary>
        /// <value>
        /// The publish date.
        /// </value>
        public System.DateTime? PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>
        /// The update date.
        /// </value>
        public System.DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the knowledge base article identifier.
        /// </summary>
        /// <value>
        /// The knowledge base article identifier.
        /// </value>
        public string KnowledgeBaseArticleId { get; set; }

        /// <summary>
        /// Gets or sets the article category.
        /// </summary>
        /// <value>
        /// The article category.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the internal text.
        /// </summary>
        /// <value>
        /// The internal text.
        /// </value>
        public string InternalText { get; set; }

        /// <summary>
        /// Gets or sets the external text.
        /// </summary>
        /// <value>
        /// The external text.
        /// </value>
        public string ExternalText { get; set; }
        
        #region ISearchContent
        /// <summary>
        /// Gets the search attachment.
        /// </summary>
        /// <value>
        /// The search attachment.
        /// </value>
        public Attachment SearchAttachment
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the search authors.
        /// </summary>
        /// <value>
        /// The search authors.
        /// </value>
        public IEnumerable<string> SearchAuthors
        {
            get { return Authors != null ? new ReadOnlyCollection<string>(Authors.ToList()) : null; }
        }

        /// <summary>
        /// Gets the search categories.
        /// </summary>
        /// <value>
        /// The search categories.
        /// </value>
        public IEnumerable<string> SearchCategories
        {
            get { return Keywords != null ? new ReadOnlyCollection<string>(Keywords.ToList()) : null; }
        }

        /// <summary>
        /// Gets or sets the search file extension.
        /// </summary>
        /// <value>
        /// The search file extension.
        /// </value>
        public string SearchFileExtension { get { return null; } set { } }

        /// <summary>
        /// Gets or sets the search filename.
        /// </summary>
        /// <value>
        /// The search filename.
        /// </value>
        public string SearchFilename { get { return null; } set { } }

        /// <summary>
        /// Gets the search geo location.
        /// </summary>
        /// <value>
        /// The search geo location.
        /// </value>
        public GeoLocation SearchGeoLocation
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the name of the search hit type.
        /// </summary>
        /// <value>
        /// The name of the search hit type.
        /// </value>
        public string SearchHitTypeName
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// Gets the search hit URL.
        /// </summary>
        /// <value>
        /// The search hit URL.
        /// </value>
        public string SearchHitUrl
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the search meta data.
        /// </summary>
        /// <value>
        /// The search meta data.
        /// </value>
        public IDictionary<string, IndexValue> SearchMetaData { get { return new Dictionary<string, IndexValue>(); } set { } }

        /// <summary>
        /// Gets the search publish date.
        /// </summary>
        /// <value>
        /// The search publish date.
        /// </value>
        public System.DateTime? SearchPublishDate
        {
            get { return PublishDate; }
        }

        /// <summary>
        /// Gets the search section.
        /// </summary>
        /// <value>
        /// The search section.
        /// </value>
        public string SearchSection
        {
            get { return KnowledgeBaseArticleId; }
        }

        /// <summary>
        /// Gets the search subsection.
        /// </summary>
        /// <value>
        /// The search subsection.
        /// </value>
        public string SearchSubsection
        {
            get { return Category; }
        }

        /// <summary>
        /// Gets the search summary.
        /// </summary>
        /// <value>
        /// The search summary.
        /// </value>
        public string SearchSummary
        {
            get { return Summary; }
        }

        /// <summary>
        /// Gets the search text.
        /// </summary>
        /// <value>
        /// The search text.
        /// </value>
        public string SearchText
        {
            get { return (InternalText ?? "") + " " + (ExternalText ?? "") + " " + (Category ?? ""); }
        }

        /// <summary>
        /// Gets the search title.
        /// </summary>
        /// <value>
        /// The search title.
        /// </value>
        public string SearchTitle
        {
            get { return Title; }
        }

        /// <summary>
        /// Gets the name of the search type.
        /// </summary>
        /// <value>
        /// The name of the search type.
        /// </value>
        public string SearchTypeName
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// Gets the search update date.
        /// </summary>
        /// <value>
        /// The search update date.
        /// </value>
        public System.DateTime? SearchUpdateDate
        {
            get { return UpdateDate; }
        }
        #endregion
        #endregion
    }
}
