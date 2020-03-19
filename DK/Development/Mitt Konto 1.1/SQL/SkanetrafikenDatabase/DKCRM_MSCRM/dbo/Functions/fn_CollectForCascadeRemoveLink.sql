CREATE function fn_CollectForCascadeRemoveLink
(
@c CascadeCollectionTable READONLY,
@is_delete_entity_schema int,
@topType int,
@f int
)
returns @t table
( 
o uniqueidentifier NOT NULL,
t int NOT NULL,
c int NOT NULL,
PRIMARY KEY CLUSTERED(t,o,c)
)as begin
declare @y int
DECLARE yc CURSOR LOCAL FAST_FORWARD
FOR SELECT DISTINCT t FROM @c
OPEN yc
FETCH NEXT FROM yc INTO @y
while(0=@@FETCH_STATUS)begin
if (@topType = @y and @is_delete_entity_schema = 1) goto a
if(@y=9333)begin
insert into @t(o,t,c)select o.SolutionId,7100,36 from Solution o,@c c where c.t=9333 and c.o=o.ConfigurationPageId
goto a end
if(@y=2024)begin
insert into @t(o,t,c)select o.QueueId,2020,1 from Queue o,@c c where c.t=2024 and c.o=o.QueueId
goto a end
if(@y=10056)begin
insert into @t(o,t,c)select o.CampaignId,4400,10050 from Campaign o,@c c where c.t=10056 and c.o=o.cgi_trafficcontractId
goto a end
if(@y=2023)begin
insert into @t(o,t,c)select o.QueueId,2020,1 from Queue o,@c c where c.t=2023 and c.o=o.QueueId
goto a end
if(@y=10029 and @f=0)begin
insert into @t(o,t,c)select o.cgi_travelcardId,10027,44 from cgi_travelcard o,@c c where c.t=10029 and c.o=o.cgi_CardTypeid
goto a end
if(@y=7100 and @f=0)begin
insert into @t(o,t,c)select o.DependencyNodeId,7106,5 from DependencyNode o,@c c where c.t=7100 and c.o=o.TopSolutionId
goto a end
if(@y=113)begin
insert into @t(o,t,c)select o.IncidentId,112,1 from Incident o,@c c where c.t=113 and c.o=o.IncidentId
goto a end
if(@y=3231)begin
insert into @t(o,t,c)select o.ConnectionId,3234,18 from Connection o,@c c where c.t=3231 and c.o=o.Record1RoleId
insert into @t(o,t,c)select o.ConnectionId,3234,16 from Connection o,@c c where c.t=3231 and c.o=o.Record2RoleId
goto a end
if(@y=10015 and @f=0)begin
insert into @t(o,t,c)select o.cgi_refundId,10019,47 from cgi_refund o,@c c where c.t=10015 and c.o=o.cgi_InvoiceRecipient
goto a end
if(@y=4411 and @f=0)begin
insert into @t(o,t,c)select o.ImportFileId,4412,34 from ImportFile o,@c c where c.t=4411 and c.o=o.ImportMapId
goto a end
if(@y=4009)begin
insert into @t(o,t,c)select o.ResourceId,4002,10 from Resource o,@c c where c.t=4009 and c.o=o.SiteId
goto a end
if(@y=4102 and @f=0)begin
insert into @t(o,t,c)select o.DisplayStringMapId,4101,3 from DisplayStringMapAsIfPublished o,@c c where c.t=4102 and c.o=o.DisplayStringId
goto a end
if(@y=10027)begin
insert into @t(o,t,c)select o.IncidentId,112,10031 from Incident o,@c c where c.t=10027 and c.o=o.cgi_TravelCardid
if(@f=0)insert into @t(o,t,c)select o.cgi_travelcardtransactionId,10028,43 from cgi_travelcardtransaction o,@c c where c.t=10027 and c.o=o.cgi_TravelCardid
goto a end
if(@y=1056)begin
insert into @t(o,t,c)select o.ProductPriceLevelId,1026,4 from ProductPriceLevel o,@c c where c.t=1056 and c.o=o.UoMScheduleId
insert into @t(o,t,c)select o.ContractDetailId,1011,31 from ContractDetail o,@c c where c.t=1056 and c.o=o.UoMScheduleId
goto a end
if(@y=10047)begin
insert into @t(o,t,c)select o.IncidentId,112,10150 from Incident o,@c c where c.t=10047 and c.o=o.cgi_letter_templateId
goto a end
if(@y=4500)begin
insert into @t(o,t,c)select o.CustomerOpportunityRoleId,4503,13 from CustomerOpportunityRole o,@c c where c.t=4500 and c.o=o.OpportunityRoleId
goto a end
if(@y=1022)begin
insert into @t(o,t,c)select o.CampaignId,4400,8 from Campaign o,@c c where c.t=1022 and c.o=o.PriceListId
insert into @t(o,t,c)select o.OpportunityId,3,3 from Opportunity o,@c c where c.t=1022 and c.o=o.PriceLevelId
goto a end
if(@y=129)begin
insert into @t(o,t,c)select o.SubjectId,129,6 from Subject o,@c c where c.t=129 and c.o=o.ParentSubject
goto a end
if(@y=10018 and @f=0)begin
insert into @t(o,t,c)select o.cgi_localizedlabelId,10017,31 from cgi_localizedlabel o,@c c where c.t=10018 and c.o=o.cgi_LocalizedLabelGroupid
goto a end
if(@y=2010)begin
insert into @t(o,t,c)select o.OrganizationId,1019,65 from Organization o,@c c where c.t=2010 and c.o=o.AcknowledgementTemplateId
goto a end
if(@y=10016 and @f=0)begin
insert into @t(o,t,c)select o.cgi_localizedlabelId,10017,29 from cgi_localizedlabel o,@c c where c.t=10016 and c.o=o.cgi_LocalizationLanguageid
goto a end
if(@y=10041)begin
insert into @t(o,t,c)select o.IncidentId,112,10139 from Incident o,@c c where c.t=10041 and c.o=o.cgi_Representativid
insert into @t(o,t,c)select o.ContactId,2,10024 from Contact o,@c c where c.t=10041 and c.o=o.cgi_Representativeid
goto a end
if(@y=123)begin
insert into @t(o,t,c)select o.ActivityId,4208,12 from OpportunityClose o,@c c where c.t=123 and c.o=o.CompetitorId
goto a end
if(@y=2020)begin
insert into @t(o,t,c)select o.SystemUserId,8,135 from SystemUser o,@c c where c.t=2020 and c.o=o.QueueId
insert into @t(o,t,c)select o.TeamId,9,95 from Team o,@c c where c.t=2020 and c.o=o.QueueId
if(@f=0)begin
insert into @t(o,t,c)select o.cgi_settingId,10026,36 from cgi_setting o,@c c where c.t=2020 and c.o=o.cgi_Defaultoutgoingemailqueue
insert into @t(o,t,c)select o.cgi_settingId,10026,97 from cgi_setting o,@c c where c.t=2020 and c.o=o.cgi_case_rgol_defaultqueue
end goto a end
if(@y=9606)begin
insert into @t(o,t,c)select o.QueueId,2020,113 from Queue o,@c c where c.t=9606 and c.o=o.DefaultMailbox
insert into @t(o,t,c)select o.SystemUserId,8,155 from SystemUser o,@c c where c.t=9606 and c.o=o.DefaultMailbox
goto a end
if(@y=9600)begin
insert into @t(o,t,c)select o.GoalId,9600,39 from Goal o,@c c where c.t=9600 and c.o=o.ParentGoalId
goto a end
if(@y=8 and @f=0)begin
insert into @t(o,t,c)select o.cgi_settingId,10026,94 from cgi_setting o,@c c where c.t=8 and c.o=o.cgi_case_rgol_defaultowner
insert into @t(o,t,c)select o.mbs_pluginprofileId,10057,48 from mbs_pluginprofile o,@c c where c.t=8 and c.o=o.mbs_InitiatingUserId
insert into @t(o,t,c)select o.cgi_settingId,10026,72 from cgi_setting o,@c c where c.t=8 and c.o=o.cgi_userid
insert into @t(o,t,c)select o.msdyn_wallsavedqueryusersettingsId,10004,50 from msdyn_wallsavedqueryusersettings o,@c c where c.t=8 and c.o=o.msdyn_userid
goto a end
if(@y=9750)begin
insert into @t(o,t,c)select o.IncidentId,112,130 from Incident o,@c c where c.t=9750 and c.o=o.SLAInvokedId
if(@f=0)begin
insert into @t(o,t,c)select o.EntitlementId,9700,47 from Entitlement o,@c c where c.t=9750 and c.o=o.SLAId
insert into @t(o,t,c)select o.EntitlementTemplateId,9702,43 from EntitlementTemplate o,@c c where c.t=9750 and c.o=o.SLAId
end goto a end
if(@y=4703 and @f=0)begin
insert into @t(o,t,c)select o.AsyncOperationId,4700,32 from AsyncOperation o,@c c where c.t=4703 and c.o=o.WorkflowActivationId
goto a end
if(@y=4710 and @f=0)begin
insert into @t(o,t,c)select o.ProcessSessionId,4710,36 from ProcessSession o,@c c where c.t=4710 and c.o=o.PreviousLinkedSessionId
insert into @t(o,t,c)select o.ProcessSessionId,4710,24 from ProcessSession o,@c c where c.t=4710 and c.o=o.NextLinkedSessionId
insert into @t(o,t,c)select o.ProcessSessionId,4710,26 from ProcessSession o,@c c where c.t=4710 and c.o=o.OriginatingSessionId
goto a end
if(@y=9702 and @f=0)begin
insert into @t(o,t,c)select o.EntitlementId,9700,52 from Entitlement o,@c c where c.t=9702 and c.o=o.EntitlementTemplateId
goto a end
if(@y=9605)begin
insert into @t(o,t,c)select o.OrganizationId,1019,237 from Organization o,@c c where c.t=9605 and c.o=o.DefaultEmailServerProfileId
insert into @t(o,t,c)select o.MailboxId,9606,50 from Mailbox o,@c c where c.t=9605 and c.o=o.EmailServerProfile
goto a end
if(@y=9100)begin
insert into @t(o,t,c)select o.ReportLinkId,9104,13 from ReportLink o,@c c where c.t=9100 and c.o=o.LinkedReportId
insert into @t(o,t,c)select o.ReportId,9100,23 from ReportAsIfPublished o,@c c where c.t=9100 and c.o=o.ParentReportId
goto a end
if(@y=10013 and @f=0)begin
insert into @t(o,t,c)select o.cgi_emailrecipientId,10014,36 from cgi_emailrecipient o,@c c where c.t=10013 and c.o=o.cgi_EmailGroupid
goto a end
if(@y=10035 and @f=0)begin
insert into @t(o,t,c)select o.cgi_refundId,10019,64 from cgi_refund o,@c c where c.t=10035 and c.o=o.cgi_transportcompanyid
goto a end
if(@y=4300 and @f=0)begin
insert into @t(o,t,c)select o.st_external_listId,10064,34 from st_external_list o,@c c where c.t=4300 and c.o=o.st_MarketingListId
goto a end
if(@y=10030)begin
insert into @t(o,t,c)select o.IncidentId,112,10165 from Incident o,@c c where c.t=10030 and c.o=o.cgi_TravelInformationLookup
goto a end
if(@y=10010)begin
insert into @t(o,t,c)select o.ActivityId,10008,95 from cgi_callguidechat o,@c c where c.t=10010 and c.o=o.cgi_CallguideInfoid
insert into @t(o,t,c)select o.ActivityId,10009,95 from cgi_callguidefacebook o,@c c where c.t=10010 and c.o=o.cgi_CallguideInfoid
insert into @t(o,t,c)select o.IncidentId,112,10027 from Incident o,@c c where c.t=10010 and c.o=o.cgi_CallGuideInfo
insert into @t(o,t,c)select o.ActivityId,4210,10003 from PhoneCall o,@c c where c.t=10010 and c.o=o.cgi_CallguideInfoid
goto a end
if(@y=10 and @f=0)begin
insert into @t(o,t,c)select o.mbs_pluginprofileId,10057,46 from mbs_pluginprofile o,@c c where c.t=10 and c.o=o.mbs_BusinessUnitId
goto a end
if(@y=4003)begin
insert into @t(o,t,c)select o.ServiceId,4001,15 from Service o,@c c where c.t=4003 and c.o=o.CalendarId
insert into @t(o,t,c)select o.CalendarId,4003,38 from Calendar o,@c c where c.t=4003 and c.o=o.HolidayScheduleCalendarId
insert into @t(o,t,c)select o.CalendarRuleId,4004,25 from CalendarRule o,@c c where c.t=4003 and c.o=o.InnerCalendarId
insert into @t(o,t,c)select o.BusinessUnitId,10,85 from BusinessUnit o,@c c where c.t=4003 and c.o=o.CalendarId
insert into @t(o,t,c)select o.OrganizationId,1019,69 from Organization o,@c c where c.t=4003 and c.o=o.BusinessClosureCalendarId
goto a end
if(@y=4000)begin
insert into @t(o,t,c)select o.AccountId,1,164 from Account o,@c c where c.t=4000 and c.o=o.PreferredEquipmentId
insert into @t(o,t,c)select o.ContactId,2,183 from Contact o,@c c where c.t=4000 and c.o=o.PreferredEquipmentId
goto a end
if(@y=9700)begin
insert into @t(o,t,c)select o.IncidentId,112,134 from Incident o,@c c where c.t=9700 and c.o=o.EntitlementId
goto a end
if(@y=10012)begin
insert into @t(o,t,c)select o.IncidentId,112,10093 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row3_cat1Id
insert into @t(o,t,c)select o.IncidentId,112,10079 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row1_cat2Id
insert into @t(o,t,c)select o.IncidentId,112,10077 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row1_cat1Id
insert into @t(o,t,c)select o.IncidentId,112,10095 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row3_cat3Id
insert into @t(o,t,c)select o.IncidentId,112,10101 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row4_cat3Id
insert into @t(o,t,c)select o.IncidentId,112,10081 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row1_cat3Id
insert into @t(o,t,c)select o.IncidentId,112,10087 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row2_cat3Id
insert into @t(o,t,c)select o.IncidentId,112,10091 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row3_cat2Id
insert into @t(o,t,c)select o.IncidentId,112,10036 from Incident o,@c c where c.t=10012 and c.o=o.cgi_OriginalCallguideCategory
insert into @t(o,t,c)select o.IncidentId,112,10085 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row2_cat2Id
insert into @t(o,t,c)select o.IncidentId,112,10083 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row2_cat1Id
insert into @t(o,t,c)select o.IncidentId,112,10097 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row4_cat1Id
insert into @t(o,t,c)select o.IncidentId,112,10099 from Incident o,@c c where c.t=10012 and c.o=o.cgi_casdet_row4_cat2Id
if(@f=0)begin
insert into @t(o,t,c)select o.cgi_categorydetailId,10012,45 from cgi_categorydetail o,@c c where c.t=10012 and c.o=o.cgi_parentid2
insert into @t(o,t,c)select o.cgi_settingId,10026,84 from cgi_setting o,@c c where c.t=10012 and c.o=o.cgi_category_detail3id
insert into @t(o,t,c)select o.cgi_categorydetailId,10012,38 from cgi_categorydetail o,@c c where c.t=10012 and c.o=o.cgi_Parentid
insert into @t(o,t,c)select o.cgi_casecategoryId,10011,34 from cgi_casecategory o,@c c where c.t=10012 and c.o=o.cgi_Category1id
insert into @t(o,t,c)select o.cgi_settingId,10026,86 from cgi_setting o,@c c where c.t=10012 and c.o=o.cgi_category_detail2id
insert into @t(o,t,c)select o.cgi_casecategoryId,10011,36 from cgi_casecategory o,@c c where c.t=10012 and c.o=o.cgi_Category2id
insert into @t(o,t,c)select o.cgi_settingId,10026,90 from cgi_setting o,@c c where c.t=10012 and c.o=o.cgi_category_detail1id
insert into @t(o,t,c)select o.cgi_casecategoryId,10011,38 from cgi_casecategory o,@c c where c.t=10012 and c.o=o.cgi_Category3id
end goto a end
if(@y=10014)begin
insert into @t(o,t,c)select o.ActivityId,4202,10007 from Email o,@c c where c.t=10014 and c.o=o.cgi_email_recipient_Id
insert into @t(o,t,c)select o.ActivityId,4202,10012 from Email o,@c c where c.t=10014 and c.o=o.cgi_cc_emailrecipient
insert into @t(o,t,c)select o.ActivityId,4202,10014 from Email o,@c c where c.t=10014 and c.o=o.cgi_bcc_emailrecipient
goto a end
if(@y=4413 and @f=0)begin
insert into @t(o,t,c)select o.ImportLogId,4423,19 from ImportLog o,@c c where c.t=4413 and c.o=o.ImportDataId
goto a end
if(@y=10042 and @f=0)begin
insert into @t(o,t,c)select o.cgi_refundtypeId,10022,41 from cgi_refundtype o,@c c where c.t=10042 and c.o=o.cgi_refundresponsibleId
insert into @t(o,t,c)select o.cgi_refundId,10019,84 from cgi_refund o,@c c where c.t=10042 and c.o=o.cgi_responsibleId
goto a end
if(@y=10021 and @f=0)begin
insert into @t(o,t,c)select o.cgi_refundtypeId,10022,43 from cgi_refundtype o,@c c where c.t=10021 and c.o=o.cgi_refundproductid
insert into @t(o,t,c)select o.cgi_refundId,10019,51 from cgi_refund o,@c c where c.t=10021 and c.o=o.cgi_Productid
goto a end
if(@y=10020 and @f=0)begin
insert into @t(o,t,c)select o.cgi_refundtypeId,10022,39 from cgi_refundtype o,@c c where c.t=10020 and c.o=o.cgi_refundaccountid
insert into @t(o,t,c)select o.cgi_refundId,10019,49 from cgi_refund o,@c c where c.t=10020 and c.o=o.cgi_Accountid
goto a end
if(@y=10023)begin
insert into @t(o,t,c)select o.IncidentId,112,10198 from Incident o,@c c where c.t=10023 and c.o=o.cgi_RefundReimbursementForm
if(@f=0)begin
insert into @t(o,t,c)select o.cgi_rgolsettingId,10040,37 from cgi_rgolsetting o,@c c where c.t=10023 and c.o=o.cgi_ReimbursementFormid
insert into @t(o,t,c)select o.cgi_refundId,10019,55 from cgi_refund o,@c c where c.t=10023 and c.o=o.cgi_ReimbursementFormid
end goto a end
if(@y=10022)begin
insert into @t(o,t,c)select o.IncidentId,112,10188 from Incident o,@c c where c.t=10022 and c.o=o.cgi_RefundTypes
if(@f=0)begin
insert into @t(o,t,c)select o.cgi_refundId,10019,53 from cgi_refund o,@c c where c.t=10022 and c.o=o.cgi_RefundTypeid
insert into @t(o,t,c)select o.cgi_settingId,10026,92 from cgi_setting o,@c c where c.t=10022 and c.o=o.cgi_refundtypeproductnotrequiredid
insert into @t(o,t,c)select o.cgi_rgolsettingId,10040,39 from cgi_rgolsetting o,@c c where c.t=10022 and c.o=o.cgi_RefundTypeid
end goto a end
if(@y=4001)begin
insert into @t(o,t,c)select o.AccountId,1,163 from Account o,@c c where c.t=4001 and c.o=o.PreferredServiceId
insert into @t(o,t,c)select o.ContactId,2,174 from Contact o,@c c where c.t=4001 and c.o=o.PreferredServiceId
insert into @t(o,t,c)select o.ActivityId,4200,10 from ActivityPointer o,@c c where c.t=4001 and c.o=o.ServiceId
goto a end
if(@y=4400)begin
insert into @t(o,t,c)select o.LeadId,4,133 from Lead o,@c c where c.t=4400 and c.o=o.CampaignId
insert into @t(o,t,c)select o.OpportunityId,3,63 from Opportunity o,@c c where c.t=4400 and c.o=o.CampaignId
insert into @t(o,t,c)select o.QuoteId,1084,90 from Quote o,@c c where c.t=4400 and c.o=o.CampaignId
insert into @t(o,t,c)select o.SalesOrderId,1088,95 from SalesOrder o,@c c where c.t=4400 and c.o=o.CampaignId
goto a end
if(@y=1)begin
insert into @t(o,t,c)select o.ActivityId,4202,94 from Email o,@c c where c.t=1 and c.o=o.SendersAccount and o.SendersAccountObjectTypeCode=1
insert into @t(o,t,c)select o.OpportunityId,3,131 from Opportunity o,@c c where c.t=1 and c.o=o.ParentAccountId
insert into @t(o,t,c)select o.IncidentId,112,10208 from Incident o,@c c where c.t=1 and c.o=o.cgi_refundtransportcompanyid
insert into @t(o,t,c)select o.LeadId,4,174 from Lead o,@c c where c.t=1 and c.o=o.ParentAccountId
insert into @t(o,t,c)select o.IncidentId,112,10018 from Incident o,@c c where c.t=1 and c.o=o.cgi_Accountid
insert into @t(o,t,c)select o.AccountId,1,161 from Account o,@c c where c.t=1 and c.o=o.MasterId
insert into @t(o,t,c)select o.AccountId,1,57 from Account o,@c c where c.t=1 and c.o=o.ParentAccountId
if(@f=0)begin
insert into @t(o,t,c)select o.ActivityId,4216,96 from SocialActivity o,@c c where c.t=1 and c.o=o.PostAuthorAccount and o.PostAuthorAccountType=1
insert into @t(o,t,c)select o.cgi_travelcardId,10027,41 from cgi_travelcard o,@c c where c.t=1 and c.o=o.cgi_Accountid
insert into @t(o,t,c)select o.EntitlementId,9700,44 from Entitlement o,@c c where c.t=1 and c.o=o.CustomerId and o.CustomerIdType=1
insert into @t(o,t,c)select o.cgi_settingId,10026,33 from cgi_setting o,@c c where c.t=1 and c.o=o.cgi_DefaultCustomerOnCase
insert into @t(o,t,c)select o.EntitlementId,9700,110 from Entitlement o,@c c where c.t=1 and c.o=o.AccountId and o.CustomerIdType=1
insert into @t(o,t,c)select o.ActivityId,4216,101 from SocialActivity o,@c c where c.t=1 and c.o=o.PostAuthor and o.PostAuthorType=1
insert into @t(o,t,c)select o.cgi_creditorderrowId,10044,44 from cgi_creditorderrow o,@c c where c.t=1 and c.o=o.cgi_Accountid
end goto a end
if(@y=2)begin
insert into @t(o,t,c)select o.IncidentId,112,10033 from Incident o,@c c where c.t=2 and c.o=o.cgi_ThirdpartyNameid
insert into @t(o,t,c)select o.ContactId,2,179 from Contact o,@c c where c.t=2 and c.o=o.ParentCustomerId and o.ParentCustomerIdType=2
insert into @t(o,t,c)select o.OpportunityId,3,135 from Opportunity o,@c c where c.t=2 and c.o=o.ParentContactId
insert into @t(o,t,c)select o.IncidentId,112,330 from Incident o,@c c where c.t=2 and c.o=o.PrimaryContactId
insert into @t(o,t,c)select o.IncidentId,112,36 from Incident o,@c c where c.t=2 and c.o=o.ResponsibleContactId
insert into @t(o,t,c)select o.ContactId,2,175 from Contact o,@c c where c.t=2 and c.o=o.MasterId
insert into @t(o,t,c)select o.AccountId,1,20 from Account o,@c c where c.t=2 and c.o=o.PrimaryContactId
insert into @t(o,t,c)select o.IncidentId,112,10115 from Incident o,@c c where c.t=2 and c.o=o.cgi_Contactid
insert into @t(o,t,c)select o.LeadId,4,175 from Lead o,@c c where c.t=2 and c.o=o.ParentContactId
if(@f=0)begin
insert into @t(o,t,c)select o.cgi_refundId,10019,80 from cgi_refund o,@c c where c.t=2 and c.o=o.cgi_Contactid
insert into @t(o,t,c)select o.cgi_travelcardId,10027,49 from cgi_travelcard o,@c c where c.t=2 and c.o=o.cgi_Contactid
insert into @t(o,t,c)select o.cgi_creditorderrowId,10044,47 from cgi_creditorderrow o,@c c where c.t=2 and c.o=o.cgi_Contactid
insert into @t(o,t,c)select o.EntitlementId,9700,114 from Entitlement o,@c c where c.t=2 and c.o=o.ContactId
insert into @t(o,t,c)select o.ActivityId,4216,101 from SocialActivity o,@c c where c.t=2 and c.o=o.PostAuthor and o.PostAuthorType=2
insert into @t(o,t,c)select o.EntitlementId,9700,44 from Entitlement o,@c c where c.t=2 and c.o=o.CustomerId and o.CustomerIdType=2
insert into @t(o,t,c)select o.ActivityId,4216,96 from SocialActivity o,@c c where c.t=2 and c.o=o.PostAuthorAccount and o.PostAuthorAccountType=2
insert into @t(o,t,c)select o.st_external_listId,10064,36 from st_external_list o,@c c where c.t=2 and c.o=o.st_Contact
end goto a end
if(@y=99)begin
insert into @t(o,t,c)select o.IncidentId,112,119 from Incident o,@c c where c.t=99 and c.o=o.SocialProfileId
goto a end
if(@y=4502)begin
insert into @t(o,t,c)select o.CustomerRelationshipId,4502,11 from CustomerRelationship o,@c c where c.t=4502 and c.o=o.ConverseRelationshipId
goto a end
if(@y=4)begin
insert into @t(o,t,c)select o.ContactId,2,8 from Contact o,@c c where c.t=4 and c.o=o.OriginatingLeadId
insert into @t(o,t,c)select o.OpportunityId,3,23 from Opportunity o,@c c where c.t=4 and c.o=o.OriginatingLeadId
insert into @t(o,t,c)select o.LeadId,4,132 from Lead o,@c c where c.t=4 and c.o=o.MasterId
insert into @t(o,t,c)select o.AccountId,1,17 from Account o,@c c where c.t=4 and c.o=o.OriginatingLeadId
goto a end
if(@y=3)begin
insert into @t(o,t,c)select o.LeadId,4,214 from Lead o,@c c where c.t=3 and c.o=o.QualifyingOpportunityId
goto a end
if(@y=9 and @f=0)begin
insert into @t(o,t,c)select o.cgi_settingId,10026,69 from cgi_setting o,@c c where c.t=9 and c.o=o.cgi_DefaultTeamonPASSCase
goto a end
if(@y=112)begin
insert into @t(o,t,c)select o.IncidentId,112,101 from Incident o,@c c where c.t=112 and c.o=o.ExistingCase
insert into @t(o,t,c)select o.IncidentId,112,10186 from Incident o,@c c where c.t=112 and c.o=o.cgi_UnderordnaderendenId
insert into @t(o,t,c)select o.LeadId,4,224 from Lead o,@c c where c.t=112 and c.o=o.OriginatingCaseId
insert into @t(o,t,c)select o.IncidentId,112,137 from Incident o,@c c where c.t=112 and c.o=o.MasterId
if(@f=0)begin
insert into @t(o,t,c)select o.cgi_refundId,10019,57 from cgi_refund o,@c c where c.t=112 and c.o=o.cgi_Caseid
insert into @t(o,t,c)select o.cgi_casecategoryId,10011,40 from cgi_casecategory o,@c c where c.t=112 and c.o=o.cgi_Caseid
insert into @t(o,t,c)select o.cgi_travelcardtransactionId,10028,55 from cgi_travelcardtransaction o,@c c where c.t=112 and c.o=o.cgi_caseId
insert into @t(o,t,c)select o.cgi_travelinformationId,10030,45 from cgi_travelinformation o,@c c where c.t=112 and c.o=o.cgi_Caseid
end goto a end
if(@y=4202)begin
insert into @t(o,t,c)select o.ActivityId,4202,102 from Email o,@c c where c.t=4202 and c.o=o.ParentActivityId
goto a end
if(@y=10009)begin
insert into @t(o,t,c)select o.IncidentId,112,10025 from Incident o,@c c where c.t=10009 and c.o=o.cgi_FacebookPostid
goto a end
if(@y=10008)begin
insert into @t(o,t,c)select o.IncidentId,112,10023 from Incident o,@c c where c.t=10008 and c.o=o.cgi_Chatid
goto a end
if(@y=4401)begin
insert into @t(o,t,c)select o.LeadId,4,182 from Lead o,@c c where c.t=4401 and c.o=o.RelatedObjectId
goto a end
if(@y=3234)begin
insert into @t(o,t,c)select o.ConnectionId,3234,10 from Connection o,@c c where c.t=3234 and c.o=o.RelatedConnectionId
goto a end
a:
if(@y in(1,2,3,4,112,1010,1084,1088,1090,4400,4402,4406,9700,9702,10000,10014,10025,10027,10064))
insert into @t(o,t,c)select o.ActivityId,4200,21 from ActivityPointer o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(1,2,3,4,112,1010,1084,1088,1090,4400,9700))
insert into @t(o,t,c)select o.ActivityId,4214,32 from ServiceAppointment o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(1,2,4,8,2020,4000))
insert into @t(o,t,c)select o.ActivityId,4202,90 from Email o,@c c where c.t=@y and c.o=o.EmailSender and o.EmailSenderObjectTypeCode=@y
if(1=@is_delete_entity_schema)begin
if(@y in(10014,10000,10064,10027,10025))
insert into @t(o,t,c)select o.ActivityId,4214,32 from ServiceAppointment o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10064,10000,10025,10014,10027))
insert into @t(o,t,c)select o.ActivityId,4216,64 from SocialActivity o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10000,10014,10064,10025,10027))
insert into @t(o,t,c)select o.ActivityId,4204,1 from Fax o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10027,10025,10064,10014,10000))
insert into @t(o,t,c)select o.ActivityId,4202,36 from Email o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10064,10000,10014,10027,10025))
insert into @t(o,t,c)select o.ActivityId,4212,4 from Task o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10027,10014,10000,10064,10025))
insert into @t(o,t,c)select o.ActivityId,4251,38 from RecurringAppointmentMaster o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10064,10027,10014,10000,10025))
insert into @t(o,t,c)select o.ActivityId,4201,10 from Appointment o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10027,10014,10000,10025,10064))
insert into @t(o,t,c)select o.ActivityId,4210,10 from PhoneCall o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10064,10000,10014,10027,10025))
insert into @t(o,t,c)select o.ActivityId,4207,4 from Letter o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10000,10027,10014,10064,10025))
insert into @t(o,t,c)select o.ActivityId,10009,29 from cgi_callguidefacebook o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10000,10027,10014,10064,10025))
insert into @t(o,t,c)select o.ActivityId,10008,29 from cgi_callguidechat o,@c c where c.t=@y and c.o=o.RegardingObjectId and o.RegardingObjectTypeCode=@y
if(@y in(10008,10009))
insert into @t(o,t,c)select o.ActivityId,4401,33 from CampaignResponse o,@c c where c.t=@y and c.o=o.OriginatingActivityId and o.OriginatingActivityIdTypeCode=@y
end
FETCH NEXT FROM yc INTO @y
end
CLOSE yc
DEALLOCATE yc
return
end
