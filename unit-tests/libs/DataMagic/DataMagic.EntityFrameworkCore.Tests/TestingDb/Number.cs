using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataMagic.EntityFrameworkCore.Tests.TestingDb
{
    public class Number
    {
        [Key] public int Id { get; set; }

        public int Int { get; set; }
        public float Float { get; set; }
    }

    public class StudentConfiguration : IEntityTypeConfiguration<Number>
    {
        public void Configure(EntityTypeBuilder<Number> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        }
    }
}