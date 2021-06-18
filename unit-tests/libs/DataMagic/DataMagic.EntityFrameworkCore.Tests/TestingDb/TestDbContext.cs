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
            modelBuilder.Entity<User>()
                        .HasData(
                            new User { Id = 1, Name = "Name1", Birthday = Convert.ToDateTime("1-1-2015") , DeathTime = null},
                            new User { Id = 2, Name = "Name2", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2076") },
                            new User { Id = 3, Name = "Name3", Birthday = Convert.ToDateTime("1-1-2016"), DeathTime = Convert.ToDateTime("1-1-2096") },
                            new User { Id = 4, Name = "Name4", Birthday = Convert.ToDateTime("1-1-2017"), DeathTime = Convert.ToDateTime("1-1-2086") },
                            new User { Id = 5, Name = "Name5", Birthday = Convert.ToDateTime("1-1-2018"), DeathTime = Convert.ToDateTime("1-1-2086") }
                        );
            modelBuilder.Entity<Number>( )
                        .HasData(
                            new Number { Id = 1, Int = 1, Float = ( float ) 2.6 },
                            new Number { Id = 2, Int = 2, Float = ( float ) 7.3 },
                            new Number { Id = 3, Int = 2, Float = ( float ) 5.4 },
                            new Number { Id = 4, Int = 3, Float = ( float ) 5.4 }
                        );

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Number> Numbers { get; set; }
    }
}