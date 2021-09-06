using System;
using System.Linq.Expressions;
using DataMagic.Abstractions.Enums.Operators;
using DataMagic.Abstractions.Models.Filters;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataMagic.MongoDatabase.Extensions
{
    public static class TextSearchExtensions
    {
        #region Methods

        public static FilterDefinition<T> WithTextSearch<T>(this FilterDefinitionBuilder<T> builder,
            FieldDefinition<T, string> field,
            TextFilter textFilter)
        {
            switch (textFilter.Mode)
            {
                case TextFilterModes.Contains:
                    return builder.Regex(field, new BsonRegularExpression($".*{textFilter.Value}.*"));

                case TextFilterModes.Equal:
                    return builder.Eq(field, textFilter.Value);

                case TextFilterModes.EndWith:
                    return builder.Regex(field, new BsonRegularExpression($"{textFilter.Value}$"));

                case TextFilterModes.StartWith:
                    return builder.Regex(field, new BsonRegularExpression($"^{textFilter.Value}"));

                case TextFilterModes.EqualIgnoreCase:
                    return builder.Regex(field, new BsonRegularExpression($"^{textFilter.Value}$", "i"));

                default:
                    return FilterDefinition<T>.Empty;
            }
        }

        public static FilterDefinition<T> WithTextSearch<T>(this FilterDefinitionBuilder<T> builder,
            Expression<Func<T, object>> field,
            TextFilter textFilter)
        {
            if (textFilter == null)
                return FilterDefinition<T>.Empty;

            switch (textFilter.Mode)
            {
                case TextFilterModes.Contains:
                    return builder.Regex(field, new BsonRegularExpression($".*{textFilter.Value}.*"));

                case TextFilterModes.Equal:
                    return builder.Eq(field, textFilter.Value);

                case TextFilterModes.EndWith:
                    return builder.Regex(field, new BsonRegularExpression($"{textFilter.Value}$"));

                case TextFilterModes.StartWith:
                    return builder.Regex(field, new BsonRegularExpression($"^{textFilter.Value}"));

                case TextFilterModes.EqualIgnoreCase:
                    return builder.Regex(field, new BsonRegularExpression($"^{textFilter.Value}$", "i"));

                default:
                    return FilterDefinition<T>.Empty;
            }
        }

        #endregion
    }
}