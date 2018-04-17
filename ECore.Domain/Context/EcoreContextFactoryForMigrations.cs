using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECore.Domain.Context
{
    /// <summary>
    /// This class is needed to allow Add-Migrations command to be run. 
    /// see https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext#using-idesigntimedbcontextfactorytcontext
    /// </summary>
    public class EcoreContextFactoryForMigrations : IDesignTimeDbContextFactory<EcoreDbContext>
    {
        private const string ConnectionString =
            "Server=(localdb)\\mssqllocaldb;Database=ecore_db;Trusted_Connection=True;MultipleActiveResultSets=true";

        public EcoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EcoreDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString,
                b => b.MigrationsAssembly("ECore.Domain"));

            return new EcoreDbContext(optionsBuilder.Options);
        }
    }
}
