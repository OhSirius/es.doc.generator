using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.Impl
{
    public abstract class ObjectBusinessProcessBase : IObjectBusinessProcess
    {

        public bool IsValidLastOperation { protected set; get; }

        public string LastOperationErrorText { protected set; get; }

        protected void SetOperationState(IEntityOperation operation)
        {
            Guard.Against<ArgumentNullException>(operation == null, "Ошибка установки состояния операции: не определен объект");

            IsValidLastOperation = operation.IsValid;
            LastOperationErrorText = operation.ErrorText;
        }
        protected void SetOperationState(bool isValid, string errorText)
        {
            IsValidLastOperation = isValid;
            LastOperationErrorText = errorText;
        }

        protected void ExecuteOperation<TEntityOperation>(TEntityOperation operation, Action<TEntityOperation> actions) where TEntityOperation : IEntityOperation
        {
            try
            {
                actions(operation);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                SetOperationState(operation);
            }
        }





    }
}
