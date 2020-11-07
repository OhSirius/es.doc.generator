using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Description
{
    public interface IDerivedEntity
    {
        IEntityDescription BaseType { get; }

        bool IsDerivedProperty(string fieldName);
    }
}
