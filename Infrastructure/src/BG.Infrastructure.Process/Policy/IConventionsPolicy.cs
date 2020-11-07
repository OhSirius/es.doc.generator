using BG.Infrastructure.Process.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.BusinessProcess.Policy
{
    /// <summary>
    /// Предоставляет политику разрешений для компонента <see cref="TEntity"/>
    /// </summary>
    public interface IConventionsPolicy<TEntity>
    {
        void Add(ConventionCollection<TEntity> conventions);
        void Add(IConvention<TEntity> convention);
        // bool Matches(TEntity entity);
        bool Matches(IConventionParams<TEntity> conventionParams);
    }

    public class ConventionCollection<TEntity> : List<IConvention<TEntity>>
    {
        public ConventionCollection()
        {
        }

        public ConventionCollection(IEnumerable<IConvention<TEntity>> convs)
            : base(convs)
        {
        }
    }
}
