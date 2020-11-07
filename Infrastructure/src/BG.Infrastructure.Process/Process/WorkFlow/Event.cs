using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.WorkFlow
{
    public class Event
    {
        public string Name { set; get; }

        public Context Source { set; get; }

        public WorkFlowPattern Pattern { set; get; }

        public EventParam Param { set; get; }

        public int? ProcessId { set; get; }

    }

}
