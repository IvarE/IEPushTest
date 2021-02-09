using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CGIXrmRainDanceExport
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try
            {
                RunBatch _run = new RunBatch();
                _run.Run();

                RunBatch_UTLAND _run2 = new RunBatch_UTLAND();
                _run2.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        

        

    }
}
