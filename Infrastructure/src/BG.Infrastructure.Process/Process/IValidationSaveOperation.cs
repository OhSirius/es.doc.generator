using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process
{
    public interface IValidationSaveOperation<TEntity> : IEntityOperation  where TEntity : class
    {
        bool Validate(TEntity obj, string parentName, object parentID);
    }
}
