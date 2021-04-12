using System.Collections.Generic;

namespace Skanetrafiken.UpSalesMigration.Model
{
    public class ImportExcelInfo
    {
        public List<ExcelColumn> lColumns { get; set; }
        public List<List<ExcelLineData>> lData { get; set; }

        public ImportExcelInfo(List<ExcelColumn> lC, List<List<ExcelLineData>> lD)
        {
            lColumns = lC;
            lData = lD;
        }
    }
}
