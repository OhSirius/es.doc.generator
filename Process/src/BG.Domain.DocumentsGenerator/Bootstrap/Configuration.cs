using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Common.Processes;
using BG.Domain.DocumentsGenerator.Generators;
using BG.Domain.DocumentsGenerator.Generators.Impl;
using BG.Domain.DocumentsGenerator.Repositories;
using BG.Domain.DocumentsGenerator.Repositories.Impl;
using NLog;
using BG.Infrastructure.Morphology;
using BG.Domain.DocumentsGenerator.Formatters;
using BG.Domain.DocumentsGenerator.Formatters.Impl;
using BG.DAL.Attributes;
using BG.Infrastructure.Morphology.Padeg;
using BG.Infrastructure.Process.Configuration.Impl.Unity;
using BG.Infrastructure.Process.Process;
using BG.Infrastructure.Common.Utility;
using BG.Domain.DocumentsGenerator.Enumerators;
using BG.Domain.DocumentsGenerator.Enumerators.Impl;

namespace BG.Domain.DocumentsGenerator.Bootstrap
{
    public static class Configuration 
    {
        public static IProcess Configure<TTemplateData>(string sourcePath, string templatePath, string outPath) where TTemplateData: class, new()
        {
            Guard.AssertNotEmpty(sourcePath, "Не определен путь источника");
            Guard.AssertNotEmpty(templatePath, "Не определен пусть к шаблону");

            IUnityContainer container = new UnityContainer();
            UnityBusinessConfigure.Using<IProcess>(container)
                      //.ConfigureOperation<ISourceRepository<TTemplateData>, ExcelSourceRepository<TTemplateData>>(new ConstructorParameterCollection().Add("sourcePath", sourcePath))
                      .ConfigureOperation<IDataEnumerator<TTemplateData>, ExcelSourceEnumerator<TTemplateData>>(new ConstructorParameterCollection().Add("sourcePath", sourcePath))
                      .ConfigureSingletonParameter<Logger>(LogManager.GetLogger("DocumentGeneration"))
                     //.RegisterInstance<IEmitRepository>(new WordEmitRepository())
                     //.RegisterType<IEmitRepository, WordEmitRepository>(new InjectionConstructor(new InjectionParameter(templatePath)))
                     .ConfigureOperation<IEmitRepository, WordEmitRepository>()
                     .ConfigureOperation<IFormatter, DefaultFormatter>()
                     .ConfigureOperation<IFormatter, DefaultFormatter>("Default")
                     .ConfigureOperation<IFormatter, AppointmentDeclensionFormatter>("AppointmentDeclension")
                     .ConfigureOperation<IFormatter, PersonNameFormatter>("PersonName")
                     .ConfigureOperation<IFormatter, WelcomeFormatter>("Welcome")
                     .ConfigureOperation<IFormatter, PersonTotalNameDeclensionFormatter>("PersonTotalNameDeclension")
                     .ConfigureOperation<IDictionary<FilterType, IFormatter>, Dictionary<FilterType, IFormatter>>(c => new Dictionary<FilterType, IFormatter>()
                                       {
                                          { FilterType.None, c.Resolve<IFormatter>("Default") }
#if(Basis || Pro || DEBUG)
                                         ,{ FilterType.PersonDeclension, c.Resolve<IFormatter>("PersonTotalNameDeclension") },
                                          { FilterType.OnlyPersonName, c.Resolve<IFormatter>("PersonName") },
                                          { FilterType.AppointmentDeclension, c.Resolve<IFormatter>("AppointmentDeclension") },
                                          { FilterType.Welcome, c.Resolve<IFormatter>("Welcome") }
#endif
                     })
                     .ConfigureOperation<IFormatterFactory, FormatterFactory>()
                     .ConfigureOperation<IDeclension, Declension>()
                     .ConfigureOperation<IGenerator<TTemplateData>, Generator<TTemplateData>>(//new InjectionConstructor(new ResolvedParameter<IEmitRepository>(), new InjectionParameter(outPath), new InjectionParameter(templatePath), new ResolvedParameter<IFormatterFactory>()))
                          new ConstructorParameterCollection().Add("paths", new Tuple<string, string>(outPath, templatePath)))//.Add("templatePath", templatePath).Add("outPath", outPath))
                     //.ConfigureProcess<IProcess, Process<TTemplateData>>(c => new Process<TTemplateData>(c.Resolve<Logger>(), c.Resolve<ISourceRepository<TTemplateData>>(), () => c.Resolve<IGenerator<TTemplateData>>(), c.Resolve<IEmitRepository>()))
                     .ConfigureProcess<IProcess, Process<TTemplateData>>(c =>new Process<TTemplateData>(c.Resolve<Logger>(), () => { lock(c) return c.Resolve<IGenerator<TTemplateData>>(); }
                                                                                            , c.Resolve<IDataEnumerator<TTemplateData>>(), c.Resolve<IEmitRepository>())
                     )
                     .BuildConfiguration();//new InjectionFactory(c=>new Process<TTemplateData>(c.Resolve<Logger>(), c.Resolve<ISourceRepository<TTemplateData>>(), ()=>c.Resolve<IGenerator<TTemplateData>>(), c.Resolve<IEmitRepository>()))
                                                                                                                                                                                                                                                                           //.RegisterType<IProcess, Process<TTemplateData>>();
            return container.Resolve<IProcess>();
        }
    }
}
