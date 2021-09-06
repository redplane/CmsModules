using System.Linq;
using DataMagic.EntityFrameworkCore.Interfaces;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataMagic.EntityFrameworkCore.Validators.BuiltAnnotationToValidationFailures
{
    public class MaxLengthToValidationFailure : IRelationalEntityValidator
    {
        #region Properties

        private readonly int _maxLength = 0;

        #endregion

        #region Methods

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool AbleToValidate(IProperty property, object value)
        {
            if (!(property.ClrType == typeof(string)))
                return false;

            // Get the annotations.
            var maxLengthAnnotation = property.GetAnnotations()
                .FirstOrDefault(x => x.Name.Equals("MaxLength"));

            // No max length annotation is found.
            if (maxLengthAnnotation == null)
                return false;

            var actualValue = (string)value;
            if (maxLengthAnnotation.Value is not int maxLength || string.IsNullOrEmpty(actualValue))
                return false;

            if (actualValue.Length <= maxLength)
                return false;

            return true;
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ValidationFailure ToValidationFailure(IProperty property, object value)
        {
            var maxLengthFailure = new ValidationFailure(property.Name,
                $"{property.Name}'s length cannot exceed {_maxLength} character(s)", value);
            maxLengthFailure.ErrorCode = "MAX_LENGTH_EXCEEDED";
            return maxLengthFailure;
        }

        #endregion
    }
}