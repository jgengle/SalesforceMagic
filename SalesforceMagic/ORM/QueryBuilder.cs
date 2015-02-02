﻿using System;
using System.Linq.Expressions;
using SalesforceMagic.Entities;
using SalesforceMagic.Extensions;
using SalesforceMagic.LinqProvider;

namespace SalesforceMagic.ORM
{
    public static class QueryBuilder
    {
        public static string GenerateQuery<T>(int limit = default(int))
            where T : SObject
        {
            Type type = typeof(T);
            string query = CompileSelectStatements(type);
            if (limit != default(int)) AddQueryLimit(ref query, limit);

            return query;
        }

        public static string GenerateQuery<T>(Expression<Func<T, bool>> predicate, int limit = default(int))
            where T : SObject
        {
            Type type = typeof(T);
            string query = CompileSelectStatements(type);
            if (predicate != null) AddConditionsSet(ref query, predicate);
            if (limit != default(int)) AddQueryLimit(ref query, limit);

            return query;
        }

        private static string CompileSelectStatements(Type type)
        {
            return string.Format("SELECT {0} FROM {1}", string.Join(", ", type.GetPropertyNames(true)), type.GetName());
        }

        private static void AddConditionsSet<T>(ref string query, Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
                query = query + " WHERE " + SOQLVisitor.ConvertToSOQL(predicate);
        }

        private static void AddQueryLimit(ref string query, int limit)
        {
            query = string.Format("{0} LIMIT {1}", query, limit);
        }
    }
}