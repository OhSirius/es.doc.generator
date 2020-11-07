using BG.Infrastructure.Process.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Extensions;
using BG.Infrastructure.Process.Identity;
using BG.Infrastructure.Common.MEF;

namespace BG.Infrastructure.Process.Process.WorkFlow.Impl
{
    public class WorkFlowBusinessesProcessInvoker : IWorkFlowBusinessProcessesInvoker
    {
        private readonly IUser _currentUser;
        private readonly IWorkFlowBusinessProcessProvider _processProvider;

        public WorkFlowBusinessesProcessInvoker(IUser currentUser, IWorkFlowBusinessProcessProvider processProvider)
        {
            _currentUser = currentUser;
            _processProvider = processProvider;
        }

        public IEnumerable<Response> Invoke(Event @event)
        {
            Guard.AssertNotNull(_currentUser, "Не определен пользователь");
            Guard.AssertNotNull(_processProvider, "Не определен провайдер процессов");

            //var workFlowProcesses = BusinessProcessConfuguration.Default.GetSignalBusinessProcessMetaData();

            var activeProcesses = _processProvider.GetActiveProcesses();

            if (activeProcesses.IsNullOrEmpty())
                return null;
            
            var responses = new List<Response>();

            foreach (var process in activeProcesses)//Усовершенстовать
            {
                if (@event.ProcessId.HasValue && @event.ProcessId.Value != process.ProcessId)//Сообщение предназначено для конкретного процесса
                    continue;

                var res = InvokeProcess(BusinessProcessConfuguration.aNone, EntityType.None, process.ProcessName, process.ProcessId, @event);
                if (res != null)
                    responses.Add(res);
            }
            return responses;
        }

        protected Response InvokeProcess(string entityName, EntityType entityType, string configurationName, int processId, Event @event)
        {
            var process = BusinessProcessConfuguration.Default.CreateBusinessProcess<IWorkFlowBusinessProcess>(_currentUser, entityName, entityType, configurationName, 
                new BusinessProcessParams() { ProcessId = processId });
            var response = process.Raise<Response>(@event);
            return response;
        }

    }
}
