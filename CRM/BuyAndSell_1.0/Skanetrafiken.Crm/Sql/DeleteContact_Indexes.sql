CREATE NONCLUSTERED INDEX IX_ED_IncidentBase_PrimaryContactId ON IncidentBase (PrimaryContactId)
CREATE NONCLUSTERED INDEX IX_ED_IncidentBase_cgi_ThirdPartyNameId ON IncidentBase (cgi_ThirdpartyNameid)
CREATE NONCLUSTERED INDEX IX_ED_IncidentBase_cgi_ContactId ON IncidentBase (cgi_ContactId)
CREATE NONCLUSTERED INDEX IX_ED_st_update_contactbase_st_ContactLookup ON st_update_contactBase (st_ContactLookup)
--CREATE NONCLUSTERED INDEX IX_ED_st_templastloginupdateBase_st_Cont ON st_templastloginupdateBase (st_Cont) --table only existed in UAT environment, not in Prod.
CREATE NONCLUSTERED INDEX IX_ED_cgi_refundBase_cgi_Contactid ON cgi_refundBase (cgi_Contactid)
CREATE NONCLUSTERED INDEX IX_ED_cgi_travelcardBase_cgi_Contactid ON cgi_travelcardBase (cgi_Contactid)
--CREATE NONCLUSTERED INDEX IX_ED_st_update_mkl_idBase_st_contactid ON st_update_mkl_idBase (st_contactid)
  -- drop INDEX st_update_mkl_idBase.IX_ED_st_update_mkl_idBase_st_contactid
CREATE NONCLUSTERED INDEX IX_ED_cgi_creditorderrowBase_cgi_Contactid ON cgi_creditorderrowBase (cgi_contactid)
CREATE NONCLUSTERED INDEX IX_ED_st_discountcodeBase_st_contact ON st_discountcodeBase (st_contact)

--Added per 2017-06-09:
CREATE NONCLUSTERED INDEX IX_ED_cdi_sentemailBase_cdi_contactid ON cdi_sentemailBase (cdi_contactid)
CREATE NONCLUSTERED INDEX IX_ED_cdi_unsubscribeBase_cdi_contactid ON cdi_unsubscribeBase (cdi_contactid)
CREATE NONCLUSTERED INDEX IX_ED_st_update_markupnoteBase_ContantLookup ON st_update_markupnoteBase (st_MarkUpContactLookup) -- did not exist in UAT env.
CREATE NONCLUSTERED INDEX IX_ED_cdi_excludedemailBase_cdi_contactid ON cdi_excludedemailBase (cdi_contactid)
CREATE NONCLUSTERED INDEX IX_ED_cdi_emaileventBase_cdi_contactid ON cdi_emaileventBase (cdi_contactid)