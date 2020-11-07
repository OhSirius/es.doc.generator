using BG.Infrastructure.Common.MEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process
{
    public class Context
    {
        public string EntityName { set; get; }

        public EntityType EntityType { set; get; }

        public string ProcessName { set; get; }

        public ProcessType ProcessType { set; get; }



        public string Module { set; get; }
    }

    public interface IBusinessProcessContext
    {
        Context Context { get; }
    }
}
