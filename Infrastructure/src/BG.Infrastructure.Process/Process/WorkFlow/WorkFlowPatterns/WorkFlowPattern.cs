using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.WorkFlow
{

    public enum WorkFlowPatternType
    {
        Unknown,
        Parallel,
        Wait
    }

    public class WorkFlowPattern
    {
        public WorkFlowPatternType Type { set; get; }

        public string Name { set; get; }
    }
}
