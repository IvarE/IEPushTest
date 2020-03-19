using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.DeltabatchService
{
    class Set<T> : HashSet<T>, Quartz.Collection.ISet<T>
    {
    }
}
