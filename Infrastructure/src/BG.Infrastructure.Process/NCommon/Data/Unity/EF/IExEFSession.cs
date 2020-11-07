using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.NCommon.Data.Unity.EF
{
    public interface IExEFSession : IEFSession
    {
        void AfterSaveChanges();
    }
}
