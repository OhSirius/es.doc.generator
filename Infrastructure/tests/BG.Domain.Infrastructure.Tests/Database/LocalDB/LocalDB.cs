using BG.Domain.Infrastructure.Tests.Database.LocalDB;
using BG.Domain.Infrastructure.Tests.Database.LocalDB;
using BG.LicenseDAL.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.Infrastructure.Tests.Database.LocalDB
{
    public class LocalDB
    {
        public void InitDatabase()
        {
            var con = new LicenseContext();
            con.Database.ExecuteSqlCommand(LocalDBScripts.AuthDB);
        }
        public void ReseedTables()
        {
            var con = new LicenseContext();
            con.Database.ExecuteSqlCommand(LocalDBScripts.AuthDBReseed);
        }

        public void InitLists()
        {
            var con = new LicenseContext();
            con.Database.ExecuteSqlCommand(LocalDBScripts.AuthDBListsInsert);
        }

    }
}
