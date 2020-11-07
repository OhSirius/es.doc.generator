using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Logging.Adapters.EF.Revisions
{
    public interface IEntityRevisionDescriptionManager
    {
        IEnumerable<EntityRevisionDescription> GetDescriptions(Type type);
    }
}
