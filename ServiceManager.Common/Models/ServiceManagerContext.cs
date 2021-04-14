using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceManager.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceManager.Common.Models
{
    public class ServiceManagerContext : IdentityDbContext<SystemUser, SystemRole, string>
    {
        public ServiceManagerContext() { }
        public ServiceManagerContext(DbContextOptions<ServiceManagerContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                const string TableNamePrefix = "AspNet";
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith(TableNamePrefix))
                    entityType.SetTableName(tableName.Substring(TableNamePrefix.Length));
            }

            //modelBuilder.Entity<SystemService>()
            //    .HasOne(x => x.Project)
            //    .WithMany(x => x.Services)
            //    .HasForeignKey(x => x.ProjectId);

        }

        public DbSet<SystemService> SystemService { get; set; }
        public DbSet<CommandRequest> CommandRequest { get; set; }
        public DbSet<CommandResponse> CommandResponse { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Machine> Machine { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.GetConnectionString(nameof(ServiceManagerContext)));
            }
        }
    }
}
