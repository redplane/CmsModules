using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataMagic.Abstractions.Interfaces;
using MongoDB.Driver;

namespace DataMagic.MongoDatabase.Models
{
    public class EntityUpdateBuilder<T>
    {
        #region Properties

        private readonly LinkedList<UpdateDefinition<T>> _definitions;

        #endregion
        
        #region Constructor

        public EntityUpdateBuilder()
        {
            _definitions = new LinkedList<UpdateDefinition<T>>();
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
        public virtual EntityUpdateBuilder<T> Update<TField>(FieldDefinition<T, TField> field, IEditableField<TField> model)
        {
            if (!model.HasModified)
                return this;

            var updateDefinition = Builders<T>.Update.Set(field, model.Value);
            _definitions.AddLast(updateDefinition);
            return this;
        }
        
        /// <summary>
        /// Update property base on editable field.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="model"></param>
        /// <typeparam name="TField"></typeparam>
        /// <returns></returns>
        public virtual EntityUpdateBuilder<T> Update<TField>(Expression<Func<T, TField>> field, IEditableField<TField> model)
        {
            if (!model.HasModified)
                return this;

            var updateDefinition = Builders<T>.Update.Set(field, model.Value);
            _definitions.AddLast(updateDefinition);
            return this;
        }

        /// <summary>
        /// Build the update definition.
        /// </summary>
        /// <returns></returns>
        public virtual UpdateDefinition<T> Build()
        {
            if (_definitions == null || _definitions.Count < 1)
                return null;

            return Builders<T>.Update.Combine(_definitions);
        }

        /// <summary>
        /// Get the number of updated fields.
        /// </summary>
        /// <returns></returns>
        public virtual int CountUpdatedFields()
        {
            return _definitions.Count;
        }
        
        #endregion
    }
}