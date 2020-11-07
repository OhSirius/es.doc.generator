using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BG.Extensions;

namespace BG.Infrastructure.Process.Configuration
{
    public class BusinessOperationConfigCollection<TContainer> : Dictionary<string, IBusinessOperationConfig<TContainer>> where TContainer:class
    {
        public BusinessOperationConfigCollection()
        {
        }

        public BusinessOperationConfigCollection(IDictionary<string, IBusinessOperationConfig<TContainer>> configs)
        {
            if (!configs.IsNullOrEmpty())
                configs.ForEach(config => this.New(config.Key, config.Value));
        }

        public BusinessOperationConfigCollection<TContainer> New(string operationName, IBusinessOperationConfig<TContainer> operationConfig)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка добавления новой конфигурации для стратегии: название конфигурации не может быть нулевым");
            Guard.Against<ArgumentNullException>(operationConfig == null, "Ошибка добавления новой конфигурации для стратегии: объект конфигурации не может быть нулевым");
            Guard.Against<ArgumentNullException>(this.ContainsKey(operationName), string.Format("Ошибка добавления новой конфигурации для стратегии: данная конфигурация с именем {0} уже присутствует в коллекции", operationName));

            base.Add(operationName, operationConfig);
            return this;
        }

        public BusinessOperationConfigCollection<TContainer> Update(string operationName, IBusinessOperationConfig<TContainer> operationConfig)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка обновления конфигурации для стратегии: название конфигурации не может быть нулевым");
            Guard.Against<ArgumentNullException>(operationConfig == null, "Ошибка обновления конфигурации для стратегии: объект конфигурации не может быть нулевым");

            if (!this.ContainsKey(operationName))
                base.Add(operationName, operationConfig);
            else
                base[operationName] = operationConfig;

            return this;
        }

        public IBusinessOperationConfig<TContainer> Get(string operationName)
        {
            Guard.Against<ArgumentNullException>(string.IsNullOrEmpty(operationName), "Ошибка получения конфигурации для стратегии: название конфигурации не может быть нулевым");
            Guard.Against<ArgumentNullException>(!this.ContainsKey(operationName), string.Format("Ошибка получения конфигурации для стратегии: данная конфигурация с именем {0} отсутствует в коллекции", operationName));

            return base[operationName];
        }

        public static BusinessOperationConfigCollection<TContainer> Create()
        {
            return new BusinessOperationConfigCollection<TContainer>();
        }

        public new void Add(string key, IBusinessOperationConfig<TContainer> value)
        {
            throw new NotSupportedException("Ошибка добавления новой конфигурации для стратегии: метод Add является недопостимым, вместо него необходимо использовать метод AddConfig");
        }

        public new IBusinessOperationConfig<TContainer> this[string key]
        {
            set
            {
                throw new NotSupportedException("Ошибка обновления (добавления) конфигурации для стратегии: индексатор является недопостимым, вместо него необходимо использовать метод AddConfig или UpdateConfig");
            }
            get
            {
                throw new NotSupportedException("Ошибка получения  конфигурации для стратегии: индексатор является недопостимым, вместо него необходимо использовать метод GetConfig");
            }
        }
    }

}
