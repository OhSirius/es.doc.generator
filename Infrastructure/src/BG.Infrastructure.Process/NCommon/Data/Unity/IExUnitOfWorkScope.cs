using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.NCommon.Data.Unity
{
    public interface IExUnitOfWorkScope
    {
        T CurrentUnitOfWork<T>();

        //bool TrackQueryORM { set; get; }

        void SetTrackQueryORM<TEntity>();
    }
}
