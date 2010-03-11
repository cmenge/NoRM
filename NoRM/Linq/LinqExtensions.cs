﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NoRM.BSON;
using NoRM.Protocol.Messages;
using NoRM.Responses;

namespace NoRM.Linq
{
    /// <summary>
    /// Linq extensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Gets the constant value.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <returns>The get constant value.</returns>
        public static object GetConstantValue(this Expression exp)
        {
            object result = null;
            if (exp is ConstantExpression)
            {
                var c = (ConstantExpression) exp;
                result = c.Value;
            }

            return result;
        }

        /// <summary>
        /// Asks Mongo for an explain plan for a linq query.
        /// </summary>
        /// <typeparam name="T">Type to explain</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>Query explain plan</returns>
        public static ExplainResponse Explain<T>(this IQueryable<T> expression)
        {
            var translator = new MongoQueryTranslator();
            translator.Translate(expression.Expression);

            if (expression is MongoQuery<T>)
            {
                return (expression as MongoQuery<T>).Explain(translator.FlyWeight);
            }

            return null;
        }

        public static IEnumerable<T> Hint<T>(this IEnumerable<T> find, Expression<Func<T, object>> hint, IndexOption direction)
        {
            var translator = new MongoQueryTranslator();
            var index = translator.Translate(hint);

            var proxy = (MongoQueryExecutor<T, Flyweight>)find;
            proxy.AddHint(index, direction);
            return find;
        }
    }
}
