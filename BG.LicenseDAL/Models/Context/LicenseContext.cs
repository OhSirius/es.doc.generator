using BG.LicenseDAL.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LicenseDAL.Models.Context
{
    public partial class LicenseContext : DbContext
    {
        public LicenseContext() : base("BGLicenseContext")
        {
            //Использовать автоматическую миграцию не совсем целесообразно при первой инициализации БД, т.к. она зависит от назначения БД (разработка, тестирование, продакшен)
            //Database.SetInitializer(new CronInitializer());
            DbContextUtils<LicenseContext>.SetInitializer(new ContextInitializer());
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<LicenseType> LicenseTypes { get; set; }
        public DbSet<LogonHistory> LogonHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Student>()
            //    .HasMany(s => s.Enrollments)
            //    .WithRequired(e => e.Student)
            //    .HasForeignKey(e => e.StudentID);

            //modelBuilder.Entity<Course>()
            //    .HasMany(c => c.Enrollments)
            //    .WithRequired(e => e.Course)
            //    .HasForeignKey(e => e.CourseID);

            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            //modelBuilder.Entity<LicenseType>()
            //.HasOptional(s => s.Application)
            //.WithMany()
            //.WillCascadeOnDelete(false);
        }
    }
}
