using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.WorkFlow
{
    public class ProcessInfo
    {
        public int ProcessId { set; get; }

        public string ProcessName { set; get; }
    }

    public interface IWorkFlowBusinessProcessProvider
    {
        IEnumerable<ProcessInfo> GetActiveProcesses();
    }
}
