using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Demo.Framework.Data
{
    public static class RepositoryExtensions
    {

        public static IQueryable<TEntity> Get<TEntity>(this IRepository<TEntity> _repository, Expression<Func<TEntity, bool>> expression)
        {
            var q = _repository.Table.Where(expression);
            return q;
        }

        public static IQueryable<TEntity> Get<TEntity>(this IRepository<TEntity> _repository, Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> order)
        {
            var query = _repository.Table;
            if (expression != null)
                query = query.Where(expression);
            if (order != null)
            {
                query = order(query);
            }

            return query;
        }


        public static long GetCount<TEntity>(this IRepository<TEntity> _repository, Expression<Func<TEntity, bool>> expression)
        {
            var q = _repository.Table;
            if (expression != null)
                q = q.Where(expression);
            long count = q.LongCount();
            return count;
        }

        public static long GetCount<TEntity>(this IRepository<TEntity> _repository)
        {
            return GetCount(_repository,null);
        }
    }


    public static class DbSetExtensions
    {
        public static DbQuery<T> Include<T>(this DbQuery<T> query, Expression<Func<T, object>> selector) where T : class
        {
            string path = new PropertyPathVisitor().GetPropertyPath(selector);
            return query.Include(path);
        }

        class PropertyPathVisitor : ExpressionVisitor
        {
            private Stack<string> _stack;
            public string GetPropertyPath(Expression expression)
            {
                _stack = new Stack<string>();
                Visit(expression);
                return _stack
                    .Aggregate(
                       new StringBuilder(),
                       (sb, name) =>
                           (sb.Length > 0 ? sb.Append(".") : sb).Append(name))
                   .ToString();
            }

            protected override Expression VisitMember(MemberExpression expression)
            {
                if (_stack != null)
                    _stack.Push(expression.Member.Name);
                return base.VisitMember(expression);
            }

            protected override Expression VisitMethodCall(MethodCallExpression expression)
            {
                if (IsLinqOperator(expression.Method))
                {
                    for (int i = 1; i < expression.Arguments.Count; i++)
                    {
                        Visit(expression.Arguments[i]);
                    }
                    Visit(expression.Arguments[0]);
                    return expression;
                }
                return base.VisitMethodCall(expression);
            }

            private static bool IsLinqOperator(MethodInfo method)
            {
                if (method.DeclaringType != typeof(Queryable) && method.DeclaringType != typeof(Enumerable))
                    return false;
                return Attribute.GetCustomAttribute(method, typeof(ExtensionAttribute)) != null;
            }
        }
    }
}
