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
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<User>()
        //                .HasNoKey()
        //                .HasData(
        //                    new User { Id = 1, Name = "Name1", Birthday = DateTime.Now.ToOADate(  ) },
        //                    new User { Id = 2, Name = "Name2", Birthday = DateTime.Now.AddDays( -1 ).ToOADate() },
        //                    new User { Id = 3, Name = "Name3", Birthday = DateTime.Now.AddDays( -3 ).ToOADate() },
        //                    new User { Id = 4, Name = "Name4", Birthday = DateTime.Now.ToOADate() }
        //                );
        
        //}

        public virtual DbSet<User> Users { get; set; }
    }
}