using DataMagic.EntityFrameworkCore.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace DataMagic.EntityFrameworkCore.Tests.TestingDb
{
    public class RelationalAnnotationValidatorDbContext : DbContext
    {
        #region Constructor

        public RelationalAnnotationValidatorDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion

        #region Properties

        public virtual DbSet<Student> Students { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasKey(x => x.Id);

            var student = modelBuilder.Entity<Student>();
            student.Property(x => x.Name).IsRequired();
            student.Property(x => x.Name).HasMaxLength(20);

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}