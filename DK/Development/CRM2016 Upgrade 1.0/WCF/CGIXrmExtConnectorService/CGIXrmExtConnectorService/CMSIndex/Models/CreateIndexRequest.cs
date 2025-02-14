﻿using System.Runtime.Serialization;
namespace CGIXrmExtConnectorService
{
    [DataContract]
    public class CreateIndexRequest
    {
        //[DataMember]
        //public KBArticle KbArticleForIntranet { get; set; }
        //[DataMember]
        //public KBArticle KbArticleForExternalWeb { get; set; }
        [DataMember]
        public Article KbArticleForIntranet { get; set; }
        [DataMember]
        public Article KbArticleForExternalWeb { get; set; }
        [DataMember]
        public ActionType RequestActionType { get; set; }

    }
}