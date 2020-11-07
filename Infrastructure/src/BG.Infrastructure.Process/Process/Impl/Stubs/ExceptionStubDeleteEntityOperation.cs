using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.Impl.Stubs
{
    public class ExceptionStubDeleteEntityOperation<TEntity> : IDeleteEntityOperation<TEntity> where TEntity:class 
    {
        public void DeleteEntity(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool IsValid { get; private set; }
        public string ErrorText { get; private set; }
    }
}
