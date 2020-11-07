using BG.LicenseDAL.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LicenseDAL.Models.Context
{
    class ContextInitializer : MigrateDatabaseToLatestVersion<LicenseContext, Configuration>
    {

    }
}
