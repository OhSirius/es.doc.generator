using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Configuration
{
    public interface IBusinessOperationConfig<TContainer> where TContainer:class
    {
        IBusinessOperationConfig<TContainer> Register(Action<TContainer> actions);

        void Confugure(TContainer container);
    }
}
