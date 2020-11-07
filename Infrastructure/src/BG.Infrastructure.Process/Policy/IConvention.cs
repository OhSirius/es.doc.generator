using BG.Infrastructure.Process.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.BusinessProcess.Policy
{
    public interface IConvention<TEntity> : IChildConvension
    {
        //bool Matches(TEntity entity);
        bool Matches(IConventionParams<TEntity> conventionParams);
    }
}
