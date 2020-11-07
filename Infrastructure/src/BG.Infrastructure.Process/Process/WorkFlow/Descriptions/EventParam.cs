using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.WorkFlow
{
    public enum ResponseType
    {
        Unknown,
        Processed
    }

    public class ResponseParam
    {

    }

    public class Response
    {
        public ResponseType Type { set; get; }

        public int ProcessId { set; get; }

        public ResponseParam Param { set; get; }
    }

    public class EventParam
    {

    }

}
