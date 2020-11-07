using BG.Infrastructure.Process.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Infrastructure.Common.MEF;
using BG.Infrastructure.Process.Identity;

namespace BG.Infrastructure.Process.Process.Impl
{
    public class DefaultBusinessProcessFactory : IBusinessProcessFactory
    {
        private readonly IUser _currentUser;

        public DefaultBusinessProcessFactory(IUser currentUser)
        {
            this._currentUser = currentUser;
        }

        public TBusinessProcess CreateBusinessProcess<TBusinessProcess>(string entityName, EntityType entityType, string configurationName, BusinessProcessParams @params = null)
            where TBusinessProcess : IBusinessProcess
        {
            var process = BusinessProcessConfuguration.Default.CreateBusinessProcess<TBusinessProcess>(
                _currentUser, entityName, entityType, configurationName, @params);
            return process;

        }


    }
}
