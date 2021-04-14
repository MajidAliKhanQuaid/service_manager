using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ServiceManager.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceManager.Common.Models
{
    public class ServiceManagerDesignTimeContext : IDesignTimeDbContextFactory<ServiceManagerContext>
    {
        public ServiceManagerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServiceManagerContext>();
            optionsBuilder.UseSqlServer(Config.GetConnectionString(nameof(ServiceManagerContext)));

            return new ServiceManagerContext(optionsBuilder.Options);
        }
    }
}
