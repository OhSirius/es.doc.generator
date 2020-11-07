using BG.Domain.DocumentsGenerator.Generators;
using BG.Domain.DocumentsGenerator.Repositories;
using BG.Infrastructure.Common.Processes;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Extensions;
using BG.Infrastructure.Process.Process;
using System.Threading;

namespace BG.Domain.DocumentsGenerator
{
    //public class ProcessTest<TTemplateData> : IProcess where TTemplateData : class
    //{
    //    readonly ISourceRepository<TTemplateData> _sourceRepository;
    //    readonly Func<IGenerator<TTemplateData>> _generator;
    //    private readonly IEmitRepository _emitRepository;
    //    readonly Logger _logger;

    //    const int _pageSize = 10;
    //    const int _threadCount = 10;

    //    public event Action<ProcessEventsArgs> Processing;

    //    volatile int processedCount = 0;
    //    volatile int count = 0;

    //    public ProcessTest(Logger logger, ISourceRepository<TTemplateData> sourceRepository, Func<IGenerator<TTemplateData>> generator, IEmitRepository emitRepository)
    //    {
    //        _sourceRepository = sourceRepository;
    //        _generator = generator;
    //        _emitRepository = emitRepository;
    //        _logger = logger;
    //    }

    //    public void Execute(CancellationToken token)
    //    {
    //        _logger.Info("Начало работы сервиса генерации документации");
    //        Processing?.Invoke(new ProcessEventsArgs() { Display = "Начало работы сервиса генерации документации" });

    //        try
    //        {
    //            Processing?.Invoke(new ProcessEventsArgs() { Display = "Извлечение данных из источника..." });
    //            count = _sourceRepository.GetCount();

    //            _logger.Info($"Необходимо обработать {count} записей");
    //            Processing?.Invoke(new ProcessEventsArgs() { Display = $"Необходимо обработать {count} записей" });
    //            if (count == 0)
    //            {
    //                _logger.Info("Завершение работы сервиса генерации документации");
    //                return;
    //            }

    //            for (int pageNumber = 0; pageNumber < (count / _pageSize + 1); pageNumber++)
    //            {
    //                if(token!= null && token.IsCancellationRequested)
    //                {
    //                    _logger.Warn("Отмена операции");
    //                    Processing?.Invoke(new ProcessEventsArgs() { Display = $"Отмена работы сервиса генерации документации: обработано {processedCount} из {count} записей" });
    //                    return;
    //                }

    //                var data = _sourceRepository.GetPage(pageNumber, _pageSize);
    //                if (data.IsNullOrEmpty())
    //                    continue;

    //                ProcessData(data, token);
    //                _logger.Info($"Обработана {pageNumber + 1} страница");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.Error(ex.ToString());
    //        }
    //        finally
    //        {
    //            _emitRepository.Dispose();
    //        }

    //        Processing?.Invoke(new ProcessEventsArgs() { Display = $"Завершение работы сервиса генерации документации: обработано {processedCount} из {count} записей" });
    //        _logger.Info($"Завершение работы сервиса генерации документации: обработано {processedCount} из {count} записей");

    //    }

    //    void ProcessData(IEnumerable<TTemplateData> data, CancellationToken token)
    //    {
    //        try
    //        {
    //            //data.ForEach(d =>
    //            //var pq = data.AsParallel().WithDegreeOfParallelism(_threadCount);
    //            //if (token != null)
    //            //    pq = pq.WithCancellation(token);

    //            //pq.ForAll(d =>
    //            //{
    //            //    var res = _generator().Run(d);
    //            //    processedCount++;
    //            //    Processing?.Invoke(new ProcessEventsArgs() { Percent = (int)(processedCount * 100.0 / count ),  Display =  res?.Display });//$"Обработано: {processedCount} из {count}"
    //            //});

    //            var options = new ParallelOptions();
    //            //if (token != null)
    //            //    options.CancellationToken = token;
    //            options.MaxDegreeOfParallelism = _threadCount;

    //            Parallel.ForEach(data, options, d =>
    //            {
    //                var res = _generator().Run(d);
    //                processedCount++;
    //                Processing?.Invoke(new ProcessEventsArgs() { Percent = (int)(processedCount * 100.0 / count), Display = res?.Display });//$"Обработано: {processedCount} из {count}"
    //            });

    //        }
    //        catch (AggregateException ag)
    //        {
    //            if (!ag.InnerExceptions.IsNullOrEmpty())
    //            {
    //                ag.InnerExceptions.ForEach(ex => _logger.Error(ex.ToString()));
    //            }
    //        }
          

    //    }
    //}
}
