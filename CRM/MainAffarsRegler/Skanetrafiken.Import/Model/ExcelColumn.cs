
using System.Collections.Generic;

namespace Skanetrafiken.Import.Model
{
    public class ExcelColumn
    {
        public int index { get; set; }
        public string name { get; set; }

        public ExcelColumn(int i, string n)
        {
            index = i;
            name = n;
        }
    }

    public class ExcelLineData
    {
        public int index { get; set; }

        public string value { get; set; }

        public ExcelLineData(int i, string v)
        {
            index = i;
            value = v;
        }
    }
}
