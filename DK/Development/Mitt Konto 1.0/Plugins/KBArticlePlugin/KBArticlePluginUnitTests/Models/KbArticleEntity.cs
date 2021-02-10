using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using CGIXrmWin;

namespace CGI.CRM2013.Skanetrafiken.KBArticlePluginUnitTests.Models
{
    [XrmEntity("kbarticle")]
    internal class KbArticleEntity
    {
        [Xrm("cgi_applicationname")]
        public string ApplicationName { get; set; }
    }
}
