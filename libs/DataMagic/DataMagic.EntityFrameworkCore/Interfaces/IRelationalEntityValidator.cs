using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataMagic.EntityFrameworkCore.Interfaces
{
    /// <summary>
    ///     Read the annotation provided by entity framework and convert it to ValidationFailure in FluentValidation
    /// </summary>
    public interface IRelationalEntityValidator
    {
        #region Methods

        /// <summary>
        ///     Whether convention annotation can be validated or not.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool AbleToValidate(IProperty property, object value);

        /// <summary>
        ///     Convert annotation to validation failure.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ValidationFailure ToValidationFailure(IProperty property, object value);

        #endregion
    }
}