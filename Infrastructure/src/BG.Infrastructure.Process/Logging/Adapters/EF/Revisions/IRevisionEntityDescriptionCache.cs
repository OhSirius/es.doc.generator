using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Logging.Adapters.EF.Revisions
{
    public interface IRevisionEntityDescriptionCache
    {
        bool Contains(Type type);
        void Add(Type type, EntityRevisionDescription description);
        EntityRevisionDescription Get(Type type);
    }
}
