using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.Rules.Selectors
{
    public interface IOperationSelector<TEntity, TOperation> where TEntity:class 
    {
        TOperation Get(TEntity entity);
        TOperation Get();
    }
}
