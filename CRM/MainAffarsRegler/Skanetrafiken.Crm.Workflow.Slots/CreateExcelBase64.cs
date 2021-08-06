using System;
using System.Data;
using System.Activities;
using System.Data.SqlClient;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;
using System.IO;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
using Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Workflow.Slots
{
    /// <summary>
    /// This custom activity is not deployed on SandBox because it requires the DocumentFormat.OpenXml dll which is external
    /// This dll is required to create an excel file from a memory stream
    /// </summary>
    public class CreateExcelBase64 : CodeActivity
    {
        [Input("FromDate")]
        [RequiredArgument()]
        public InArgument<DateTime> FromDate { get; set; }

        [Input("ToDate")]
        [RequiredArgument()]
        public InArgument<DateTime> ToDate { get; set; }

        [Output("ExcelBase64")]
        public OutArgument<string> ExcelBase64 { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();

            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {
            //GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.Trace($"CreateExcelBase64 started.");

            //GET VALUE(S)
            DateTime fromDate = FromDate.Get(activityContext);
            DateTime toDate = ToDate.Get(activityContext);

            //TRY EXECUTE
            try
            {
                string response = ExecuteCodeActivity(localContext, fromDate, toDate);
                ExcelBase64.Set(activityContext, response);
            }
            catch (Exception ex)
            {
                ExcelBase64.Set(activityContext, ex.Message);
            }

            localContext.Trace($"CreateExcelBase64 finished.");

        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, DateTime fromDate, DateTime toDate)
        {
            if (fromDate == null || toDate == null)
                Console.WriteLine($"Either FromDate or ToDate is null.");

            ColumnSet columnsSlots = new ColumnSet(ed_Slots.Fields.ed_SlotIdentifier, ed_Slots.Fields.ed_name, ed_Slots.Fields.CreatedOn,
                    ed_Slots.Fields.ed_BookingDay, ed_Slots.Fields.ed_StandardPrice, ed_Slots.Fields.ed_CustomPrice, ed_Slots.Fields.ed_BookingStatus,
                    ed_Slots.Fields.ed_Extended, ed_Slots.Fields.ed_Opportunity);

            QueryExpression querySlots = new QueryExpression(ed_Slots.EntityLogicalName);
            querySlots.NoLock = true;
            querySlots.ColumnSet = columnsSlots;
            querySlots.Criteria.AddCondition(ed_Slots.Fields.ed_BookingDay, ConditionOperator.OnOrAfter, fromDate);
            querySlots.Criteria.AddCondition(ed_Slots.Fields.ed_BookingDay, ConditionOperator.OnOrBefore, toDate);

            LinkEntity linkAccount = querySlots.AddLink(Account.EntityLogicalName, ed_Slots.Fields.ed_Account, Account.Fields.AccountId, JoinOperator.LeftOuter);
            linkAccount.EntityAlias = "aa";
            linkAccount.Columns.AddColumns(Account.Fields.Address1_City, Account.Fields.AccountNumber, Account.Fields.Name);

            LinkEntity linkSystemUser = querySlots.AddLink(SystemUserEntity.EntityLogicalName, ed_Slots.Fields.OwningUser, SystemUserEntity.Fields.SystemUserId, JoinOperator.LeftOuter);
            linkSystemUser.EntityAlias = "au";
            linkSystemUser.Columns.AddColumns(SystemUserEntity.Fields.ed_RSID, SystemUserEntity.Fields.FullName);

            LinkEntity linkOpportunity = querySlots.AddLink(OpportunityEntity.EntityLogicalName, ed_Slots.Fields.ed_Opportunity, OpportunityEntity.Fields.OpportunityId, JoinOperator.LeftOuter);
            linkOpportunity.EntityAlias = "ao";
            linkOpportunity.Columns.AddColumns(OpportunityEntity.Fields.DiscountAmount, OpportunityEntity.Fields.DiscountPercentage, OpportunityEntity.Fields.ed_campaigndatestart, OpportunityEntity.Fields.ed_campaigndateend);

            LinkEntity linkProduct = querySlots.AddLink(Product.EntityLogicalName, ed_Slots.Fields.ed_ProductID, Product.Fields.ProductId, JoinOperator.LeftOuter);
            linkProduct.EntityAlias = "ap";
            linkProduct.Columns.AddColumns(Product.Fields.ProductNumber, Product.Fields.Name);

            List<ed_Slots> lSlots = XrmRetrieveHelper.RetrieveMultiple<ed_Slots>(localContext, querySlots);

            if (lSlots.Count == 0)
                return string.Empty;

            List<ExcelOpportunityProduct> excelOppProd = new List<ExcelOpportunityProduct>();

            for (int i = 0; i < lSlots.Count; i++)
            {
                ed_Slots slot = lSlots[i];
                EntityReference erOpportunity = slot.ed_Opportunity;

                if (erOpportunity == null)
                    continue;

                List<OpportunityProductEntity> lOpportunityProducts = new List<OpportunityProductEntity>();
                var checkOpp = excelOppProd.FirstOrDefault(x => x.opportunity == erOpportunity.Id);
                if (checkOpp == null)
                {
                    QueryExpression queryOpportunityProduct = new QueryExpression(OpportunityProductEntity.EntityLogicalName);
                    queryOpportunityProduct.NoLock = true;
                    queryOpportunityProduct.ColumnSet = new ColumnSet(OpportunityProductEntity.Fields.ManualDiscountAmount, OpportunityProductEntity.Fields.VolumeDiscountAmount);
                    queryOpportunityProduct.Criteria.AddCondition(OpportunityProductEntity.Fields.OpportunityId, ConditionOperator.Equal, erOpportunity.Id);

                    lOpportunityProducts = XrmRetrieveHelper.RetrieveMultiple<OpportunityProductEntity>(localContext, queryOpportunityProduct);
                    ExcelOpportunityProduct excelOpp = new ExcelOpportunityProduct();
                    excelOpp.opportunity = erOpportunity.Id;
                    excelOpp.lOppProducts = lOpportunityProducts;
                    excelOppProd.Add(excelOpp);
                }
                else
                    lOpportunityProducts = checkOpp.lOppProducts;

                Money manualDiscount = new Money(0);
                Money volumeDiscount = new Money(0);
                foreach (OpportunityProductEntity oppProd in lOpportunityProducts)
                {
                    if (oppProd.ManualDiscountAmount != null)
                        manualDiscount.Value += oppProd.ManualDiscountAmount.Value;

                    if (oppProd.VolumeDiscountAmount != null)
                        volumeDiscount.Value += oppProd.VolumeDiscountAmount.Value;
                }

                lSlots[i].Attributes.Add("manualdiscountamount", manualDiscount.Value);
                lSlots[i].Attributes.Add("volumediscountamount", volumeDiscount.Value);
            }

            return CreateExcelFile(lSlots);
        }

        public static string CreateExcelFile(List<ed_Slots> lSlots)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(mem, SpreadsheetDocumentType.Workbook);

                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Active Slots"
                };
                sheets.Append(sheet);

                SheetData sheetData1 = new SheetData();
                sheetData1.Append(CreateHeaderRowForExcel());

                foreach (ed_Slots slot in lSlots)
                {
                    Row partsRows = GenerateRowForChildPartDetail(slot);
                    sheetData1.Append(partsRows);
                }

                worksheetPart.Worksheet = new Worksheet(sheetData1);

                workbookpart.Workbook.Save();
                spreadsheetDocument.Save();
                spreadsheetDocument.Close();

                //reset the position to the start of the stream
                //mem.Seek(0, SeekOrigin.Begin);
                byte[] byteArray = mem.ToArray();

                return Convert.ToBase64String(byteArray);
            }
        }

        private static EnumValue<CellValues> ResolveCellDataTypeOnValue(string text)
        {
            int intVal;
            double doubleVal;
            if (int.TryParse(text, out intVal) || double.TryParse(text, out doubleVal))
            {
                return CellValues.Number;
            }
            else
            {
                return CellValues.String;
            }
        }

        private static Cell CreateCell(string text, uint styleIndex)
        {
            Cell cell = new Cell();
            cell.StyleIndex = styleIndex;
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new CellValue(text);

            return cell;
        }

        private static Cell CreateCell(string text)
        {
            Cell cell = new Cell();
            cell.StyleIndex = 1U;
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new CellValue(text);

            return cell;
        }

        private static Row GenerateRowForChildPartDetail(ed_Slots slot)
        {
            Row tRow = new Row();

            string slotidentifier = slot.ed_SlotIdentifier == null ? string.Empty : slot.ed_SlotIdentifier;
            tRow.Append(CreateCell(slotidentifier));

            string sName = slot.ed_name == null ? string.Empty : slot.ed_name;
            tRow.Append(CreateCell(sName));

            string createdon = slot.CreatedOn == null ? string.Empty : slot.CreatedOn.Value.ToString();
            tRow.Append(CreateCell(createdon));

            string bookingday = slot.ed_BookingDay == null ? string.Empty : slot.ed_BookingDay.Value.ToString();
            tRow.Append(CreateCell(bookingday));

            string standardprice = slot.ed_StandardPrice == null ? string.Empty : slot.ed_StandardPrice.Value.ToString();
            tRow.Append(CreateCell(standardprice));

            string customprice = slot.ed_CustomPrice == null ? string.Empty : slot.ed_CustomPrice.Value.ToString();
            tRow.Append(CreateCell(customprice));

            string bookingstatus = slot.ed_BookingStatus == null ? string.Empty : slot.ed_BookingStatus.Value.ToString();
            tRow.Append(CreateCell(bookingstatus));

            string extended = slot.ed_Extended == null ? string.Empty : slot.ed_Extended.Value.ToString();
            tRow.Append(CreateCell(extended));

            string manualdiscount = slot.Contains("manualdiscountamount") ? slot.Attributes["manualdiscountamount"].ToString() : string.Empty;
            tRow.Append(CreateCell(manualdiscount));

            string volumediscount = slot.Contains("volumediscountamount") ? slot.Attributes["volumediscountamount"].ToString() : string.Empty;
            tRow.Append(CreateCell(volumediscount));

            var aliasedRsId = slot.GetAttributeValue<AliasedValue>("au.ed_rsid");
            string rsid = aliasedRsId == null ? string.Empty : aliasedRsId.Value.ToString();
            tRow.Append(CreateCell(rsid));

            var aliasedFullName = slot.GetAttributeValue<AliasedValue>("au.fullname");
            string fullname = aliasedFullName == null ? string.Empty : aliasedFullName.Value.ToString();
            tRow.Append(CreateCell(fullname));

            var aliasedANumber = slot.GetAttributeValue<AliasedValue>("aa.accountnumber");
            string accountnumber = aliasedANumber == null ? string.Empty : aliasedANumber.Value.ToString();
            tRow.Append(CreateCell(accountnumber));

            var aliasedName = slot.GetAttributeValue<AliasedValue>("aa.name");
            string name = aliasedName == null ? string.Empty : aliasedName.Value.ToString();
            tRow.Append(CreateCell(name));

            var aliasedACity = slot.GetAttributeValue<AliasedValue>("aa.address1_city");
            string addresscity = aliasedACity == null ? string.Empty : aliasedACity.Value.ToString();
            tRow.Append(CreateCell(addresscity));

            var aliasedCampaignStart = slot.GetAttributeValue<AliasedValue>("ao.ed_campaigndatestart");
            string campaignstart = aliasedCampaignStart == null ? string.Empty : ((DateTime)aliasedCampaignStart.Value).ToString();
            tRow.Append(CreateCell(campaignstart));

            var aliasedCampaignEnd = slot.GetAttributeValue<AliasedValue>("ao.ed_campaigndateend");
            string campaignend = aliasedCampaignEnd == null ? string.Empty : ((DateTime)aliasedCampaignEnd.Value).ToString();
            tRow.Append(CreateCell(campaignend));

            var aliasedDiscountA = slot.GetAttributeValue<AliasedValue>("ao.discountamount");
            string discountamount = aliasedDiscountA == null ? string.Empty : (((Money)aliasedDiscountA.Value).Value).ToString();
            tRow.Append(CreateCell(discountamount));

            var aliasedDiscountP = slot.GetAttributeValue<AliasedValue>("ao.discountpercentage");
            string discountpercentage = aliasedDiscountP == null ? string.Empty : ((decimal)aliasedDiscountP.Value).ToString();
            tRow.Append(CreateCell(discountpercentage));

            var aliasedPNumber = slot.GetAttributeValue<AliasedValue>("ap.productnumber");
            string productnumber = aliasedPNumber == null ? string.Empty : aliasedPNumber.Value.ToString();
            tRow.Append(CreateCell(productnumber));

            var aliasedPName = slot.GetAttributeValue<AliasedValue>("ap.name");
            string pName = aliasedPName == null ? string.Empty : aliasedPName.Value.ToString();
            tRow.Append(CreateCell(pName));

            return tRow;
        }

        private static Row CreateHeaderRowForExcel()
        {
            Row workRow = new Row();
            workRow.Append(CreateCell("Slott_id", 2U));
            workRow.Append(CreateCell("Slott_namn", 2U));
            workRow.Append(CreateCell("Slott_datum", 2U));
            workRow.Append(CreateCell("Slott_bokningsdatum", 2U));
            workRow.Append(CreateCell("Slott_standardpris", 2U));
            workRow.Append(CreateCell("Slott_custompris", 2U));
            workRow.Append(CreateCell("Slott_status", 2U));
            workRow.Append(CreateCell("Slott_extended", 2U));

            workRow.Append(CreateCell("Produkt_id", 2U));
            workRow.Append(CreateCell("Produkt_namn", 2U));

            workRow.Append(CreateCell("Produkt_rabatt(SEK)", 2U));
            workRow.Append(CreateCell("Volymn_rabatt(SEK)", 2U));

            workRow.Append(CreateCell("Offert_rabatt(%)", 2U));
            workRow.Append(CreateCell("Offert_rabatt(SEK)", 2U));
            workRow.Append(CreateCell("Kampanjdatum_slut", 2U));
            workRow.Append(CreateCell("Kampanjdatum_start", 2U));

            workRow.Append(CreateCell("Kund_id", 2U));
            workRow.Append(CreateCell("Kund_namn", 2U));
            workRow.Append(CreateCell("Kund_postadress_stad", 2U));

            workRow.Append(CreateCell("Säljare_id", 2U));
            workRow.Append(CreateCell("Säljare_namn", 2U));

            return workRow;
        }
    }

    public class ExcelOpportunityProduct
    {
        public Guid opportunity { get; set; }
        public List<OpportunityProductEntity> lOppProducts { get; set; }
    }
}
