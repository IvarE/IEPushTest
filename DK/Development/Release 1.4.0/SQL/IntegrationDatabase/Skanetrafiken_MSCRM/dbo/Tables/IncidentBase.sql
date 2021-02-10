CREATE TABLE [dbo].[IncidentBase] (
    [IncidentId]                            UNIQUEIDENTIFIER NOT NULL,
    [OwningBusinessUnit]                    UNIQUEIDENTIFIER NULL,
    [ContractDetailId]                      UNIQUEIDENTIFIER NULL,
    [SubjectId]                             UNIQUEIDENTIFIER NULL,
    [ContractId]                            UNIQUEIDENTIFIER NULL,
    [ActualServiceUnits]                    INT              NULL,
    [CaseOriginCode]                        INT              NULL,
    [BilledServiceUnits]                    INT              NULL,
    [CaseTypeCode]                          INT              NULL,
    [ProductSerialNumber]                   NVARCHAR (100)   NULL,
    [Title]                                 NVARCHAR (200)   NULL,
    [ProductId]                             UNIQUEIDENTIFIER NULL,
    [ContractServiceLevelCode]              INT              NULL,
    [Description]                           NVARCHAR (MAX)   NULL,
    [IsDecrementing]                        BIT              NULL,
    [CreatedOn]                             DATETIME         NULL,
    [TicketNumber]                          NVARCHAR (100)   NULL,
    [PriorityCode]                          INT              NULL,
    [CustomerSatisfactionCode]              INT              NULL,
    [IncidentStageCode]                     INT              NULL,
    [ModifiedOn]                            DATETIME         NULL,
    [CreatedBy]                             UNIQUEIDENTIFIER NULL,
    [FollowupBy]                            DATETIME         NULL,
    [ModifiedBy]                            UNIQUEIDENTIFIER NULL,
    [VersionNumber]                         ROWVERSION       NULL,
    [StateCode]                             INT              NOT NULL,
    [SeverityCode]                          INT              NULL,
    [StatusCode]                            INT              NULL,
    [ResponsibleContactId]                  UNIQUEIDENTIFIER NULL,
    [KbArticleId]                           UNIQUEIDENTIFIER NULL,
    [TimeZoneRuleVersionNumber]             INT              NULL,
    [ImportSequenceNumber]                  INT              NULL,
    [UTCConversionTimeZoneCode]             INT              NULL,
    [OverriddenCreatedOn]                   DATETIME         NULL,
    [ExchangeRate]                          DECIMAL (23, 10) NULL,
    [ModifiedOnBehalfBy]                    UNIQUEIDENTIFIER NULL,
    [TransactionCurrencyId]                 UNIQUEIDENTIFIER NULL,
    [CreatedOnBehalfBy]                     UNIQUEIDENTIFIER NULL,
    [CustomerId]                            UNIQUEIDENTIFIER NULL,
    [OwnerId]                               UNIQUEIDENTIFIER CONSTRAINT [DF_IncidentBase_OwnerId] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [OwnerIdType]                           INT              CONSTRAINT [DF_IncidentBase_OwnerIdType] DEFAULT ((8)) NOT NULL,
    [CustomerIdType]                        INT              NULL,
    [CustomerIdName]                        NVARCHAR (4000)  NULL,
    [CustomerIdYomiName]                    NVARCHAR (4000)  NULL,
    [ActivitiesComplete]                    BIT              NULL,
    [StageId]                               UNIQUEIDENTIFIER NULL,
    [ExistingCase]                          UNIQUEIDENTIFIER NULL,
    [ServiceStage]                          INT              CONSTRAINT [DF_IncidentBase_ServiceStage] DEFAULT ((0)) NULL,
    [CheckEmail]                            BIT              NULL,
    [FollowUpTaskCreated]                   BIT              NULL,
    [EntityImageId]                         UNIQUEIDENTIFIER NULL,
    [ProcessId]                             UNIQUEIDENTIFIER NULL,
    [cgi_AccountNumber]                     NVARCHAR (100)   NULL,
    [cgi_ChatConversation]                  NVARCHAR (MAX)   NULL,
    [cgi_City]                              NVARCHAR (100)   NULL,
    [cgi_ContactCustomer]                   BIT              NULL,
    [cgi_Country]                           NVARCHAR (100)   NULL,
    [cgi_CustomerNumber]                    NVARCHAR (100)   NULL,
    [cgi_CustomerPayout]                    MONEY            NULL,
    [cgi_customerpayout_Base]               MONEY            NULL,
    [cgi_EmailAddress]                      NVARCHAR (100)   NULL,
    [cgi_GiroNumber]                        NVARCHAR (100)   NULL,
    [cgi_PaymentType]                       INT              NULL,
    [cgi_Postaladdress]                     NVARCHAR (100)   NULL,
    [cgi_SendMailAction]                    NVARCHAR (100)   NULL,
    [cgi_StreetAddress]                     NVARCHAR (100)   NULL,
    [cgi_TelephoneNumber]                   NVARCHAR (100)   NULL,
    [cgi_Accountid]                         UNIQUEIDENTIFIER NULL,
    [cgi_Chatid]                            UNIQUEIDENTIFIER NULL,
    [cgi_FacebookPostid]                    UNIQUEIDENTIFIER NULL,
    [cgi_CallGuideInfo]                     UNIQUEIDENTIFIER NULL,
    [cgi_TravelCardid]                      UNIQUEIDENTIFIER NULL,
    [cgi_ThirdpartyNameid]                  UNIQUEIDENTIFIER NULL,
    [cgi_OriginalCallguideCategory]         UNIQUEIDENTIFIER NULL,
    [SLAInvokedId]                          UNIQUEIDENTIFIER NULL,
    [RouteCase]                             BIT              CONSTRAINT [DF_IncidentBase_RouteCase] DEFAULT ((1)) NULL,
    [IsEscalated]                           BIT              CONSTRAINT [DF_IncidentBase_IsEscalated] DEFAULT ((0)) NULL,
    [CustomerContacted]                     BIT              CONSTRAINT [DF_IncidentBase_CustomerContacted] DEFAULT ((0)) NULL,
    [MasterId]                              UNIQUEIDENTIFIER NULL,
    [BlockedProfile]                        BIT              NULL,
    [PrimaryContactId]                      UNIQUEIDENTIFIER NULL,
    [SentimentValue]                        FLOAT (53)       NULL,
    [Merged]                                BIT              CONSTRAINT [DF_IncidentBase_Merged] DEFAULT ((0)) NULL,
    [ResponseBy]                            DATETIME         NULL,
    [EscalatedOn]                           DATETIME         NULL,
    [ParentCaseId]                          UNIQUEIDENTIFIER NULL,
    [SocialProfileId]                       UNIQUEIDENTIFIER NULL,
    [MessageTypeCode]                       INT              NULL,
    [ResolveBySLAStatus]                    INT              NULL,
    [ResolveBy]                             DATETIME         NULL,
    [InfluenceScore]                        FLOAT (53)       NULL,
    [EntitlementId]                         UNIQUEIDENTIFIER NULL,
    [FirstResponseSLAStatus]                INT              NULL,
    [FirstResponseSent]                     BIT              CONSTRAINT [DF_IncidentBase_FirstResponseSent] DEFAULT ((0)) NULL,
    [cgi_ActionDate]                        DATETIME         NULL,
    [cgi_customer_email]                    NVARCHAR (300)   NULL,
    [cgi_customer_telephonenumber]          NVARCHAR (100)   NULL,
    [cgi_case_category_selected]            NVARCHAR (100)   NULL,
    [cgi_set_anonymous_customer]            NVARCHAR (100)   NULL,
    [cgi_BOMBMobileNumber]                  NVARCHAR (100)   NULL,
    [cgi_casdet_row1_cat1Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row1_cat2Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row1_cat3Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row2_cat1Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row2_cat2Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row2_cat3Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row3_cat2Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row3_cat1Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row3_cat3Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row4_cat1Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row4_cat2Id]                UNIQUEIDENTIFIER NULL,
    [cgi_casdet_row4_cat3Id]                UNIQUEIDENTIFIER NULL,
    [cgi_clearingnr]                        NVARCHAR (100)   NULL,
    [cgi_customer_number]                   NVARCHAR (100)   NULL,
    [cgi_soc_sec_number]                    NVARCHAR (100)   NULL,
    [cgi_track_token]                       NVARCHAR (100)   NULL,
    [cgi_sendtoqueue]                       INT              NULL,
    [cgi_invoiceno]                         NVARCHAR (100)   NULL,
    [cgi_controlfeeno]                      NVARCHAR (100)   NULL,
    [cgi_way_of_transport]                  NVARCHAR (100)   NULL,
    [cgi_line]                              NVARCHAR (100)   NULL,
    [cgi_train]                             NVARCHAR (100)   NULL,
    [cgi_county]                            NVARCHAR (100)   NULL,
    [cgi_Contactid]                         UNIQUEIDENTIFIER NULL,
    [cgi_Customers_Category]                NVARCHAR (100)   NULL,
    [cgi_Customers_SubCategory]             NVARCHAR (100)   NULL,
    [cgi_CaseSolved]                        NVARCHAR (100)   NULL,
    [cgi_emailcount]                        NVARCHAR (100)   NULL,
    [cgi_sFN]                               NVARCHAR (100)   NULL,
    [cgi_sLN]                               NVARCHAR (100)   NULL,
    [cgi_sA]                                NVARCHAR (100)   NULL,
    [cgi_sPA]                               NVARCHAR (100)   NULL,
    [cgi_sPH]                               NVARCHAR (100)   NULL,
    [cgi_sPW]                               NVARCHAR (100)   NULL,
    [cgi_sPM]                               NVARCHAR (100)   NULL,
    [cgi_sPF]                               NVARCHAR (100)   NULL,
    [cgi_sEM]                               NVARCHAR (100)   NULL,
    [cgi_sSSN]                              NVARCHAR (100)   NULL,
    [cgi_sPR]                               NVARCHAR (100)   NULL,
    [cgi_sOP1]                              NVARCHAR (100)   NULL,
    [cgi_sOP2]                              NVARCHAR (100)   NULL,
    [cgi_sTTJ]                              NVARCHAR (100)   NULL,
    [cgi_iBID]                              INT              NULL,
    [cgi_iDC]                               INT              NULL,
    [cgi_TravelCardNo]                      NVARCHAR (100)   NULL,
    [cgi_Representativid]                   UNIQUEIDENTIFIER NULL,
    [cgi_customer_demand]                   NVARCHAR (MAX)   NULL,
    [cgi_arrival_date]                      DATETIME         NULL,
    [cgi_customer_telephonenumber_work]     NVARCHAR (100)   NULL,
    [cgi_customer_telephonenumber_mobile]   NVARCHAR (100)   NULL,
    [cgi_UnregisterdTravelCard]             NVARCHAR (100)   NULL,
    [cgi_case_remittance]                   INT              NULL,
    [cgi_track_token_customer]              NVARCHAR (100)   NULL,
    [cgi_case_reopen]                       NVARCHAR (100)   NULL,
    [cgi_letter_templateId]                 UNIQUEIDENTIFIER NULL,
    [cgi_letter_title]                      NVARCHAR (100)   NULL,
    [cgi_letter_body]                       NVARCHAR (MAX)   NULL,
    [cgi_RGOLIssueId]                       NVARCHAR (50)    NULL,
    [cgi_DepartureDateTime]                 DATETIME         NULL,
    [cgi_full_name]                         NVARCHAR (100)   NULL,
    [cgi_passhasbeenopenedatleastonce]      BIT              NULL,
    [cgi_TravelInformation]                 NVARCHAR (200)   NULL,
    [cgi_TravelInformationLookup]           UNIQUEIDENTIFIER NULL,
    [cgi_TravelInformationCity]             NVARCHAR (100)   NULL,
    [cgi_TravelInformationLine]             NVARCHAR (100)   NULL,
    [cgi_TravelInformationTour]             NVARCHAR (100)   NULL,
    [cgi_TravelInformationCompany]          NVARCHAR (100)   NULL,
    [cgi_TravelInformationStart]            NVARCHAR (100)   NULL,
    [cgi_TravelInformationStop]             NVARCHAR (100)   NULL,
    [cgi_TravelInformationStartPlanned]     DATETIME         NULL,
    [cgi_TravelInformationArrivalPlanned]   DATETIME         NULL,
    [cgi_TravelInformationDirectionText]    NVARCHAR (200)   NULL,
    [cgi_TravelInformationDeviationMessage] NVARCHAR (MAX)   NULL,
    [cgi_TravelInformationArrivalActual]    NVARCHAR (100)   NULL,
    [cgi_TravelInformationStartActual]      NVARCHAR (100)   NULL,
    [cgi_TravelInformationDisplayText]      NVARCHAR (500)   NULL,
    [cgi_TravelInformationTransport]        NVARCHAR (100)   NULL,
    [cgi_RunPriorityWorkflow]               BIT              NULL,
    [cgi_UnderordnaderendenId]              UNIQUEIDENTIFIER NULL,
    [cgi_RefundTypes]                       UNIQUEIDENTIFIER NULL,
    [cgi_RefundMilagekm]                    DECIMAL (23, 10) NULL,
    [cgi_RefundMilageCompensation]          DECIMAL (23, 10) NULL,
    [cgi_RefundQuantity]                    INT              NULL,
    [cgi_RefundCalculatedAmount]            DECIMAL (23, 10) NULL,
    [cgi_RefundAmount]                      MONEY            NULL,
    [cgi_refundamount_Base]                 MONEY            NULL,
    [cgi_RefundReimbursementForm]           UNIQUEIDENTIFIER NULL,
    [cgi_RefundValueCode]                   NVARCHAR (100)   NULL,
    [cgi_RefundTravelCardNo]                NVARCHAR (100)   NULL,
    [cgi_RefundLastValid]                   DATETIME         NULL,
    [cgi_RefundAccountNo]                   NVARCHAR (100)   NULL,
    [cgi_notravelinfo]                      BIT              NULL,
    [cgi_refundtransportcompanyid]          UNIQUEIDENTIFIER NULL,
    [cgi_refundcomments]                    NVARCHAR (MAX)   NULL,
    [cgi_refundcheckno]                     NVARCHAR (100)   NULL,
    [cgi_CirculationNameInPass1]            NVARCHAR (200)   NULL,
    [cgi_CirculationNameInPass2]            NVARCHAR (200)   NULL,
    [cgi_OperatorPass1]                     NVARCHAR (200)   NULL,
    [cgi_OperatorPass2]                     NVARCHAR (200)   NULL,
    [cgi_refundmobilenumber]                NVARCHAR (100)   NULL,
    [cgi_remiss_emailreminders]             INT              NULL,
    [cgi_track_token_remiss_reminder]       NVARCHAR (100)   NULL,
    [cgi_Handelsedatum]                     NVARCHAR (16)    NULL,
    [cgi_experienceddelay]                  NVARCHAR (100)   NULL,
    [cgi_tickettype_1]                      NVARCHAR (100)   NULL,
    [cgi_ticketnumber1]                     NVARCHAR (100)   NULL,
    [cgi_tickettype_2]                      NVARCHAR (100)   NULL,
    [cgi_ticketnumber2]                     NVARCHAR (100)   NULL,
    [cgi_milagefrom]                        NVARCHAR (100)   NULL,
    [cgi_milageto]                          NVARCHAR (100)   NULL,
    [cgi_milagekilometers]                  NVARCHAR (100)   NULL,
    [cgi_milagelicenseplatenumber]          NVARCHAR (100)   NULL,
    [cgi_taxifrom]                          NVARCHAR (100)   NULL,
    [cgi_taxito]                            NVARCHAR (100)   NULL,
    [cgi_compensationclaimfromrgol]         MONEY            NULL,
    [cgi_compensationclaimfromrgol_Base]    MONEY            NULL,
    [cgi_rgolcaselog]                       NVARCHAR (MAX)   NULL,
    [cgi_reqreceipt]                        BIT              NULL,
    [cgi_iscompleted]                       BIT              NULL,
    [cgi_taxiclaimedamount]                 MONEY            NULL,
    [cgi_taxiclaimedamount_Base]            MONEY            NULL,
    [cgi_refundapprovaltype]                NVARCHAR (50)    NULL,
    CONSTRAINT [cndx_PrimaryKey_Incident] PRIMARY KEY CLUSTERED ([IncidentId] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [business_unit_incidents] FOREIGN KEY ([OwningBusinessUnit]) REFERENCES [dbo].[BusinessUnitBase] ([BusinessUnitId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_1_1_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row1_cat1Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_1_2_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row1_cat2Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_1_3_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row1_cat3Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_2_1_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row2_cat1Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_2_2_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row2_cat2Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_2_3_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row2_cat3Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_3_1_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row3_cat1Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_3_2_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row3_cat2Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_3_3_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row3_cat3Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_4_1_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row4_cat1Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_4_2_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row4_cat2Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_4_3_categorydetail_incident] FOREIGN KEY ([cgi_casdet_row4_cat3Id]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_account_incident_Account] FOREIGN KEY ([cgi_Accountid]) REFERENCES [dbo].[AccountBase] ([AccountId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_account_incident_RefundTransportCompanyId] FOREIGN KEY ([cgi_refundtransportcompanyid]) REFERENCES [dbo].[AccountBase] ([AccountId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_callguidechat_incident_Chatid] FOREIGN KEY ([cgi_Chatid]) REFERENCES [dbo].[ActivityPointerBase] ([ActivityId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_callguidefacebook_incident_FacebookPostid] FOREIGN KEY ([cgi_FacebookPostid]) REFERENCES [dbo].[ActivityPointerBase] ([ActivityId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_callguideinfo_incident_CallGuideInfo] FOREIGN KEY ([cgi_CallGuideInfo]) REFERENCES [dbo].[cgi_callguideinfoBase] ([cgi_callguideinfoId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_categorydetail_incident_OriginalCallguideCategory] FOREIGN KEY ([cgi_OriginalCallguideCategory]) REFERENCES [dbo].[cgi_categorydetailBase] ([cgi_categorydetailId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_refundtype_incident_RefundTypes] FOREIGN KEY ([cgi_RefundTypes]) REFERENCES [dbo].[cgi_refundtypeBase] ([cgi_refundtypeId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_reimbursementform_incident_RefundReimbursementForm] FOREIGN KEY ([cgi_RefundReimbursementForm]) REFERENCES [dbo].[cgi_reimbursementformBase] ([cgi_reimbursementformId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_representative_incident_Representativid] FOREIGN KEY ([cgi_Representativid]) REFERENCES [dbo].[cgi_representativeBase] ([cgi_representativeId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_travelcard_incident_TravelCardid] FOREIGN KEY ([cgi_TravelCardid]) REFERENCES [dbo].[cgi_travelcardBase] ([cgi_travelcardId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_cgi_travelinformation_incident_TravelInformation] FOREIGN KEY ([cgi_TravelInformationLookup]) REFERENCES [dbo].[cgi_travelinformationBase] ([cgi_travelinformationId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_contact_incident_Contactid] FOREIGN KEY ([cgi_Contactid]) REFERENCES [dbo].[ContactBase] ([ContactId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_contact_incident_ThirdpartyNameid] FOREIGN KEY ([cgi_ThirdpartyNameid]) REFERENCES [dbo].[ContactBase] ([ContactId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_incident_incident] FOREIGN KEY ([cgi_UnderordnaderendenId]) REFERENCES [dbo].[IncidentBase] ([IncidentId]) NOT FOR REPLICATION,
    CONSTRAINT [cgi_letter_template_incident] FOREIGN KEY ([cgi_letter_templateId]) REFERENCES [dbo].[cgi_letter_templateBase] ([cgi_letter_templateId]) NOT FOR REPLICATION,
    CONSTRAINT [contact_as_primary_contact] FOREIGN KEY ([PrimaryContactId]) REFERENCES [dbo].[ContactBase] ([ContactId]) NOT FOR REPLICATION,
    CONSTRAINT [contact_as_responsible_contact] FOREIGN KEY ([ResponsibleContactId]) REFERENCES [dbo].[ContactBase] ([ContactId]) NOT FOR REPLICATION,
    CONSTRAINT [contract_cases] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[ContractBase] ([ContractId]) NOT FOR REPLICATION,
    CONSTRAINT [contract_detail_cases] FOREIGN KEY ([ContractDetailId]) REFERENCES [dbo].[ContractDetailBase] ([ContractDetailId]) NOT FOR REPLICATION,
    CONSTRAINT [entitlement_cases] FOREIGN KEY ([EntitlementId]) REFERENCES [dbo].[EntitlementBase] ([EntitlementId]) NOT FOR REPLICATION,
    CONSTRAINT [kbarticle_incidents] FOREIGN KEY ([KbArticleId]) REFERENCES [dbo].[KbArticleBase] ([KbArticleId]) NOT FOR REPLICATION,
    CONSTRAINT [owner_incidents] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[OwnerBase] ([OwnerId]) NOT FOR REPLICATION,
    CONSTRAINT [product_incidents] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProductBase] ([ProductId]) NOT FOR REPLICATION,
    CONSTRAINT [socialprofile_cases] FOREIGN KEY ([SocialProfileId]) REFERENCES [dbo].[SocialProfileBase] ([SocialProfileId]) NOT FOR REPLICATION,
    CONSTRAINT [subject_incidents] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[SubjectBase] ([SubjectId]) NOT FOR REPLICATION,
    CONSTRAINT [TransactionCurrency_Incident] FOREIGN KEY ([TransactionCurrencyId]) REFERENCES [dbo].[TransactionCurrencyBase] ([TransactionCurrencyId]) NOT FOR REPLICATION,
    CONSTRAINT [AK1_IncidentBase] UNIQUE NONCLUSTERED ([OwningBusinessUnit] ASC, [TicketNumber] ASC)
);


GO
EXECUTE sp_tableoption @TableNamePattern = N'[dbo].[IncidentBase]', @OptionName = N'text in row', @OptionValue = N'7000';


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_contract_detail_cases]
    ON [dbo].[IncidentBase]([ContractDetailId] ASC) WHERE ([ContractDetailId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_TicketNumber]
    ON [dbo].[IncidentBase]([TicketNumber] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_for_cascaderelationship_incident_customer_accounts]
    ON [dbo].[IncidentBase]([CustomerId] ASC, [CustomerIdType] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_subject_incidents]
    ON [dbo].[IncidentBase]([SubjectId] ASC) WHERE ([SubjectId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_Title]
    ON [dbo].[IncidentBase]([Title] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_contract_cases]
    ON [dbo].[IncidentBase]([ContractId] ASC) WHERE ([ContractId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_product_incidents]
    ON [dbo].[IncidentBase]([ProductId] ASC) WHERE ([ProductId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_kbarticle_incidents]
    ON [dbo].[IncidentBase]([KbArticleId] ASC) WHERE ([KbArticleId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Core]
    ON [dbo].[IncidentBase]([StateCode] ASC, [StatusCode] ASC)
    INCLUDE([CaseTypeCode], [CaseOriginCode]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_report]
    ON [dbo].[IncidentBase]([StateCode] ASC)
    INCLUDE([OwningBusinessUnit], [ModifiedOn], [OwnerId]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [ndx_Security]
    ON [dbo].[IncidentBase]([OwnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_contact_as_responsible_contact]
    ON [dbo].[IncidentBase]([ResponsibleContactId] ASC) WHERE ([ResponsibleContactId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE UNIQUE NONCLUSTERED INDEX [fndx_Sync_VersionNumber]
    ON [dbo].[IncidentBase]([VersionNumber] ASC) WHERE ([VersionNumber] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_entitlement_cases]
    ON [dbo].[IncidentBase]([EntitlementId] ASC) WHERE ([EntitlementId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_case_parent_case]
    ON [dbo].[IncidentBase]([ParentCaseId] ASC) WHERE ([ParentCaseId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cascaderelationship_case_master_case]
    ON [dbo].[IncidentBase]([MasterId] ASC) WHERE ([MasterId] IS NOT NULL) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [fndx_for_cases_origin_per_day]
    ON [dbo].[IncidentBase]([CreatedOn] ASC, [CaseOriginCode] ASC) WITH (FILLFACTOR = 80);

