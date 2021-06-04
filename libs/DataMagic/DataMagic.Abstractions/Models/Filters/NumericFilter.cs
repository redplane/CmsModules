using System;
using System.Linq;
using DataMagic.Abstractions.Enums;
using DataMagic.Abstractions.Enums.Operators;

namespace DataMagic.Abstractions.Models.Filters
{
    public class NumericFilter<T>
    {
        #region Properties

        /// <summary>
        /// Numeric value to be searched for.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Numeric comparision mode.
        /// </summary>
        public NumericComparisonOperators Operator { get; private set; }

        #endregion

        #region Constructors

        public NumericFilter(T value, NumericComparisonOperators @operator)
        {
            Value = value;
            Operator = @operator;

            // Get the supported value types.
            var valueTypes = ValidNumericTypes();
            if (!valueTypes.Any(valueType => valueTypes.Contains(valueType)))
            {
	            var szValueTypes = string.Join(',', valueTypes.Select(x => x.Name));
                throw new ArgumentException($"Must be one of these data type: {szValueTypes}", nameof(value));
            }
        }

        #endregion

        #region Methods

        public static Type[] ValidNumericTypes()
        {
	        var validTypes = new[]
	        {
		        typeof(byte), typeof(sbyte), typeof(decimal), typeof(double), typeof(float), typeof(int),
		        typeof(uint), typeof(long), typeof(ulong), typeof(short), typeof(ushort),

		        typeof(byte?), typeof(sbyte?), typeof(decimal?), typeof(double?), typeof(float?), typeof(int?),
		        typeof(uint?), typeof(long?), typeof(ulong?), typeof(short?), typeof(ushort?)
	        };

	        return validTypes;
        }

        #endregion
    }
}
