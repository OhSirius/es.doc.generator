using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Extensions;

namespace BG.Infrastructure.Process.Process.WorkFlow.Impl
{
    public class WorkFlowBusinessProcessProvider : IWorkFlowBusinessProcessProvider
    {
        public IEnumerable<ProcessInfo> GetActiveProcesses()
        {
            throw new NotImplementedException();
            //var query = new Query(CustomObjectProcesses.EntryName);
            //query.Attributes.Add(CustomObjectProcesses.aID);
            //query.Attributes.Add(CustomObjectProcesses.aType);
            //query.Condition.Add(Criteria.Operator(CustomObjectProcesses.aState, CustomObjectProcesses.iState_запущен));
            //var list = query.Run();
            //if (list.IsNullOrEmpty())
            //    return null;

            //var info = list.Cast<CustomObjectProcesses>().Select(l => new ProcessInfo() { ProcessId = l.ID, ProcessName = l.Type }).ToArray();
            //return info;
        }
    }
}
