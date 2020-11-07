using Akka;
using Akka.Actor;
using Akka.Configuration;
using Akka.Streams;
using Akka.Streams.Dsl;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.Impl
{
    public abstract class ActorSystemProcessBase : IProcess
    {
        const int stopTimeOut = 1000;//ms

        public event Action<ProcessEventsArgs> Processing;

        public ActorSystemProcessBase(Logger logger)
        {
            Logger = logger;
        }

        protected abstract string Name { get; }

        protected abstract string DisplayName { get; }

        protected Logger Logger { private set; get; }

        //protected virtual Config Config => @"akka.loglevel = DEBUG
        //            akka.loggers=[""Akka.Logger.NLog.NLogLogger, Akka.Logger.NLog""]";

           

        public async Task<bool> Execute(CancellationToken token)
        {
            ActorMaterializer mat = null;
            bool ret = false;

            try
            {
                BeforeExecuteProcess();

                using (var sys = ActorSystem.Create(Name))//, Config))//, Config
                {
                    using (mat = sys.Materializer())// ActorMaterializerSettings.Create(sys).WithInputBuffer(200, 1000)))//new ActorMaterializerSettings(100, 1000,null,)
                    {
                        var graph = CreateRunnableGraph(mat);
                        graph.Run(mat);

                        await WaitStop(token, mat);
                    }
                }

                ret = true;
            }
            catch(AggregateException e)
            {
                if (mat != null && !mat.IsShutdown)
                    await mat.System.Terminate();

                Logger.Error(e.ToString());
            }
            catch (Exception e)
            {
                RaiseMessage($"Ошибка:{e.Message}");
                Logger.Error(e.ToString());
            }
            finally
            {
                AfterExecuteProcess();
            }

            return ret;
        }


        protected virtual void BeforeExecuteProcess() {
            RaiseMessage($"Начало работы сервиса {DisplayName}");
            Logger.Info($"Начало работы сервиса {DisplayName}");
        }

        protected virtual void AfterExecuteProcess() {
            RaiseMessage($"Завершение работы сервиса {DisplayName}");
            Logger.Info($"Завершение работы сервиса {DisplayName}");
        }

        protected abstract IRunnableGraph<NotUsed> CreateRunnableGraph(ActorMaterializer mat);

        protected void RaiseMessage(string message, int percent = 0)
        {
            Guard.AssertNotEmpty(message, "Не определено сообщение вызова");

            Processing?.Invoke(new ProcessEventsArgs() { Display = message, Percent = percent });
        }

        async Task<bool> WaitStop(CancellationToken token, ActorMaterializer mat)
        {
           return await Task.Run(async () =>
           {
               while (!token.IsCancellationRequested && !mat.IsShutdown)
                   Thread.Sleep(stopTimeOut);

               if (!mat.IsShutdown)
               {
                   RaiseMessage("Попытка остановки сервиса...");
                   if (!mat.IsShutdown)
                       await mat.System.Terminate();
                   RaiseMessage("Сервис остановлен");
               }

               return true;
           }, token).ConfigureAwait(false);
        }
    }
}
