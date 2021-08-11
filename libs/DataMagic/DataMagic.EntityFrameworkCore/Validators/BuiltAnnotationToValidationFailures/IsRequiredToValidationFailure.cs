using System.Collections.Generic;
using System.Linq;
using DataMagic.EntityFrameworkCore.Interfaces;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataMagic.EntityFrameworkCore.Validators.BuiltAnnotationToValidationFailures
{
    public class IsRequiredToValidationFailure : IRelationalEntityValidator
    {
        #region Methods

        public virtual bool AbleToValidate(IProperty property, object value)
        {
            if (!(property.ClrType == typeof(string)))
                return false;

            // Property is nullable
            if (property.IsNullable)
                return false;

            return true;
        }

        public virtual ValidationFailure ToValidationFailure(IProperty property, object value)
        {
            var requiredFailure = new ValidationFailure(property.Name,
                $"{property.Name} is required");
            requiredFailure.ErrorCode = "IS_REQUIRED";
            return requiredFailure;
        }

        #endregion
    }
}