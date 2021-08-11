using DataMagic.Abstractions.Enums.Operators;

namespace DataMagic.Abstractions.Models.Filters
{
    public class TextFilter
    {
        #region Properties

        /// <summary>
        /// Text to be searched.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Operator which will be applied to text filter operation.
        /// </summary>
        public TextComparisonOperators Operator { get; set; }

        #endregion
    }
}