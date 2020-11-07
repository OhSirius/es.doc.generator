using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process
{
    public interface IBusinessProcess
    {
    }

    public class ProcessEventsArgs : EventArgs
    {
        public int Percent { set; get; }

        public string Display { set; get; }
    }


    public interface IProcess : IBusinessProcess
    {
        event Action<ProcessEventsArgs> Processing;

        Task<bool> Execute(CancellationToken token);
    }
}
