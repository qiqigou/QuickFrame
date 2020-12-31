using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace QuickFrame.Common
{
    /// <summary>
    /// 表达式树构建帮助类
    /// </summary>
    public class ExpressionHelper
    {
        /// <summary>
        /// 拼接条件表达式
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="propName">属性名</param>
        /// <param name="values">值</param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> WhereEqualOr<TEntity, TValue>(string propName, TValue[] values)
            where TEntity : class
            where TValue : notnull
        {
            _ = values.Any() ? true : throw new ArgumentNullException($"参数{nameof(values)}没有任何值");
            var px = Expression.Parameter(typeof(TEntity), "px");
            var left = Expression.Property(px, propName);
            Expression? where = default;
            foreach (var item in values)
            {
                var right = Expression.Constant(item);
                var comp = Expression.Equal(left, right);
                if (where == default)
                {
                    where = comp;
                }
                else
                {
                    where = Expression.OrElse(where, comp);
                }
            }
            _ = where ?? throw new ArgumentNullException($"条件表达式{nameof(where)}为空");
            return Expression.Lambda<Func<TEntity, bool>>(where, px);
        }
        /// <summary>
        /// 拼接条件表达式(支持元组)
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TValue">元组类型</typeparam>
        /// <param name="propNames">属性名</param>
        /// <param name="values">值(元组类型)</param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> WhereEqualOr<TEntity, TValue>(string[] propNames, TValue[] values)
            where TEntity : class
            where TValue : notnull
        {
            _ = propNames.Any() ? true : throw new ArgumentNullException($"参数{nameof(propNames)}没有任何值");
            _ = values.Any() ? true : throw new ArgumentNullException($"参数{nameof(values)}没有任何值");
            if (typeof(TValue).IsAssignableTo(typeof(ITuple)))
            {
                var tups = new ITuple[values.Length];
                for (int i = 0; i < tups.Length; i++)
                {
                    tups[i] = values[i] as ITuple ?? throw new ArgumentNullException($"{nameof(values)}转为ITuple失败");
                }
                var px = Expression.Parameter(typeof(TEntity), "px");
                Expression? where = default;
                foreach (var tup in tups)
                {
                    Expression? itemwhere = default;
                    for (int i = 0; i < propNames.Length; i++)
                    {
                        var left = Expression.Property(px, propNames[i]);
                        var right = Expression.Constant(tup[i]);
                        var comp = Expression.Equal(left, right);
                        if (itemwhere == default)
                        {
                            itemwhere = comp;
                        }
                        else
                        {
                            itemwhere = Expression.AndAlso(itemwhere, comp);
                        }
                    }
                    _ = itemwhere ?? throw new ArgumentNullException($"构建lambda子项{nameof(itemwhere)}为空");
                    if (where == default)
                    {
                        where = itemwhere;
                    }
                    else
                    {
                        where = Expression.OrElse(where, itemwhere);
                    }
                }
                _ = where ?? throw new ArgumentNullException($"条件表达式{nameof(where)}为空");
                return Expression.Lambda<Func<TEntity, bool>>(where, px);
            }
            else
            {
                return WhereEqualOr<TEntity, TValue>(propNames[0], values);
            }
        }
        /// <summary>
        /// 拼接条件表达式
        /// </summary>
        /// <param name="propName">属性</param>
        /// <param name="value">值</param>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> WhereLambda<TEntity, TValue>(string propName, TValue value)
            where TEntity : class
            where TValue : notnull
        {
            var px = Expression.Parameter(typeof(TEntity), "px");
            var left = Expression.Property(px, propName);
            var right = Expression.Constant(value);
            var comp = Expression.Equal(left, right);
            return Expression.Lambda<Func<TEntity, bool>>(comp, px);
        }
        /// <summary>
        /// 拼接条件表达式(支持元组)
        /// </summary>
        /// <param name="propNames">属性集合</param>
        /// <param name="value">值(支持元组)</param>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> WhereLambda<TEntity, TValue>(string[] propNames, TValue value)
            where TEntity : class
            where TValue : notnull
        {
            _ = propNames.Any() ? true : throw new ArgumentNullException($"参数{nameof(propNames)}没有任何值");
            if (typeof(TValue).IsAssignableTo(typeof(ITuple)))
            {
                var tup = value as ITuple ?? throw new ArgumentNullException($"{nameof(value)}转为ITuple失败");
                var px = Expression.Parameter(typeof(TEntity), "px");
                Expression? where = default;
                for (int i = 0; i < propNames.Length; i++)
                {
                    var left = Expression.Property(px, propNames[i]);
                    var right = Expression.Constant(tup[i]);
                    var comp = Expression.Equal(left, right);
                    if (where == default)
                    {
                        where = comp;
                    }
                    else
                    {
                        where = Expression.AndAlso(where, comp);
                    }
                }
                _ = where ?? throw new ArgumentNullException($"条件表达式{nameof(where)}为空");
                return Expression.Lambda<Func<TEntity, bool>>(where, px);
            }
            else
            {
                return WhereLambda<TEntity, TValue>(propNames[0], value);
            }
        }
        /// <summary>
        /// 拼接属性访问表达式
        /// </summary>
        /// <param name="propName"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public static Expression<Func<TEntity, TKey>> MemberLambda<TEntity, TKey>(string propName)
            where TEntity : class
            where TKey : notnull
        {
            var px = Expression.Parameter(typeof(TEntity), "px");
            var member = Expression.Property(px, propName);
            return Expression.Lambda<Func<TEntity, TKey>>(member, px);
        }
        /// <summary>
        /// 拼接属性访问表达式(支持元组)
        /// </summary>
        /// <param name="propNames">属性</param>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TValue">值类型(支持元组)</typeparam>
        /// <returns></returns>
        public static Expression<Func<TEntity, TValue>> MemberLambda<TEntity, TValue>(string[] propNames)
            where TEntity : class
            where TValue : notnull
        {
            _ = propNames.Any() ? true : throw new ArgumentNullException($"参数{nameof(propNames)}没有任何值");
            if (typeof(TValue).IsAssignableTo(typeof(ITuple)))
            {
                if (propNames.Length > 7) throw new ArgumentException($"{nameof(propNames)}元组的最大长度为8");
                var genMethod = typeof(ValueTuple).GetMethods().Where(x => x.Name == nameof(ValueTuple.Create) && x.GetGenericArguments().Length == propNames.Length).Single();
                var types = typeof(TEntity).GetProperties().Where(x => propNames.Contains(x.Name)).Select(x => x.PropertyType).ToArray();
                var method = genMethod.MakeGenericMethod(types);
                var px = Expression.Parameter(typeof(TEntity), "px");
                var propExp = propNames.Select(x => Expression.Property(px, x));
                var callExp = Expression.Call(method, propExp);
                return Expression.Lambda<Func<TEntity, TValue>>(callExp, px);
            }
            else
            {
                return MemberLambda<TEntity, TValue>(propNames[0]);
            }
        }
        /// <summary>
        /// 拼接赋值表达式
        /// </summary>
        /// <param name="propName">属性</param>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <returns></returns>
        public static Expression<Action<TEntity, TValue?>> AssignLambda<TEntity, TValue>(string propName)
            where TEntity : class
            where TValue : notnull
        {
            var data = Expression.Parameter(typeof(TEntity), "data");
            var value = Expression.Parameter(typeof(TValue), "value");
            var left = Expression.Property(data, propName);
            var assign = Expression.Assign(left, value);
            return Expression.Lambda<Action<TEntity, TValue?>>(assign, data, value);
        }
        /// <summary>
        /// 获取表达式的属性名
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string[] GetMemberNames<TEntity, TValue>(Expression<Func<TEntity, TValue>> exp)
            where TEntity : class
            where TValue : notnull
        {
            switch (exp.Body)
            {
                case NewExpression msg:
                    var names = msg.Arguments.Cast<MemberExpression>().Select(x => x.Member.Name).ToArray();
                    if (names.Length > 0) return names;
                    throw new ArgumentNullException($"lambda表达式{nameof(exp)}不包含任何属性名");
                case MemberExpression msg:
                    return new[] { msg.Member.Name };
                case UnaryExpression msg:
                    var name = msg.Operand is MemberExpression member ? member.Member.Name : throw new ArgumentException("lambda参数类型错误");
                    return new[] { name };
                default:
                    throw new ArgumentNullException($"lambda表达式{nameof(exp)}不是受支持的类型");
            }
        }
    }
}
