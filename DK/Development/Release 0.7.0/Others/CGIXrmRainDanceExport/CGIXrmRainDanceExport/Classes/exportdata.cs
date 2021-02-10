using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmRainDanceExport
{
    public class ExportData
    {
        private int _counter;
        public int Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        private string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
