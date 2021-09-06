using DataMagic.EntityFrameworkCore.Interfaces;
using DataMagic.EntityFrameworkCore.Tests.Models;
using DataMagic.EntityFrameworkCore.Tests.TestingDb;
using DataMagic.EntityFrameworkCore.Validators;
using DataMagic.EntityFrameworkCore.Validators.BuiltAnnotationToValidationFailures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataMagic.EntityFrameworkCore.Tests.Validators
{
    [TestFixture]
    public partial class RelationalAnnotationValidatorTests
    {
        [SetUp]
        public void SetUp()
        {
            _services.Clear();

            _services.AddDbContext<RelationalAnnotationValidatorDbContext>(options =>
                options.UseSqlite("DataSource=file::memory:?cache=shared"));
            _services.AddSingleton<RelationalAnnotationValidatorDbContext>();
            _services.AddScoped(provider =>
            {
                var dbContext = provider.GetService<RelationalAnnotationValidatorDbContext>();
                var annotationToValidationFailures = provider.GetServices<IRelationalEntityValidator>();
                return new RelationalAnnotationValidator<Student>(dbContext, annotationToValidationFailures);
            });
            _services.AddScoped(provider =>
            {
                var dbContext = provider.GetService<RelationalAnnotationValidatorDbContext>();
                var annotationToValidationFailures = provider.GetServices<IRelationalEntityValidator>();
                return new RelationalAnnotationValidator<Teacher>(dbContext, annotationToValidationFailures);
            });

            _services.AddScoped<IRelationalEntityValidator, MaxLengthToValidationFailure>();
            _services.AddScoped<IRelationalEntityValidator, IsRequiredToValidationFailure>();
        }

        [TearDown]
        public void TearDown()
        {
            _services.Clear();
        }

        private readonly IServiceCollection _services;

        public RelationalAnnotationValidatorTests()
        {
            _services = new ServiceCollection();
        }
    }
}