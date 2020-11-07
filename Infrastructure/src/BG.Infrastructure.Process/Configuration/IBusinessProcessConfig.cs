using BG.Infrastructure.Process.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Configuration
{
    public interface IBusinessProcessConfig<TContainer, TBusinessProcess>
        where TContainer : class
        where TBusinessProcess : IBusinessProcess
    {
        IBusinessProcessConfig<TContainer, TBusinessProcess> Register(Action<TContainer> actions);

        void Confugure(TContainer container);

    }
}
