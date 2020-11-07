using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.WorkFlow
{
    public interface IWorkFlowBusinessProcess : IBusinessProcess
    {
        TResponse Raise<TResponse>(Event @event) where TResponse: Response, new();
    }
}
