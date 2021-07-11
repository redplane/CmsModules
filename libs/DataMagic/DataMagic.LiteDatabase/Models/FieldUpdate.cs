using System.Linq.Expressions;

namespace DataMagic.LiteDatabase.Models
{
    public class FieldUpdate
    {
        #region Constructor

        public FieldUpdate(LambdaExpression field, object value)
        {
            Expression = field;
            Value = value;
        }

        #endregion

        #region Accessors

        public LambdaExpression Expression { get; }

        public object Value { get; }

        #endregion
    }
}