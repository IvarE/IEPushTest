using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xrm.Sdk;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm
{
    public class SlotsUtility
    {
        public static string CreateExcelFile(List<SlotsEntity> lSlots)
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

                foreach (SlotsEntity slot in lSlots)
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

        private static Row GenerateRowForChildPartDetail(SlotsEntity slot)
        {
            Row tRow = new Row();

            string slotidentifier = slot.ed_SlotIdentifier == null ? string.Empty : slot.ed_SlotIdentifier;
            tRow.Append(CreateCell(slotidentifier));

            string sName = slot.ed_name == null ? string.Empty : slot.ed_name;
            tRow.Append(CreateCell(sName));

            string createdon = slot.CreatedOn == null ? string.Empty : slot.CreatedOn.Value.ToLocalTime().ToString();
            tRow.Append(CreateCell(createdon));

            string bookingday = slot.ed_BookingDay == null ? string.Empty : slot.ed_BookingDay.Value.ToLocalTime().ToString();
            tRow.Append(CreateCell(bookingday));

            string standardprice = slot.ed_StandardPrice == null ? string.Empty : slot.ed_StandardPrice.Value.ToString();
            tRow.Append(CreateCell(standardprice));

            string customprice = slot.ed_CustomPrice == null ? string.Empty : slot.ed_CustomPrice.Value.ToString();
            tRow.Append(CreateCell(customprice));

            string bookingstatus = slot.ed_BookingStatus == null ? string.Empty : slot.ed_BookingStatus.Value.ToString();
            tRow.Append(CreateCell(bookingstatus));

            string extended = slot.ed_Extended == null ? string.Empty : slot.ed_Extended.Value.ToString();
            tRow.Append(CreateCell(extended));

            var aliasedPNumber = slot.GetAttributeValue<AliasedValue>("ap.productnumber");
            string productnumber = aliasedPNumber == null ? string.Empty : aliasedPNumber.Value.ToString();
            tRow.Append(CreateCell(productnumber));

            var aliasedPName = slot.GetAttributeValue<AliasedValue>("ap.name");
            string pName = aliasedPName == null ? string.Empty : aliasedPName.Value.ToString();
            tRow.Append(CreateCell(pName));

            var aliasedManualDiscount = slot.GetAttributeValue<AliasedValue>("aqp.manualdiscountamount");
            string manualdiscount = aliasedManualDiscount == null ? string.Empty : ((Money)aliasedManualDiscount.Value).Value.ToString();
            tRow.Append(CreateCell(manualdiscount));

            var aliasedVolumeDiscount = slot.GetAttributeValue<AliasedValue>("aqp.volumediscountamount");
            string volumediscount = aliasedVolumeDiscount == null ? string.Empty : ((Money)aliasedVolumeDiscount.Value).Value.ToString();
            tRow.Append(CreateCell(volumediscount));

            var aliasedCampaignStart = slot.GetAttributeValue<AliasedValue>("aq.ed_campaigndatestart");
            string campaignstart = aliasedCampaignStart == null ? string.Empty : ((DateTime)aliasedCampaignStart.Value).ToLocalTime().ToShortDateString();
            tRow.Append(CreateCell(campaignstart));

            var aliasedCampaignEnd = slot.GetAttributeValue<AliasedValue>("aq.ed_campaigndateend");
            string campaignend = aliasedCampaignEnd == null ? string.Empty : ((DateTime)aliasedCampaignEnd.Value).ToLocalTime().ToShortDateString();
            tRow.Append(CreateCell(campaignend));

            var aliasedDiscountA = slot.GetAttributeValue<AliasedValue>("aq.discountamount");
            string discountamount = aliasedDiscountA == null ? string.Empty : (((Money)aliasedDiscountA.Value).Value).ToString();
            tRow.Append(CreateCell(discountamount));

            var aliasedDiscountP = slot.GetAttributeValue<AliasedValue>("aq.discountpercentage");
            string discountpercentage = aliasedDiscountP == null ? string.Empty : ((decimal)aliasedDiscountP.Value).ToString();
            tRow.Append(CreateCell(discountpercentage));

            var aliasedANumber = slot.GetAttributeValue<AliasedValue>("aa.accountnumber");
            string accountnumber = aliasedANumber == null ? string.Empty : aliasedANumber.Value.ToString();
            tRow.Append(CreateCell(accountnumber));

            var aliasedName = slot.GetAttributeValue<AliasedValue>("aa.name");
            string name = aliasedName == null ? string.Empty : aliasedName.Value.ToString();
            tRow.Append(CreateCell(name));

            var aliasedACity = slot.GetAttributeValue<AliasedValue>("aa.address1_city");
            string addresscity = aliasedACity == null ? string.Empty : aliasedACity.Value.ToString();
            tRow.Append(CreateCell(addresscity));

            var aliasedRsId = slot.GetAttributeValue<AliasedValue>("au.ed_rsid");
            string rsid = aliasedRsId == null ? string.Empty : aliasedRsId.Value.ToString();
            tRow.Append(CreateCell(rsid));

            var aliasedFullName = slot.GetAttributeValue<AliasedValue>("au.fullname");
            string fullname = aliasedFullName == null ? string.Empty : aliasedFullName.Value.ToString();
            tRow.Append(CreateCell(fullname));

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

            workRow.Append(CreateCell("Kampanjdatum_start", 2U));
            workRow.Append(CreateCell("Kampanjdatum_slut", 2U));
            workRow.Append(CreateCell("Offert_rabatt(SEK)", 2U));
            workRow.Append(CreateCell("Offert_rabatt(%)", 2U));

            workRow.Append(CreateCell("Kund_id", 2U));
            workRow.Append(CreateCell("Kund_namn", 2U));
            workRow.Append(CreateCell("Kund_postadress_stad", 2U));

            workRow.Append(CreateCell("Säljare_id", 2U));
            workRow.Append(CreateCell("Säljare_namn", 2U));

            return workRow;
        }
    }
}