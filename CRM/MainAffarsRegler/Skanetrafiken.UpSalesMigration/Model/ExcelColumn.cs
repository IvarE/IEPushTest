
namespace Skanetrafiken.UpSalesMigration.Model
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
}
