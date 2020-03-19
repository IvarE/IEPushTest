using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmRainDanceImport
{
    class Program
    {
        static void Main(string[] args)
        {
            RunBatch _run = new RunBatch();
            bool _ok = _run.Run();
        }
    }
}
