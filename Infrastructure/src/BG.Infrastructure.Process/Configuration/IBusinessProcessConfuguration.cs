using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BG.Infrastructure.Process.Process;
using BG.Infrastructure.Process.Identity;
using BG.Infrastructure.Common.MEF;

namespace BG.Infrastructure.Process.Configuration
{
    public interface IBusinessProcessParams
    {
        Guid Guid { get; }
    }

    public class EmptyBusinessProcessParams : IBusinessProcessParams
    {
        public Guid Guid { get { return Guid.Empty; } }
    }

    public class BusinessProcessParams: IBusinessProcessParams
    {
        public Guid Guid { set; get; }

        public int ProcessId { set; get; }

        public bool FullValidation { get; set; }
    }

    public interface IBusinessProcessConfuguration
    {
        TBusinessProcess CreateBusinessProcess<TBusinessProcess>(IUser currentUser, string entityName, EntityType entityType, string configurationName, BusinessProcessParams @params = null)
            where TBusinessProcess : IBusinessProcess;
    }
}
