using System;
using System.Linq;
using DataMagic.EntityFrameworkCore.Tests.Models;
using DataMagic.EntityFrameworkCore.Tests.TestingDb;
using DataMagic.EntityFrameworkCore.Validators;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataMagic.EntityFrameworkCore.Tests.Validators
{
    public partial class RelationalAnnotationValidatorTests
    {
        #region Methods

        [Test]
        public void Validate_PropertyIsRequiredButNull_RequireValidationFailureReturned()
        {
            var serviceProvider = _services.BuildServiceProvider();
            var studentValidator = serviceProvider.GetService<RelationalAnnotationValidator<Student>>()!;
            var dbContext = serviceProvider.GetService<RelationalAnnotationValidatorDbContext>();
            dbContext.Database.EnsureCreated();

            var student = new Student(Guid.NewGuid());
            
            // Do the validation and get the validation result.
            var validationResult = studentValidator.Validate(student);
            Assert.NotNull(validationResult);

            var requireValidationFailure = validationResult.Errors
                .FirstOrDefault(x => x.ErrorCode == "IS_REQUIRED");
            
            Assert.NotNull(requireValidationFailure);
            Assert.AreEqual(nameof(Student.Name), requireValidationFailure.PropertyName);
        }
        
        [Test]
        public void Validate_PropertyValueExceedMaxLength_MaxLengthValidationFailureReturned()
        {
            var serviceProvider = _services.BuildServiceProvider();
            var studentValidator = serviceProvider.GetService<RelationalAnnotationValidator<Student>>()!;
            var dbContext = serviceProvider.GetService<RelationalAnnotationValidatorDbContext>();
            dbContext.Database.EnsureCreated();

            var student = new Student(Guid.NewGuid());
            student.Name = "0123456789012345678901";
            
            // Do the validation and get the validation result.
            var validationResult = studentValidator.Validate(student);
            Assert.NotNull(validationResult);

            var requireValidationFailure = validationResult.Errors
                .FirstOrDefault(x => x.ErrorCode == "MAX_LENGTH_EXCEEDED");
            
            Assert.NotNull(requireValidationFailure);
            Assert.AreEqual(nameof(Student.Name), requireValidationFailure.PropertyName);
        }

        #endregion
    }
}