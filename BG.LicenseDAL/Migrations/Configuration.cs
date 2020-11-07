using BG.LicenseDAL.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LicenseDAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<LicenseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LicenseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            //var mainServer = new Server { IP = "virt302", Login = "crm_dev", Password = "CRM_dev3" };
            //var loggingServer = new Server { IP = "virt303", Login = "crm_dev", Password = "CRM_dev3" };

            //context.Servers.AddOrUpdate(p => p.Id, mainServer, loggingServer);

            //var defaultQueue = new Queue() { Name = CronRepository.DefaultQueue, Type = Models.Type.Topic, RoutingKey = "", BindingKeys = "", Server = mainServer };
            //var worksSheetsGenerationQueue = new Queue() { Name = "WorksSheetsGenerationQueue", Type = Models.Type.Topic, RoutingKey = "", BindingKeys = "", Server = mainServer };
            //context.Queues.AddOrUpdate(q=>q.Id,)


        }
    }

}
