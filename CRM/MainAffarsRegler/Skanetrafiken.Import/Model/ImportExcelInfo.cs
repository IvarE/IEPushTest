using System;
using System.Collections.Generic;

namespace Skanetrafiken.Import.Model
{
    public class ImportExcelInfo : IDisposable
    {
        public List<ExcelColumn> lColumns { get; set; }
        public List<List<ExcelLineData>> lData { get; set; }

        public ImportExcelInfo()
        {
            lColumns = new List<ExcelColumn>();
            lData = new List<List<ExcelLineData>>();
        }

        public ImportExcelInfo(List<ExcelColumn> lC, List<List<ExcelLineData>> lD)
        {
            lColumns = lC;
            lData = lD;
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            lColumns = new List<ExcelColumn>();
            lData = new List<List<ExcelLineData>>();

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}
