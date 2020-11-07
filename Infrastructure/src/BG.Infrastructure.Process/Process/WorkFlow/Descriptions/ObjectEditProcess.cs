using BG.Infrastructure.Process.Process.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Process.WorkFlow.Descriptions
{
    public class ObjectEditProcessProcess
    {
        public const string aName = "ObjectEdit";

        public class Events
        {
            public const string aValidating = "Validating";

        }
    }

    public class ObjectEditProcessParam : ResponseParam
    {
        public bool IsValid { set; get; }

        public string ErrorMessage { set; get; }
    }

    public class ObjectEditEventParam : EventParam
    {
        public object Object { set; get; }

        public string ParentName { set; get; }

        public object ParentId { set; get; }
    }

}
