using BG.Infrastructure.Process.Process.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Process.WorkFlow.Descriptions
{
    public class ObjectAssignProcess
    {
        public const string aName = "ObjectAssign";

        public class Events
        {
            public const string aAssigned = "Assigned";

        }
    }

    public class ObjectAssignProcessParam : ResponseParam
    {
    }

    public class ObjectAssignEventParam : EventParam
    {
        public object Object { set; get; }

    }

}
