using KiraNet.AlasFx.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraNet.AlasFx.Test.domain
{
    public class TestDbContext : DbContextBase
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<DbTest> DbTests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<DbTest>(new DbTestConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class TestDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlServer("xxx");

            return new TestDbContext(optionsBuilder.Options);
        }
    }
}
