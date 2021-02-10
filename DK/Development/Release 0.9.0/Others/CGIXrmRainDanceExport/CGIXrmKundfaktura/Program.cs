using CGIXrmRainDanceExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmKundfaktura
{
    class Program
    {
        static void Main(string[] args)
        {
            RunBatch_Kundfaktura _runKundfaktura = new RunBatch_Kundfaktura();
            _runKundfaktura.Run();
        }
    }
}
