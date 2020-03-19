using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService.CMSIndex.Models
{
    public class Article
    {
        #region Public Properties -------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>
        /// The authors.
        /// </value>
        [DataMember]
        public IEnumerable<string> Authors { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        [DataMember]
        public IEnumerable<string> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the publish date.
        /// </summary>
        /// <value>
        /// The publish date.
        /// </value>
        [DataMember]
        public System.DateTime? PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>
        /// The update date.
        /// </value>
        [DataMember]
        public System.DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the knowledge base article identifier.
        /// </summary>
        /// <value>
        /// The knowledge base article identifier.
        /// </value>
        [DataMember]
        public string KnowledgeBaseArticleId { get; set; }

        /// <summary>
        /// Gets or sets the article category.
        /// </summary>
        /// <value>
        /// The article category.
        /// </value>
        [DataMember]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        [DataMember]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the internal text.
        /// </summary>
        /// <value>
        /// The internal text.
        /// </value>
        [DataMember]
        public string InternalText { get; set; }

        /// <summary>
        /// Gets or sets the external text.
        /// </summary>
        /// <value>
        /// The external text.
        /// </value>
        [DataMember]
        public string ExternalText { get; set; }

        #endregion
    }
}
