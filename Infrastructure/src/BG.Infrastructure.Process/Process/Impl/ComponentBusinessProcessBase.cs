using BG.Infrastructure.Process.NCommon.Data;
using BG.Infrastructure.Process.Process;
//using Aetp.CRM2.Models.EF.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Extensions;
using BG.Infrastructure.Process.Identity;

namespace BG.Infrastructure.Process.Process
{
    public class ComponentBusinessProcessBase<TEntity> : EntityBusinessProcessBase<TEntity>
        where TEntity : class
    {

        public ComponentBusinessProcessBase(IUser currentUser, IUnitOfWorkScopeFactory scopeFactory,
                                    ISaveEntityOperation<TEntity> saveOperation,
                                    ICreateEntityOperation<TEntity> createOperation,
                                    //Func< BG.Infrastructure.Process.Process.ICreateEntityOperation<Aetp.CRM2.Models.EF.Main.Person>> createOperation,
                                    IDeleteEntityOperation<TEntity> deleteOperation)
            :base()
        {
           
            this.SaveOperation = saveOperation;
            this.CreateOperation = createOperation;
            this.DeleteOperation = deleteOperation;
            this.ScopeFactory = scopeFactory;
            CurrentUser = currentUser;
        }

        protected IUser CurrentUser { get; set; }
        public IUnitOfWorkScopeFactory ScopeFactory { private set; get; }

        #region Операции
        protected ISaveEntityOperation<TEntity> SaveOperation{set;get;}
        protected ICreateEntityOperation<TEntity> CreateOperation { set; get; }
        protected IDeleteEntityOperation<TEntity> DeleteOperation { set; get; }
        #endregion

        public override TEntity Create()
        {
            Guard.Against<ArgumentNullException>(CurrentUser == null, string.Format("Ошибка создания бизнес-процесса типа {0}: не задан тек. пользователь", typeof(TEntity)));
            Guard.Against<ArgumentNullException>(CreateOperation == null, string.Format("Ошибка создания бизнес-процесса типа {0}: не определена стратегия создания", typeof(TEntity)));

            TEntity newEntity = null;
            ExecuteOperation(CreateOperation, op =>
            {
                newEntity = op.CreateEntity();
            });
            return newEntity;
        }

        public override TEntity Save(TEntity entity)
        {
            Guard.Against<ArgumentNullException>(CurrentUser == null, string.Format("Ошибка сохранения бизнес-процесса типа {0}: не задан тек. пользователь", typeof(TEntity)));
            Guard.Against<ArgumentNullException>(SaveOperation == null, string.Format("Ошибка сохранения бизнес-процесса типа {0}: не определена стратегия сохранения", typeof(TEntity)));

            TEntity saveEntity = null;

            ExecuteOperation(SaveOperation, op =>
            {
                saveEntity = op.SaveEntity(entity);
            });
            return saveEntity;

        }

        public override void Delete(TEntity entity)
        {
            Guard.Against<ArgumentNullException>(CurrentUser == null, string.Format("Ошибка удаления бизнес-процесса типа {0}: не задан тек. пользователь", typeof(TEntity)));
            Guard.Against<ArgumentNullException>(DeleteOperation == null, string.Format("Ошибка удаления бизнес-процесса типа {0}: не определена стратегия удаления", typeof(TEntity)));

            var operation = DeleteOperation;
            ExecuteOperation(operation, (op) =>
            {
                operation.DeleteEntity(entity);
            });
        }

        public override void Delete(TEntity[] entities)
        {
            Guard.Against<ArgumentNullException>(CurrentUser == null, string.Format("Ошибка удаления бизнес-процесса типа {0}: не задан тек. пользователь", typeof(TEntity)));
            Guard.Against<ArgumentNullException>(ScopeFactory == null, string.Format("Ошибка удаления бизнес-процесса типа {0}: не определена фабрика транзакций", typeof(TEntity)));
            Guard.Against<ArgumentNullException>(DeleteOperation == null, string.Format("Ошибка удаления бизнес-процесса типа {0}: не определена стратегия удаления", typeof(TEntity)));

            if (entities.IsNullOrEmpty())
                return;

            var _scope = ScopeFactory.Create();

            try
            {
                foreach (var entity in entities)
                {
                    ExecuteOperation(DeleteOperation, op =>
                    {
                        op.DeleteEntity(entity);
                    });

                    if (!IsValidLastOperation)
                        break;
                }

                if (IsValidLastOperation)
                    _scope.Commit();

            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                _scope.Dispose();
            }
        }

        public override TEntity[] Save(TEntity[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
