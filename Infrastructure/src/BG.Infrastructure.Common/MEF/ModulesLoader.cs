using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web;
using System.IO;
using BG.Extensions;
using System.Diagnostics.Tracing;

namespace BG.Infrastructure.Common.MEF
{
    public class ModulesLoader
    {

        #region .ModulesLoader
        static readonly object locker = new object();
        static ModulesLoader()
        {
            lock (locker)
            {
                //ServerConfiguration.Default.Initialize();
                @default = new ModulesLoader();
                @default.Load(typeof(ModulesLoader).Assembly, @default);
                @default.SetupModules();
            }
        }
        #endregion

        #region Default
        static ModulesLoader @default;
        public static ModulesLoader Default
        {
            get
            {
                lock (locker)
                    return @default;
            }
        }
        #endregion

        [ImportMany]
        public IEnumerable<Lazy<IModuleInstaller, IModuleMetaData>> Installers { set; get; }

        public void Load(System.Reflection.Assembly assembly, params object[] attributedParts)
        {
            try
            {
                //An aggregate catalog that combines multiple catalogs
                var catalog = new AggregateCatalog();
                //Adds all the parts found in the same assembly as the ModuleLoader class
                //catalog.Catalogs.Add(new AssemblyCatalog(typeof(ModulesLoader).Assembly));
                if (assembly != null)
                    catalog.Catalogs.Add(new AssemblyCatalog(assembly));

                ////Загружаем сборки из папок расширений
                //if (!ServerSettings.Default.ModulesPaths.IsNullOrEmpty())
                //{
                //    foreach (var modulePath in ServerSettings.Default.ModulesPaths)
                //    {
                //        if (Directory.Exists(modulePath))
                //        {
                //            //catalog.Catalogs.Add(new DirectoryCatalog("C:\\SimpleCalculator\\SimpleCalculator\\Extensions"));
                //            catalog.Catalogs.Add(new DirectoryCatalog(modulePath));
                //        }
                //    }
                //}

                //if (!ServerSettings.Default.ModulesAssemblies.IsNullOrEmpty())
                //{
                //    foreach (var modulePath in ServerSettings.Default.ModulesAssemblies)
                //    {
                //        if (Directory.Exists(modulePath))
                //        {
                //            //catalog.Catalogs.Add(new DirectoryCatalog("C:\\SimpleCalculator\\SimpleCalculator\\Extensions"));
                //            catalog.Catalogs.Add(new AssemblyCatalog(modulePath));
                //        }
                //    }
                //}
                //Create the CompositionContainer with the parts in the catalog
                var _container = new CompositionContainer(catalog);

                //Fill the imports of this object
                _container.ComposeParts(attributedParts);
            }
            catch (Exception e)
            {
                //Console.WriteLine(compositionException.ToString());
                throw;
            }
        }

        #region SetupModules
        bool isModulesInitialized;
        protected void SetupModules()
        {
            SetupModules(Guid.NewGuid().ToString());
        }
        protected void SetupModules(string guid)
        {
            lock (locker)
            {
                if (isModulesInitialized)
                    return;

                //this.CreateEventSource<DocumentExhangeLoggingEventSource>().BeginInitStaticResource("Installers", guid);
                if (!Installers.IsNullOrEmpty())
                    Installers.ForEach(installer => installer.Value.Setup());
                //this.CreateEventSource<DocumentExhangeLoggingEventSource>().EndInitStaticResource("Installers", guid);
                isModulesInitialized = true;
            }
        }
        #endregion

        public TEventSource CreateEventSource<TEventSource>() where TEventSource : EventSource, new()
        {
            var eventSource = new TEventSource(); //SemanticLoggingConfiguration.Default.GetEventSource<TEventSource>();
            return eventSource;
        }

    }
}
