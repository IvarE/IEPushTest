using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace Toolbelt.Classes
{
    public class ApplicationPools : List<ApplicationPool>
    {

    }

    
    public class ApplicationPool
    {
        public string Name { get; set; }
    }


    public class Sites : List<Toolbelt.Classes.Site>
    {

    }

    public class Site
    {
        public string Name { get; set; }
    }
     
}
