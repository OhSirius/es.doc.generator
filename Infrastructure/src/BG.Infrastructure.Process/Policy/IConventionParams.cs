using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Policy
{
    public interface IConventionParams<TEntity>
    {
        TEntity Entity     { get; set; }
    }

    public interface IObjectAccessConventionParams<TEntity> : IConventionParams<TEntity>
    {
        String  EntityName { get; set; }
    }

    public interface IFieldAccessConventionParams<TEntity> : IObjectAccessConventionParams<TEntity>
    {
        String FieldName   { get; set; }
    }
}
