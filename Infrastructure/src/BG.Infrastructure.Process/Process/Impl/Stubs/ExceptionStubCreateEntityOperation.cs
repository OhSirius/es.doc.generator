using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.Impl.Stubs
{
    public class ExceptionStubCreateEntityOperation<TEntity> : ICreateEntityOperation<TEntity>
    {
        public TEntity CreateEntity()
        {
            throw new NotImplementedException();
        }

        public bool IsValid { get; private set; }
        public string ErrorText { get; private set; }
    }
}
