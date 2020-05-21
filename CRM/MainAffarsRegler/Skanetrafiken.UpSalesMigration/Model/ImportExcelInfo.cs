using System.Collections.Generic;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace Skanetrafiken.UpSalesMigration.Model
{
    public class ImportExcelInfo
    {
        public int? rowCount { get; set; }
        public int? colCount { get; set; }
        public ExcelApp.Range excelRange { get; set; }
        public List<ExcelColumn> lColumns { get; set; }

        public ImportExcelInfo(int? r, int? c, ExcelApp.Range range, List<ExcelColumn> lC)
        {
            rowCount = r;
            colCount = c;
            excelRange = range;
            lColumns = lC;
        }
    }
}
