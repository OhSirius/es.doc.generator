using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BG.Infrastructure.Process.NCommon.Data
{
    public class CustomUnitOfWorkSettings
    {
        public bool AutoCompleteScope { get; set; }
        public IsolationLevel DefaultIsolation { get; set; }
    }
}
