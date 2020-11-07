using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Extensions;

namespace BG.Infrastructure.Common.MEF
{
    public class ModulesMatcher<TConfiguration>
    {
        private readonly Func<IEnumerable<Lazy<TConfiguration, IEntityMetaData>>> _getConfigs;
        private readonly IDictionary<EntityType, string> _defaultEntityName = null;
        private readonly string _defaultConfigurationName = null;

        public ModulesMatcher(Func<IEnumerable<Lazy<TConfiguration, IEntityMetaData>>> getConfigs, IDictionary<EntityType, string> defaultEntityName, string defaultConfigurationName)
        {
            _getConfigs = getConfigs;
            _defaultEntityName = defaultEntityName;
            _defaultConfigurationName = defaultConfigurationName;
        }

        public TObject FindBestMatch<TObject>(string entityName, EntityType entityType, string configurationName, Func<TConfiguration, TObject> getObject)
        {
            Guard.AssertNotEmpty(entityName, "Не определено название сущности");
            Guard.AssertNotEmpty(configurationName, "Не определено название процесса");
            //Guard.AssertNotEmpty(_defaultEntityName,"Не определена название сущности по-умолчанию");
            Guard.AssertNotEmpty(_defaultConfigurationName, "Не определена конфигурация по-умочланию");
            Guard.AssertNotNull(getObject, "Не определен метод извлечения объекта");

            TObject obj = default(TObject);
            obj = GetConfigurationObjectBy(entityName, entityType, configurationName, getObject);
            obj = EqualityComparer<TObject>.Default.Equals(obj, default(TObject)) && configurationName != _defaultConfigurationName ? GetConfigurationObjectBy(entityName, entityType, _defaultConfigurationName, getObject) : obj;
            if (!_defaultEntityName.IsNullOrEmpty() && _defaultEntityName.ContainsKey(entityType))
            {
                obj = EqualityComparer<TObject>.Default.Equals(obj, default(TObject)) ? GetConfigurationObjectBy(_defaultEntityName[entityType], entityType, configurationName, getObject) : obj;
                obj = EqualityComparer<TObject>.Default.Equals(obj, default(TObject)) && configurationName != _defaultConfigurationName ? GetConfigurationObjectBy(_defaultEntityName[entityType], entityType, _defaultConfigurationName, getObject) : obj;
            }
            return obj;
        }

        TObject GetConfigurationObjectBy<TObject>(string entityName, EntityType entityType, string configurationName, Func<TConfiguration, TObject> getObject)
        {
            var conf = _getConfigs().SingleOrDefault(c => c.Metadata.EntityName == entityName && c.Metadata.EntityType == entityType && c.Metadata.ProcessName == configurationName).Return(l => l.Value, default(TConfiguration));
            return conf == null ? default(TObject) : getObject(conf);
        }

    }
}
