using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    public static class DynamicLinq
    {
        /// <summary>  
        /// 创建lambda中的参数,即c=>c.xxx==xx 中的c  
        /// </summary>  
        public static ParameterExpression CreateLambdaParam<T>(string name)
        {
            return Expression.Parameter(typeof(T), name);
        }

        public static Expression<Func<T,object>> GenerateSingleLambda<T>(string property)
        {
            var parameter = Expression.Parameter(typeof(T), "t");
            var express = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(parameter, property), typeof(object)), parameter);
            return express;
            //ParameterExpression product = Expression.Parameter(typeof(T));
            //var expr = Expression.Property(product,memberName);
            //Expression<Func<T, string>> orderExpression = Expression.Lambda<Func<T, string>>(expr, product);
            //return orderExpression;

            //ParameterExpression[] typeParams = new ParameterExpression[] { Expression.Parameter(typeof(T), "c") };
            //PropertyInfo pi = typeof(T).GetProperty(memberName);
            //if(pi == null)
            //    throw new Exception("要排序的字段不属于该实体");

            //LambdaExpression orderyLambda = Expression.Lambda(Expression.Property(typeParams[0], pi), typeParams);
            //Expression<Func<T,string>> result = (Expression<Func<T,string>>)orderyLambda;
            //return result;
        }

        /// <summary>  
        /// 创建linq表达示的body部分,即c=>c.xxx==xx 中的c.xxx==xx  
        /// </summary>  
        public static Expression GenerateBody<T>(this ParameterExpression param, FilterSearch filterObj)
        {
            PropertyInfo property = typeof(T).GetProperty(filterObj.Key);

            //组装左边  
            Expression left = Expression.Property(param, property);
            //组装右边  
            Expression right = null;
 
            if (property.PropertyType == typeof(int))
            {
                right = Expression.Constant(int.Parse(filterObj.Value));
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                right = Expression.Constant(DateTime.Parse(filterObj.Value));
            }
            else if (property.PropertyType == typeof(string))
            {
                right = Expression.Constant((filterObj.Value));
            }
            else if (property.PropertyType == typeof(decimal))
            {
                right = Expression.Constant(decimal.Parse(filterObj.Value));
            }
            else if (property.PropertyType == typeof(Guid))
            {
                right = Expression.Constant(Guid.Parse(filterObj.Value));
            }
            else if (property.PropertyType == typeof(bool))
            {
                right = Expression.Constant(filterObj.Value.Equals("1"));
            }
            //枚举类型
            else if (property.PropertyType.BaseType == typeof(Enum))
            {
                right = Expression.Constant(Enum.Parse(property.PropertyType, filterObj.Value));
            }
            else
            {
                throw new Exception("暂不能解析该Key的类型");
            }
            
            Expression filter = Expression.Equal(left, right);
            switch (filterObj.Contract)
            {
                case FilterSearchContract.等于:
                    filter = Expression.Equal(left, right);
                    break;
                case FilterSearchContract.不等于:
                    filter = Expression.NotEqual(left, right);
                    break;
                case FilterSearchContract.大于:
                    filter = Expression.GreaterThan(left, right);
                    break;
                case FilterSearchContract.大于等于:
                    filter = Expression.GreaterThanOrEqual(left, right);
                    break;
                case FilterSearchContract.小于:
                    filter = Expression.LessThan(left, right);
                    break;
                case FilterSearchContract.小于等于:
                    filter = Expression.LessThanOrEqual(left, right);
                    break;
                case FilterSearchContract.like:
                    filter = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                                 Expression.Constant(filterObj.Value));
                    break;
                case FilterSearchContract.notlike:
                    filter = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                                 Expression.Constant(filterObj.Value)));
                    break;
                    
                default:
                    filter = Expression.Equal(left, right);
                    break;
            }
            return filter;
        }

        /// <summary>  
        /// 创建完整的lambda,即c=>c.xxx==xx  
        /// </summary>  
        public static LambdaExpression GenerateLambda(this ParameterExpression param, Expression body)
        {
            return Expression.Lambda(body, param);
        }

        /// <summary>  
        /// 创建完整的lambda，为了兼容EF中的where语句  
        /// </summary>  
        public static Expression<Func<T, bool>> GenerateTypeLambda<T>(this ParameterExpression param, Expression body)
        {
            return (Expression<Func<T, bool>>)(param.GenerateLambda(body));
        }

        public static Expression AndAlso(this Expression expression, Expression expressionRight)
        {
            return Expression.AndAlso(expression, expressionRight);
        }

        public static Expression Or(this Expression expression, Expression expressionRight)
        {
            return Expression.Or(expression, expressionRight);
        }

        public static Expression And(this Expression expression, Expression expressionRight)
        {
            return Expression.And(expression, expressionRight);
        }
        
    }

    public static class DynamicExtention
    {
        /// <summary>
        /// 自定义Where查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereCustom<T>(this IQueryable<T> query, List<FilterSearch> filters)
        {
            var param = DynamicLinq.CreateLambdaParam<T>("c");
            Expression body = Expression.Constant(true); //初始默认一个true  
            foreach (var filter in filters)
            {
                body = body.AndAlso(param.GenerateBody<T>(filter)); //这里可以根据需要自由组合  
            }
            var lambda = param.GenerateTypeLambda<T>(body); //最终组成lambda  
            return query.Where(lambda);
        }

        /// <summary>
        /// 自定义Count查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static int CountCustom<T>(this IQueryable<T> query, List<FilterSearch> filters)
        {
            var param = DynamicLinq.CreateLambdaParam<T>("c");
            Expression body = Expression.Constant(true); //初始默认一个true  
            foreach (var filter in filters)
            {
                body = body.AndAlso(param.GenerateBody<T>(filter)); //这里可以根据需要自由组合  
            }
            var lambda = param.GenerateTypeLambda<T>(body); //最终组成lambda  
            return query.Count(lambda);
        }

        /// <summary>
        /// 自定义Any查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static bool AnyCustom<T>(this IQueryable<T> query, List<FilterSearch> filters)
        {
            var param = DynamicLinq.CreateLambdaParam<T>("c");
            Expression body = Expression.Constant(true); //初始默认一个true  
            foreach (var filter in filters)
            {
                body = body.AndAlso(param.GenerateBody<T>(filter)); //这里可以根据需要自由组合  
            }
            var lambda = param.GenerateTypeLambda<T>(body); //最终组成lambda  
            return query.Any(lambda);
        }

        /// <summary>
        /// 排序拓展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="ordering"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByCustom<T>(this IQueryable<T> query, string ordering, params object[] values)
        {
            if (query == null)
                throw new ArgumentException("query is null");
            return DynamicQueryable.OrderBy(query, ordering, values);
        }
        
    }

}
