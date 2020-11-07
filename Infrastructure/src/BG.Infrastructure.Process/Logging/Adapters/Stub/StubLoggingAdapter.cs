using BG.Infrastructure.Process.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Process.Identity;

namespace RT.Infrastructure.Process.Logging.Adapters.Stub
{
    public class StubLoggingAdapter : ILoggingAdapter
    {
        public  StubLoggingAdapter(DbContext context, IUser currentUser)
        {

        }

        public void CreateRevisions()
        {
            //throw new NotImplementedException();
        }

        public void DetectChanges()
        {
            //throw new NotImplementedException();
        }
    }
}
