using BG.Infrastructure.Process.Logging;
using NCommon.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.NCommon.Data.Unity.EF.Impl
{
    public class UnityEFSession : EFSession, IExEFSession
    {
        readonly ILoggingAdapter logger;

        public UnityEFSession(ObjectContext context, ILoggingAdapter logger)
            : base(context)
        {
            this.logger = logger;
        }

        public override void SaveChanges()
        {
            try
            {
                if (logger != null) logger.DetectChanges();

                base.SaveChanges();

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void AfterSaveChanges()
        {
            try
            {
                if (logger != null) logger.CreateRevisions();
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
