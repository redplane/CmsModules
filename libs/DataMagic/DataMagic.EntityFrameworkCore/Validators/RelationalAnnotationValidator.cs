using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataMagic.EntityFrameworkCore.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace DataMagic.EntityFrameworkCore.Validators
{
    public class RelationalAnnotationValidator<TEntity> : AbstractValidator<TEntity> where TEntity : class
    {
        #region Properties

        private readonly DbContext _dbContext;

        private readonly IEnumerable<IRelationalEntityValidator> _entityValidators;

        #endregion

        #region Constructor

        public RelationalAnnotationValidator(DbContext dbContext,
            IEnumerable<IRelationalEntityValidator> entityValidators)
        {
            _dbContext = dbContext;
            _entityValidators = entityValidators;
        }

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ValidationResult Validate(ValidationContext<TEntity> context)
        {
            // Get the entity db set.
            var dbSet = _dbContext.Set<TEntity>();
            if (dbSet == null)
                return new ValidationResult();

            var entityType = dbSet.EntityType;

            if (entityType == null)
                return new ValidationResult();

            var instanceType = context.InstanceToValidate?.GetType();
            if (instanceType == null)
                return new ValidationResult();

            var properties = entityType.GetProperties()?.ToArray();
            if (properties == null || properties.Length < 1)
                return new ValidationResult();

            // List of validation failure.
            var validationFailures = new LinkedList<ValidationFailure>();

            foreach (var property in properties)
            {
                // Get the actual value which is currently assigned to entity.
                var actualValue = instanceType.GetProperty(property.Name)?.GetMethod
                    ?.Invoke(context.InstanceToValidate, Array.Empty<object>());

                var annotations = property.GetAnnotations()?.ToArray();
                if (annotations == null || annotations.Length < 1)
                    continue;

                foreach (var annotationToValidationFailure in _entityValidators)
                {
                    // Not available to validate.
                    if (!annotationToValidationFailure.AbleToValidate(property, actualValue))
                        continue;

                    var validationFailure =
                        annotationToValidationFailure.ToValidationFailure(property, actualValue);
                    validationFailures.AddLast(validationFailure);
                }
            }

            return new ValidationResult(validationFailures);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public override Task<ValidationResult> ValidateAsync(ValidationContext<TEntity> context,
            CancellationToken cancellation = default)
        {
            var validationResult = Validate(context);
            return Task.FromResult(validationResult);
        }

        #endregion
    }
}