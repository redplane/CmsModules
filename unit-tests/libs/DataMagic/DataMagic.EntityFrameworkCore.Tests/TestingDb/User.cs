using System;
using System.ComponentModel.DataAnnotations;
using DataMagic.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMagic.EntityFrameworkCore.Tests.TestingDb
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime? DeathTime { get; set; }
    }
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(128);

        }
    }
}