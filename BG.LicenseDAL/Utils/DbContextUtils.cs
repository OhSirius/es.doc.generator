using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LicenseDAL.Utils
{
    //https://weblog.west-wind.com/posts/2013/Mar/26/Firing-an-Entity-Framework-Database-Initializer-from-within-DbContext
    public static class DbContextUtils<TContext>
        where TContext : DbContext
    {
        static object _InitializeLock = new object();
        static bool _InitializeLoaded = false;

        /// <summary>
        /// Method to allow running a DatabaseInitializer exactly once
        /// </summary>   
        /// <param name="initializer">A Database Initializer to run</param>
        public static void SetInitializer(IDatabaseInitializer<TContext> initializer = null)

        {
            if (_InitializeLoaded)
                return;

            // watch race condition
            lock (_InitializeLock)
            {
                // are we sure?
                if (_InitializeLoaded)
                    return;

                _InitializeLoaded = true;

                // force Initializer to load only once
                Database.SetInitializer<TContext>(initializer);
            }
        }
    }
}
