CREATE FULLTEXT INDEX ON [dbo].[DocumentIndex]
    ([Title] LANGUAGE 1033, [KeyWords] LANGUAGE 1033, [SearchText] LANGUAGE 1033)
    KEY INDEX [cndx_PrimaryKey_DocumentIndex]
    ON [ftcat_documentindex_5298120688d6e31180cc0050569010ad];

