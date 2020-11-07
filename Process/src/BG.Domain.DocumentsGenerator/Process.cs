using Akka;
using Akka.Actor;
using Akka.Event;
using Akka.Streams;
using Akka.Streams.Dsl;
using Akka.Streams.Util;
using BG.DAL.Interfaces;
using BG.Domain.DocumentsGenerator.Enumerators;
using BG.Domain.DocumentsGenerator.Generators;
using BG.Domain.DocumentsGenerator.Generators.Impl;
using BG.Domain.DocumentsGenerator.Repositories;
using BG.Infrastructure.Process.Process;
using BG.Infrastructure.Process.Process.Impl;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BG.Domain.DocumentsGenerator
{
    public class Process<TData>: ActorSystemProcessBase where TData : class, new()
    {
        const int _threadCount = 10;
        const int _pageSize = 40;

        readonly Func<IGenerator<TData>> _generator;
        readonly IDataEnumerator<TData> _sourceEnumerator;
        readonly IEmitRepository _emitRepository;

        volatile int processedCount = 0;
        volatile int count = 1;

        public Process(Logger logger, Func<IGenerator<TData>> generator, IDataEnumerator<TData> sourceEnumerator, IEmitRepository emitRepository)
            : base(logger)
        {
            _sourceEnumerator = sourceEnumerator;
            _emitRepository = emitRepository;
            _generator = generator;
        }

        protected override string Name => "Documents-Generator";

        protected override string DisplayName => "Генератор печатной документации";

        protected override IRunnableGraph<NotUsed> CreateRunnableGraph(ActorMaterializer mat)
        {
            count = _sourceEnumerator.Count;

            return Source.FromEnumerator(() => { RaiseMessage("Извлечение данных...");  return _sourceEnumerator; })
                         .Buffer(_pageSize, OverflowStrategy.Backpressure)
                         .SelectAsyncUnordered(_threadCount, ProcessData)
                         .Log("DocumentGeneration", d => d?.Display)
                         .WithAttributes(Attributes.CreateLogLevels(onElement: Akka.Event.LogLevel.InfoLevel))
                         .Async()
                         .Select(NotifyProcessData)
                         //.ViaMaterialized(KillSwitches.Single<Result>(), Keep.Right)
                         .To(Sink.OnComplete<Result>(() =>
                           {
                               RaiseMessage($"ОК!");
                               Logger.Info("Успешно завершена обработка");
                               mat.System.Terminate();
                           }, e =>
                           {
                               if (!(e is AbruptTerminationException))
                               {
                                   RaiseMessage($"Ошибка: {e.Message}");
                                   Logger.Error(e.ToString());
                               }
                               mat.System.Terminate();
                           }));

        }

        protected override void AfterExecuteProcess()
        {
            base.AfterExecuteProcess();

            _sourceEnumerator.Dispose();
            _emitRepository.Dispose();
        }

        async Task<Result> ProcessData(TData data)
        {
            var res = await _generator().RunAsync(data);
            processedCount++;
            return res;
        }

        Result NotifyProcessData(Result res)
        {
            RaiseMessage(res?.Display, (int)(processedCount * 100.0 / count));
            return res;// Task.FromResult(res);
        }

        //static void RaiseMessage(string ex, int count = 0)
        //{
        //    Console.WriteLine($"ex:{ count}");
        //}

        #region temp1
        //public async Task<Result> GetWeatherAsync(TData data)
        //{
        //    Result result = null;

        //    try
        //    {
        //        var httpClient = new HttpClient();
        //        //var requestUrl = $"http://api.met.no/weatherapi/locationforecast/1.9/?lat={coordinates.Latitude};lon={coordinates.Latitude}";
        //        //var requestUrl = $"http://api.met.no/weatherapi/locationforecast/1.9/?lat={coordinates.Latitude.ToString("R").Replace(",",".")}&lon={coordinates.Latitude.ToString("R").Replace(",", ".")}";
        //        var requestUrl = "https://api.met.no/weatherapi/locationforecast/1.9/?lat=50.2513366&lon=50.2513366";
        //        //var result = httpClient.GetStringAsync(requestUrl);
        //        result = await _generator().RunAsync(data);
        //        //var rr = result.Result;
        //        //return result;
        //        //var doc = XDocument.Parse(rr);
        //        //var temp = doc.Root.Descendants("temperature").First().Attribute("value").Value;
        //    }
        //    catch(Exception e)
        //    {

        //    }

        //    //var ret = 0m;
        //    //decimal.TryParse(temp, out ret);
        //    return result;//Task.FromResult(new Result(ret.ToString()));
        //}
        #endregion

        #region temp
        //public Flow<TIn, TOut, NotUsed> Balancer<TIn, TOut>(Flow<TIn, TOut, NotUsed> worker, int workerCount)
        //{
        //    return Flow.FromGraph(GraphDsl.Create(b =>
        //    {
        //        var balancer = b.Add(new Balance<TIn>(workerCount, waitForAllDownstreams: true));
        //        var merge = b.Add(new Merge<TOut>(workerCount));

        //        for (var i = 0; i < workerCount; i++)
        //            b.From(balancer).Via(worker.Async()).To(merge);

        //        return new FlowShape<TIn, TOut>(balancer.In, merge.Out);
        //    }));
        //}
        #endregion

        #region Old2
        //protected override IRunnableGraph<NotUsed> CreateRunnableGraph(ActorSystem sys, ActorMaterializer mat)
        //{

        //    //RunnableGraph<Tuple<TaskCompletionSource<int>, ICancelable, Task<int>>> r12 =
        //    //    RunnableGraph.FromGraph(GraphDsl.Create(source, flow, sink,
        //    //        Tuple.Create,
        //    //        (builder, src, f, dst) =>
        //    //        {
        //    //            builder.From(src).Via(f).To(dst);
        //    //            return ClosedShape.Instance;
        //    //        }));

        //    return Source.FromEnumerator(() => new ExcelSourceEnumerator<TData>(@"D:\users\pavlichev.a\Projects\BG\Applications\src\BG.Application.RunConsole\Организации.xlsx"))
        //                   //.Via(Flow.Create<TData>().Select(tweet => tweet)
        //                   //                          .Buffer(2, OverflowStrategy.DropNew)   
        //                   //                           .Throttle(1, TimeSpan.FromSeconds(1), 10, ThrottleMode.Shaping))
        //                   //.Via(
        //                   //           Flow.Create<TData>()

        //                   //           //.Buffer(_pageSize + 10 , OverflowStrategy.Backpressure)
        //                   //           //.SelectAsyncUnordered(_threadCount, async d =>
        //                   //           //{
        //                   //           //    var res = await Task.Run(() => new Result("1")); //_generator().RunAsync(d);
        //                   //           //    processedCount++;
        //                   //           //    RaiseMessage(res?.Display, (int)(processedCount * 100.0 / 100));
        //                   //           //    return res;
        //                   //           //})
        //                   //           //.Log("DocumentGeneration")
        //                   //           .Select(r =>
        //                   //           {
        //                   //               var res = _generator().Run(r);
        //                   //               processedCount++;
        //                   //               RaiseMessage(res?.Display, (int)(processedCount * 100.0 / 100));
        //                   //               return res;
        //                   //           }).Async()
        //                   //           //)
        //                   //           //.Log("DocumentGeneration", d => !d.IsError?  $"Обработана: {d.Display}":$"Ошибка: {d.Display}")
        //                   //           //.WithAttributes(Attributes.CreateLogLevels(onElement: LogLevel.InfoLevel))
        //                   //           //.Recover(e => {
        //                   //           //    if (e is AggregateException)
        //                   //           //        return new Option<Result>(new Result(e.Message, ((AggregateException)e).Flatten().ToString()));

        //                   //           //    return Option<Result>.None;
        //                   //           //})
        //                   //           //)
        //                   //           //.SelectError((r)=> {
        //                   //           //    var ss = r;
        //                   //           //    return r;
        //                   //           //}))
        //                   //           //Balancer(Flow.Create<TData>().Select(r =>
        //                   //           //{
        //                   //           //    var res = _generator().Run(r);
        //                   //           //    processedCount++;
        //                   //           //    RaiseMessage(res?.Display, (int)(processedCount * 100.0 / 100));
        //                   //           //    return res;}
        //                   //           //), _threadCount)
        //                   //           )
        //                   .Via(Flow.Create<TData>().Select(t => new { Data = t, Generator = _generator() }).SelectAsync(5, async r => {
        //                       //return await Task.Run(() => { new Generator<TData>(null, "", "", null).Run(r); return new Result("1"); }); }))
        //                       return await Task.Run(() => { r.Generator?.Run(r.Data); return new Result("1"); });
        //                   }))
        //                   //.To(Sink.Ignore<Result>());
        //                   .To(Sink.OnComplete<Result>(() =>
        //                   {
        //                       var rr = "dd";
        //                   }, e =>
        //                   {
        //                       var ss1 = sys;
        //                       var matt = mat;
        //                       var ss = e;
        //                   }));
        //    //.To(Sink.ForEach<Result>(r=> {
        //    //    var t = r;
        //    //}));
        //}

        #endregion

        #region Old
        //protected override IRunnableGraph<NotUsed> CreateRunnableGraph()
        //{
        //    var excelSource = Source.FromEnumerator(() => new ExcelSourceEnumerator<TData>(_sourcePath));
        //    //var formatUser = Flow.Create<IUser>()
        //    //    .Select(Utils.FormatUser);
        //    //var formatTemperature = Flow.Create<decimal>()
        //    //    .Select(Utils.FormatTemperature);
        //    var writeSink = Sink.Ignore<Result>();

        //    var graph = GraphDsl.Create(b =>
        //    {
        //        var broadcast = b.Add(new Broadcast<TData>(1));
        //        var merge = b.Add(new Merge<Result>(1));
        //        //b.From(broadcast.Out(0))
        //        //    .Via(Flow.Create<ITweet>().Select(tweet => tweet.CreatedBy)
        //        //        .Throttle(10, TimeSpan.FromSeconds(1), 1, ThrottleMode.Shaping))
        //        //    .Via(formatUser)
        //        //    .To(merge.In(0));
        //        b.From(broadcast.Out(0))
        //            //.Via(Flow.Create<TData>().Select(tweet => tweet.Coordinates)
        //            //    .Buffer(10, OverflowStrategy.DropNew)
        //            //    .Throttle(1, TimeSpan.FromSeconds(1), 10, ThrottleMode.Shaping))
        //            .Via(Flow.Create<TData>().Buffer(_pageSize, OverflowStrategy.Backpressure).Log("test", d => "").SelectAsync(_threadCount, async d => {
        //                var res = await _generator().RunAsync(d);
        //                processedCount++;
        //                RaiseMessage(res?.Display, (int)(processedCount * 100.0 / count));//$"Обработано: {processedCount} из {count}"
        //                return res;
        //            }))
        //            .To(merge.In(0));

        //        return new FlowShape<TData, Result>(broadcast.In, merge.Out);
        //    });

        //    return excelSource.Via(graph).Select(d => d).To(writeSink);
        //}
        #endregion


    }
}
