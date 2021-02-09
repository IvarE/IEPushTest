using System;

namespace CGIXrmRainDanceExport
{
    class Program
    {
        #region Main
        static void Main(string[] args)
        {
            try
            {
                RunBatch run = new RunBatch();
                run.Run();

                RunBatch_UTLAND run2 = new RunBatch_UTLAND();
                run2.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
