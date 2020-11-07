using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Logging
{
    public interface ILoggingAdapter
    {
        void DetectChanges();
        void CreateRevisions();
    }
}
