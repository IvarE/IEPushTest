using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmKBArticleRepublish
{
    class Program
    {
        static void Main(string[] args)
        {
            KBArticleRepublisher republisher = new KBArticleRepublisher();
            republisher.Run();
            Console.WriteLine("PRESS ENTER TO CONTINUE!");
            Console.ReadLine();
        }
    }
}
