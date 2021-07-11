using System;
using DataMagic.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace DataMagic.EntityFrameworkCore.Tests.TestingDb
{
    public class TestDbContext: DbContext
    {
        public TestDbContext() { }

        public TestDbContext( DbContextOptions<TestDbContext> options )
            : base( options )
        {
         
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestDbContext).Assembly);
           
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Number> Numbers { get; set; }
    }
}