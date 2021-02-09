using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using CGIXrmWin;

namespace CRM2013.SkanetrafikenPlugins.Models
{
    [XrmEntity("systemuser")]
    internal class UserEntity
    {
        [Xrm("fullname")]
        public string FullName { get; set; }

        [Xrm("domainname")]
        public string Username { get; set; }

        [Xrm("systemuserid")]
        public Guid Id { get; set; }

        [Xrm("isdisabled")]
        public bool Status { get; set; }
    }
}
