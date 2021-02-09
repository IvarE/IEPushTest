CREATE function fn_CollectForCascadeDeleteSchema
(
@root_otc int,
@f int
)
returns @t table
(
o uniqueidentifier NOT NULL,
t int NOT NULL,
d int NOT NULL
PRIMARY KEY CLUSTERED(t,o,d)
)as begin
declare @y int=@root_otc 
declare @n int=-1
if(@y=10056)begin
insert into @t(o,t,d)select cgi_traffic_contractId,10056,@n from cgi_traffic_contract
goto a end
if(@y=10046)begin
insert into @t(o,t,d)select cgi_amounttranslationId,10046,@n from cgi_amounttranslation
goto a end
if(@y=10039)begin
insert into @t(o,t,d)select cgi_zonenameId,10039,@n from cgi_zonename
goto a end
if(@y=10024)begin
insert into @t(o,t,d)select cgi_routingactionId,10024,@n from cgi_routingaction
goto a end
if(@y=10029)begin
insert into @t(o,t,d)select cgi_travelcardtypeId,10029,@n from cgi_travelcardtype
goto a end
if(@y=10001)begin
insert into @t(o,t,d)select msdyn_PostConfigId,10001,@n from msdyn_PostConfig
goto a end
if(@y=10003)begin
insert into @t(o,t,d)select msdyn_wallsavedqueryId,10003,@n from msdyn_wallsavedquery
goto a end
if(@y=10015)begin
insert into @t(o,t,d)select cgi_invoicerecipientId,10015,@n from cgi_invoicerecipient
goto a end
if(@y=10006)begin
insert into @t(o,t,d)select cgi_autonumberId,10006,@n from cgi_autonumber
goto a end
if(@y=10017)begin
insert into @t(o,t,d)select cgi_localizedlabelId,10017,@n from cgi_localizedlabel
goto a end
if(@y=10027)begin
insert into @t(o,t,d)select cgi_travelcardId,10027,@n from cgi_travelcard
goto a end
if(@y=10002)begin
insert into @t(o,t,d)select msdyn_PostRuleConfigId,10002,@n from msdyn_PostRuleConfig
goto a end
if(@y=10047)begin
insert into @t(o,t,d)select cgi_letter_templateId,10047,@n from cgi_letter_template
goto a end
if(@y=10004)begin
insert into @t(o,t,d)select msdyn_wallsavedqueryusersettingsId,10004,@n from msdyn_wallsavedqueryusersettings
goto a end
if(@y=10040)begin
insert into @t(o,t,d)select cgi_rgolsettingId,10040,@n from cgi_rgolsetting
goto a end
if(@y=10019)begin
insert into @t(o,t,d)select cgi_refundId,10019,@n from cgi_refund
goto a end
if(@y=10018)begin
insert into @t(o,t,d)select cgi_localizedlabelgroupId,10018,@n from cgi_localizedlabelgroup
goto a end
if(@y=10016)begin
insert into @t(o,t,d)select cgi_localizationlanguageId,10016,@n from cgi_localizationlanguage
goto a end
if(@y=10028)begin
insert into @t(o,t,d)select cgi_travelcardtransactionId,10028,@n from cgi_travelcardtransaction
goto a end
if(@y=10007)begin
insert into @t(o,t,d)select cgi_bankId,10007,@n from cgi_bank
goto a end
if(@y=10057)begin
insert into @t(o,t,d)select mbs_pluginprofileId,10057,@n from mbs_pluginprofile
goto a end
if(@y=10041)begin
insert into @t(o,t,d)select cgi_representativeId,10041,@n from cgi_representative
goto a end
if(@y=10011)begin
insert into @t(o,t,d)select cgi_casecategoryId,10011,@n from cgi_casecategory
goto a end
if(@y=10026)begin
insert into @t(o,t,d)select cgi_settingId,10026,@n from cgi_setting
goto a end
if(@y=10044)begin
insert into @t(o,t,d)select cgi_creditorderrowId,10044,@n from cgi_creditorderrow
goto a end
if(@y=10013)begin
insert into @t(o,t,d)select cgi_emailgroupId,10013,@n from cgi_emailgroup
goto a end
if(@y=10035)begin
insert into @t(o,t,d)select cgi_transportcompanyId,10035,@n from cgi_transportcompany
goto a end
if(@y=10064)begin
insert into @t(o,t,d)select st_external_listId,10064,@n from st_external_list
goto a end
if(@y=10030)begin
insert into @t(o,t,d)select cgi_travelinformationId,10030,@n from cgi_travelinformation
goto a end
if(@y=10010)begin
insert into @t(o,t,d)select cgi_callguideinfoId,10010,@n from cgi_callguideinfo
goto a end
if(@y=10012)begin
insert into @t(o,t,d)select cgi_categorydetailId,10012,@n from cgi_categorydetail
goto a end
if(@y=10014)begin
insert into @t(o,t,d)select cgi_emailrecipientId,10014,@n from cgi_emailrecipient
goto a end
if(@y=10005)begin
insert into @t(o,t,d)select cgi_applicationlogId,10005,@n from cgi_applicationlog
goto a end
if(@y=10042)begin
insert into @t(o,t,d)select cgi_refundresponsibleId,10042,@n from cgi_refundresponsible
goto a end
if(@y=10025)begin
insert into @t(o,t,d)select cgi_segmentId,10025,@n from cgi_segment
goto a end
if(@y=10021)begin
insert into @t(o,t,d)select cgi_refundproductId,10021,@n from cgi_refundproduct
goto a end
if(@y=10020)begin
insert into @t(o,t,d)select cgi_refundaccountId,10020,@n from cgi_refundaccount
goto a end
if(@y=10023)begin
insert into @t(o,t,d)select cgi_reimbursementformId,10023,@n from cgi_reimbursementform
goto a end
if(@y=10043)begin
insert into @t(o,t,d)select cgi_reimbursementform_cgi_refundresponsibleId,10043,@n from cgi_reimbursementform_cgi_refundresponsible
goto a end
if(@y=10033)begin
insert into @t(o,t,d)select cgi_cgi_reimbursementform_cgi_refundproductId,10033,@n from cgi_cgi_reimbursementform_cgi_refundproduct
goto a end
if(@y=10032)begin
insert into @t(o,t,d)select cgi_cgi_reimbursementform_cgi_refundaccountId,10032,@n from cgi_cgi_reimbursementform_cgi_refundaccount
goto a end
if(@y=10022)begin
insert into @t(o,t,d)select cgi_refundtypeId,10022,@n from cgi_refundtype
goto a end
if(@y=10031)begin
insert into @t(o,t,d)select cgi_cgi_refundtype_cgi_reimbursementformId,10031,@n from cgi_cgi_refundtype_cgi_reimbursementform
goto a end
if(@y=10000)begin
insert into @t(o,t,d)select msdyn_PostAlbumId,10000,@n from msdyn_PostAlbum
goto a end
if(@y=10034)begin
insert into @t(o,t,d)select cgi_cgi_segment_accountId,10034,@n from cgi_cgi_segment_account
goto a end
if(@y=10037)begin
insert into @t(o,t,d)select cgi_passtravelinformationId,10037,@n from cgi_passtravelinformation
goto a end
if(@y=10055)begin
insert into @t(o,t,d)select cgi_filelinkId,10055,@n from cgi_filelink
goto a end
if(@y=10038 and @f=0)begin
insert into @t(o,t,d)select cgi_account_contactId,10038,@n from cgi_account_contact
goto a end
if(@y=10009 and @f=0)begin
insert into @t(o,t,d)select ActivityId,10009,@n from cgi_callguidefacebook
goto a end
if(@y=10008 and @f=0)begin
insert into @t(o,t,d)select ActivityId,10008,@n from cgi_callguidechat
goto a end
a:
set @n=0
if(@y in(10014,10010,10009,10044,10011,10007,10016,10042,10040,10025,10000,10064,10026,10027,10041,10017,10029,10028,10024,10006,10037,10022,10039,10020,10035,10018,10008,10047,10015,10012,10013,10021,10030,10019,10023))
insert into @t(o,t,d)select DuplicateId,4415,@n from DuplicateRecord where BaseRecordIdTypeCode=@y and DuplicateId not in(select o from @t where t=4415)
if(@y in(10012,10042,10000,10013,10025,10016,10026,10007,10011,10006,10018,10014,10017,10035,10020,10024,10027,10028,10064,10040,10037,10019,10008,10022,10021,10023,10015,10010,10039,10029,10041,10047,10030,10009,10044))
insert into @t(o,t,d)select DuplicateId,4415,@n from DuplicateRecord where DuplicateRecordIdTypeCode=@y and DuplicateId not in(select o from @t where t=4415)
if(@y in(10008,10009))
insert into @t(o,t,d)select QueueItemId,2029,@n from QueueItem where ObjectIdTypeCode=@y
if(@y in(10008,10009,10000,10064))
insert into @t(o,t,d)select ConnectionId,3234,@n from Connection where Record1IdObjectTypeCode=@y and ConnectionId not in(select o from @t where t=3234)
if(@y in(10009,10064,10008,10000))
insert into @t(o,t,d)select ConnectionId,3234,@n from Connection where Record2IdObjectTypeCode=@y and ConnectionId not in(select o from @t where t=3234)
if(@y in(10030,10035,10013,10044,10041,10009,10018,10057,10022,10042,10007,10006,10019,10017,10014,10040,10026,10002,10016,10027,10004,10020,10001,10011,10003,10023,10000,10039,10015,10008,10037,10025,10021,10010,10047,10012,10028,10005,10029,10055,10056,10064,10024,10046))
insert into @t(o,t,d)select PrincipalObjectAttributeAccessId,44,@n from PrincipalObjectAttributeAccess where ObjectTypeCode=@y
if(@y in(10000,10025,10019,10008,10027,10009,10064))
insert into @t(o,t,d)select AnnotationId,5,@n from Annotation where ObjectTypeCode=@y
if(@y in(10005,10037,10041,10028,10004,10047,10035,10002,10057,10042,10013,10012,10040,10018,10025,10001,10010,10055,10029,10023,10064,10000,10011,10027,10017,10046,10016,10019,10014,10008,10015,10026,10020,10009,10030,10056,10044,10007,10024,10022,10021,10006,10039,10003))
insert into @t(o,t,d)select BulkDeleteFailureId,4425,@n from BulkDeleteFailure where RegardingObjectTypeCode=@y
return
end
