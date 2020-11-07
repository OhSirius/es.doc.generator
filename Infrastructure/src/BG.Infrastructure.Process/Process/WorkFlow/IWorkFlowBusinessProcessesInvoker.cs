using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.WorkFlow
{
    public interface IWorkFlowBusinessProcessesInvoker
    {
        IEnumerable<Response> Invoke(Event @event);
    }
}
