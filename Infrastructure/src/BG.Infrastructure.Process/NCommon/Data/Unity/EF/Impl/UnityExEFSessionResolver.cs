using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using BG.Extensions;
using BG.Infrastructure.Process.Logging;
using NCommon;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BG.Infrastructure.Process.NCommon.Data.Unity.EF.Impl
{
    public class UnityExEFSessionResolver<TLoggingContext,TContext> :IExEFSessionResolver<TLoggingContext,TContext>
        where TContext : DbContext
        where TLoggingContext : ILoggerDataContext<TContext>
    {
        protected readonly IDictionary<string, Guid> _objectContextTypeCache = new Dictionary<string, Guid>();
        protected readonly IDictionary<Guid, Func<TLoggingContext>> _objectContexts = new Dictionary<Guid, Func<TLoggingContext>>();

        /// <summary>
        /// Gets the number of <see cref="ObjectContext"/> instances registered with the session resolver.
        /// </summary>
        public int ObjectContextsRegistered
        {
            get { return _objectContexts.Count; }
        }

        /// <summary>
        /// Gets the unique ObjectContext key for a type. 
        /// </summary>
        /// <typeparam name="T">The type for which the ObjectContext key should be retrieved.</typeparam>
        /// <returns>A <see cref="Guid"/> representing the unique object context key.</returns>
        public Guid GetSessionKeyFor<T>()
        {
            var typeName = typeof(T).Name;
            Guid key;
            if (!_objectContextTypeCache.TryGetValue(typeName, out key))
                throw new ArgumentException("No ObjectContext has been registered for the specified type.");
            return key;
        }

        /// <summary>
        /// Opens a <see cref="IEFSession"/> instance for a given type.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="IEFSession"/> is returned.</typeparam>
        /// <returns>An instance of <see cref="IEFSession"/>.</returns>
        public virtual IEFSession OpenSessionFor<T>()
        {
            ILoggingAdapter logger = null;
            var context = GetObjectContextFor<T>(out logger);
            return new UnityEFSession(context, logger);
            //return new EFSession(context);
        }

        /// <summary>
        /// Gets the <see cref="ObjectContext"/> that can be used to query and update a given type.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="ObjectContext"/> is returned.</typeparam>
        /// <returns>An <see cref="ObjectContext"/> that can be used to query and update the given type.</returns>
        public ObjectContext GetObjectContextFor<T>()
        {
            ILoggingAdapter logger=null;
            return GetObjectContextFor<T>(out logger);
        }

        protected ObjectContext GetObjectContextFor<T>(out ILoggingAdapter logger)
        {
            var typeName = typeof(T).Name;
            Guid key;
            if (!_objectContextTypeCache.TryGetValue(typeName, out key))
                throw new ArgumentException("No ObjectContext has been registered for the specified type.");

            var context = _objectContexts[key]();
            logger = context.GetLogger();
            return ((IObjectContextAdapter)context.GetContext()).ObjectContext;
        }

        /// <summary>
        /// Registers an <see cref="ObjectContext"/> provider with the resolver.
        /// </summary>
        /// <param name="contextProvider">A <see cref="Func{T}"/> of type <see cref="ObjectContext"/>.</param>
        public void RegisterObjectContextProvider(Func<ObjectContext> contextProvider)
        {
            //var key = Guid.NewGuid();
            //_objectContexts.Add(key, contextProvider);
            ////Getting the object context and populating the _objectContextTypeCache.
            //var context = contextProvider();
            //var entities = context.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
            //entities.ForEach(entity => _objectContextTypeCache.Add(entity.Name, key));
            throw new NotImplementedException("Ошибка регистрации провайдера контекста в UnityExEFSessionResolver: метод с ObjectContext не поддерживается");
        }

        //protected readonly IDictionary<Guid, Func<ILoggingAdapter>> _objectLoggers = new Dictionary<Guid, Func<ILoggingAdapter>>();

        //public ILoggingAdapter GetObjectLoggerFor<T>()
        //{
        //    var typeName = typeof(T).Name;
        //    Guid key;
        //    if (_objectContextTypeCache.TryGetValue(typeName, out key))
        //        return _objectLoggers[key]();
        //    else
        //        return null;
        //}


        //public override IEFSession OpenSessionFor<T>()
        //{
        //    var context = GetObjectContextFor<T>();
        //    var logger = GetObjectLoggerFor<T>();
        //    return new UnityEFSession(context, logger);
        //}

        public void RegisterObjectContextProvider(Func<TLoggingContext> contextProvider)
        {
            Guard.Against<ArgumentNullException>(contextProvider == null, "Ошибка установки провайдера контекста данных : не определен провайдер");
            //Guard.Against<ArgumentNullException>(loggingProvider == null, "Ошибка установки провайдера логгирования : не определен провайдер логгирования");

            var key = Guid.NewGuid();
            _objectContexts.Add(key, contextProvider);
            //_objectLoggers.Add(key, loggingProvider);
            //Getting the object context and populating the _objectContextTypeCache.
            var context = ((IObjectContextAdapter)contextProvider().GetContext()).ObjectContext;
            var entities = context.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
            entities.ForEach(entity => _objectContextTypeCache.Add(entity.Name, key));
        }

        //public void RegisterObjectContextProvider(Func<ObjectContext> contextProvider, Func<ILoggingAdapter> loggingProvider)
        //{
        //    Guard.Against<ArgumentNullException>(contextProvider == null, "Ошибка установки провайдера контекста данных : не определен провайдер");
        //    Guard.Against<ArgumentNullException>(loggingProvider == null, "Ошибка установки провайдера логгирования : не определен провайдер логгирования");

        //    var key = Guid.NewGuid();
        //    _objectContexts.Add(key, contextProvider);
        //    _objectLoggers.Add(key, loggingProvider);
        //    //Getting the object context and populating the _objectContextTypeCache.
        //    var context = contextProvider();
        //    var entities = context.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
        //    entities.ForEach(entity => _objectContextTypeCache.Add(entity.Name, key));
        //}

    }
}
