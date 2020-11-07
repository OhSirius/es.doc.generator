using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Policy.Impl
{
    public class ConventionParams<TEntity> : IConventionParams<TEntity>
    {
        public ConventionParams() { }
        public ConventionParams(TEntity entity)
        {
            Entity     = entity;
        }

        public TEntity Entity     { get; set; }
    }

    public class ObjectAccessConventionParams<TEntity> : IObjectAccessConventionParams<TEntity>
    {
        public ObjectAccessConventionParams() { }
        public ObjectAccessConventionParams(TEntity entity)
        {
            Entity     = entity;
        }
        public ObjectAccessConventionParams(TEntity entity, String entityName)
            : this(entity)
        {
            EntityName = entityName;
        }

        public TEntity Entity     { get; set; }
        public String  EntityName { get; set; }
    }

    public class FieldAccessConventionParams<TEntity> : IFieldAccessConventionParams<TEntity>
    {
        public FieldAccessConventionParams() { }
        public FieldAccessConventionParams(TEntity entity)
        {
            Entity     = entity;
        }
        public FieldAccessConventionParams(TEntity entity, String entityName): this(entity)
        {
            EntityName = entityName;
        }
        public FieldAccessConventionParams(TEntity entity, String entityName, String fieldName): this(entity, entityName)
        {
            FieldName  = fieldName;
        }

        public TEntity Entity     { get; set; }
        public String  EntityName { get; set; }
        public String  FieldName  { get; set; }
    }

    public static class ConventionParamsFactory
    {
        public static IConventionParams<TEntity> Create<TEntity>()
        {
            return new ConventionParams<TEntity>();
        }
        public static IConventionParams<TEntity> Create<TEntity>(TEntity entity)
        {
            return new ConventionParams<TEntity>(entity);
        }
        public static IObjectAccessConventionParams<TEntity> Create<TEntity>(TEntity entity, String entityName)
        {
            return new ObjectAccessConventionParams<TEntity>(entity, entityName);
        }
        public static IFieldAccessConventionParams<TEntity>  Create<TEntity>(TEntity entity, String entityName, String fieldName)
        {
            return new FieldAccessConventionParams<TEntity> (entity, entityName, fieldName);
        }
    }

}
