using EngagerMark4.ApplicationCore.Cris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Infrasturcture.ExpressionBuilders
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<Entity,bool>> GetEqualExpression<Entity>(string columnName,object value)
        {
            ParameterExpression pe = Expression.Parameter(typeof(Entity), "param");
            MemberExpression left = Expression.Property(pe, columnName);
            ConstantExpression right = Expression.Constant(value);
            var predicate = Expression.Lambda<Func<Entity, bool>>(Expression.Equal(left, right), new[] { pe });
            return predicate;
        }

        public static Expression<Func<Entity, bool>> GetGreaterThanExpression<Entity>(string columnName, object value)
        {
            ParameterExpression pe = Expression.Parameter(typeof(Entity), "param");
            MemberExpression left = Expression.Property(pe, columnName);
            ConstantExpression right = Expression.Constant(value);
            var predicate = Expression.Lambda<Func<Entity, bool>>(Expression.GreaterThan(left, right), new[] { pe });
            return predicate;
        }

        public static Expression<Func<Entity, bool>> GetGreaterThanOrEqualExpression<Entity>(string columnName, object value)
        {
            ParameterExpression pe = Expression.Parameter(typeof(Entity), "param");
            MemberExpression left = Expression.Property(pe, columnName);
            ConstantExpression right = Expression.Constant(value);
            var predicate = Expression.Lambda<Func<Entity, bool>>(Expression.GreaterThanOrEqual(left, right), new[] { pe });
            return predicate;
        }

        public static Expression<Func<Entity, bool>> GetLessThanExpression<Entity>(string columnName, object value)
        {
            ParameterExpression pe = Expression.Parameter(typeof(Entity), "param");
            MemberExpression left = Expression.Property(pe, columnName);
            ConstantExpression right = Expression.Constant(value);
            var predicate = Expression.Lambda<Func<Entity, bool>>(Expression.LessThan(left, right), new[] { pe });
            return predicate;
        }

        public static Expression<Func<Entity, bool>> GetLessThanOrEqualExpression<Entity>(string columnName, object value)
        {
            ParameterExpression pe = Expression.Parameter(typeof(Entity), "param");
            MemberExpression left = Expression.Property(pe, columnName);
            ConstantExpression right = Expression.Constant(value);
            var predicate = Expression.Lambda<Func<Entity, bool>>(Expression.LessThanOrEqual(left, right), new[] { pe });
            return predicate;
        }

        public static Expression<Func<Entity, bool>> GetContainsExpression<Entity>(string columnName, object value)
        {
            MethodInfo contains = typeof(string).GetMethod("Contains");
            ParameterExpression pe = Expression.Parameter(typeof(Entity), "param");
            MemberExpression left = Expression.Property(pe, columnName);
            ConstantExpression right = Expression.Constant(value, typeof(string));
            var predicate = Expression.Lambda<Func<Entity, bool>>(Expression.Call(left, contains, right), new[] { pe });
            return predicate;
        }

        public static Expression<Func<Entity, bool>> GetStartsWithExpression<Entity>(string columnName, object value)
        {
            MethodInfo startsWith = typeof(string).GetMethod("StartsWith");
            ParameterExpression pe = Expression.Parameter(typeof(Entity), "param");
            MemberExpression left = Expression.Property(pe, columnName);
            ConstantExpression right = Expression.Constant(value, typeof(string));
            var predicate = Expression.Lambda<Func<Entity, bool>>(Expression.Call(left, startsWith, right), new[] { pe });
            return predicate;
        }

        public static Expression<Func<Entity, bool>> GetEndsWithExpression<Entity>(string columnName, object value)
        {
            MethodInfo endsWith = typeof(string).GetMethod("EndsWith");
            ParameterExpression pe = Expression.Parameter(typeof(Entity), "param");
            MemberExpression left = Expression.Property(pe, columnName);
            ConstantExpression right = Expression.Constant(value, typeof(string));
            var predicate = Expression.Lambda<Func<Entity, bool>>(Expression.Call(left, endsWith, right), new[] { pe });
            return predicate;
        }

        public static Expression<Func<Entity, string>> GetExpression<Entity>(string propertyName,BaseCri.DataType dataType)
        {
            var param = Expression.Parameter(typeof(Entity), "x");
            switch (dataType)
            {
                case BaseCri.DataType.String:
                    Expression conversion = Expression.Convert(Expression.Property
                     (param, propertyName), typeof(string));   //important to use the Expression.Convert
                    return Expression.Lambda<Func<Entity, string>>(conversion, param);
                case BaseCri.DataType.Int64:
                    Expression conversion3 = Expression.Convert(Expression.Property
                     (param, propertyName), typeof(Int64));   //important to use the Expression.Convert
                    return Expression.Lambda<Func<Entity, string>>(conversion3, param);
                default:
                    Expression conversion2 = Expression.Convert(Expression.Property
                     (param, propertyName), typeof(string));   //important to use the Expression.Convert
                    return Expression.Lambda<Func<Entity, string>>(conversion2, param);
            }
            
        }

        public static Expression<Func<Entity, Int64>> GetExpressionInt64<Entity>(string propertyName)
        {
            var param = Expression.Parameter(typeof(Entity), "x");
            Expression conversion = Expression.Convert(Expression.Property
                     (param, propertyName), typeof(Int64));   //important to use the Expression.Convert
            return Expression.Lambda<Func<Entity, Int64>>(conversion, param);
        }

        public static Expression<Func<Entity, dataType>> GetExpression<Entity,dataType>(string propertyName)
        {
            var param = Expression.Parameter(typeof(Entity), "x");
            Expression conversion = Expression.Convert(Expression.Property
                     (param, propertyName), typeof(dataType));   //important to use the Expression.Convert
            return Expression.Lambda<Func<Entity, dataType>>(conversion, param);
        }
    }
}
