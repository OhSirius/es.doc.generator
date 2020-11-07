using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.Impl.Stubs
{
    public class ExceptionStubValidationSaveOperation<TEntity> : IValidationSaveOperation<TEntity> where TEntity:class 
    {
        public bool Validate(TEntity obj, string parentName, object parentID)
        {
            throw new NotImplementedException();
        }

        public bool IsValid { get; private set; }
        public string ErrorText { get; private set; }
    }
}
