using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using DataMagic.Abstractions.Interfaces;

namespace DataMagic.LiteDatabase.Models
{
    public class EntityUpdateBuilder<T>
    {
        #region Properties

        private readonly LinkedList<FieldUpdate> _fieldUpdates;

        #endregion

        #region Constructor

        public EntityUpdateBuilder()
        {
            _fieldUpdates = new LinkedList<FieldUpdate>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update property base on editable field.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="model"></param>
        /// <typeparam name="TField"></typeparam>
        /// <returns></returns>
        public virtual EntityUpdateBuilder<T> Update<TField>(Expression<Func<T, TField>> field,
            IEditableField<TField> model)
        {
            if (model == null)
                return this;

            if (!model.HasModified)
                return this;

            var fieldUpdate = new FieldUpdate(field, model.Value);
            _fieldUpdates.AddLast(fieldUpdate);
            return this;
        }

        /// <summary>
        /// Build the update definition.
        /// </summary>
        /// <returns></returns>
        public virtual void Build(T entity)
        {
            if (_fieldUpdates == null || _fieldUpdates.Count < 1)
                return;

            foreach (var fieldUpdate in _fieldUpdates)
            {
                var expression = fieldUpdate.Expression;
                var memberExpression = (MemberExpression)expression.Body;
                var propertyInfo = memberExpression.Member as PropertyInfo;
                propertyInfo!.SetValue(entity, fieldUpdate.Value);
            }
        }

        /// <summary>
        /// Get the number of updated fields.
        /// </summary>
        /// <returns></returns>
        public virtual int CountUpdatedFields()
        {
            return _fieldUpdates.Count;
        }

        #endregion
    }
}