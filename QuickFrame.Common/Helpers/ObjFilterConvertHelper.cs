using System;
using System.Linq;
using System.Linq.Expressions;

namespace QuickFrame.Common
{
    public class ObjFilterConvertHelper
    {
        /// <summary>
        /// 应用SortInput排序
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="source"></param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> OrderBySortInput<TEntity>(IQueryable<TEntity> source, SortInput[]? sorts)
            where TEntity : class, new()
        {
            try
            {
                if (sorts == default || !sorts.Any()) return source;
                var px = Expression.Parameter(typeof(TEntity), "px");
                var queryExpression = source.Expression;
                var selector = Expression.Property(px, sorts[0].OrderBy);
                queryExpression = CallOrderBy(true, sorts[0].Desc) ?? source.Expression;
                foreach (var item in sorts[1..])
                {
                    selector = Expression.Property(px, item.OrderBy);
                    queryExpression = CallOrderBy(false, item.Desc) ?? queryExpression;
                }
                return source.Provider.CreateQuery<TEntity>(queryExpression);

                Expression? CallOrderBy(bool first, bool desc)
                {
                    return (first, desc) switch
                    {
                        (true, true) => CreateMethodCall(nameof(Queryable.OrderByDescending)),
                        (true, false) => CreateMethodCall(nameof(Queryable.OrderBy)),
                        (false, true) => CreateMethodCall(nameof(Queryable.ThenByDescending)),
                        (false, false) => CreateMethodCall(nameof(Queryable.ThenBy))
                    };
                }
                MethodCallExpression CreateMethodCall(string methodName)
                {
                    return Expression.Call(typeof(Queryable), methodName, new[] { source.ElementType, selector.Type }, queryExpression, Expression.Quote(Expression.Lambda(selector, px)));
                }
            }
            catch (HandelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new HandelException(MessageCodeOption.Bad_002, ex.Message);
            }
        }
        /// <summary>
        /// 转为Lambda条件树
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> ConvertToExpression<TEntity>(GroupInput? input)
            where TEntity : class, new()
        {
            try
            {
                if (input == default) return px => true;
                var px = Expression.Parameter(typeof(TEntity), "px");
                var expression = ConvertToExpression(px, input);
                if (expression == default) return px => true;
                return Expression.Lambda<Func<TEntity, bool>>(expression, px);
            }
            catch (HandelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new HandelException(MessageCodeOption.Bad_002, ex.Message);
            }
        }
        /// <summary>
        /// 转为Lambda条件树
        /// </summary>
        /// <param name="px"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private static Expression? ConvertToExpression(ParameterExpression px, GroupInput input)
        {
            Expression? itemExpress = default;
            Expression? groupExpress = default;
            input.Groups = input.Groups?.RemoveNull();
            input.Items = input.Items?.RemoveNull();

            if (input.Items != default && input.Items.Any())
            {
                var binarys = new Expression[input.Items.Length];
                var idx = 0;
                foreach (var item in input.Items)
                {
                    var left = Expression.Property(px, item.Field);
                    var right = Expression.Constant(ConvertObject(left.Type, item.Value), left.Type);
                    Expression binary = item.Compare switch
                    {
                        ConstantOptions.CompareConstant.Equal => Expression.Equal(left, right),
                        ConstantOptions.CompareConstant.NotEqual => Expression.NotEqual(left, right),
                        ConstantOptions.CompareConstant.Less => Expression.LessThan(left, right),
                        ConstantOptions.CompareConstant.LessEq => Expression.LessThanOrEqual(left, right),
                        ConstantOptions.CompareConstant.Greater => Expression.GreaterThan(left, right),
                        ConstantOptions.CompareConstant.GreaterEq => Expression.GreaterThanOrEqual(left, right),
                        ConstantOptions.CompareConstant.Contains => Expression.Call(left, left.Type.GetMethod(nameof(string.Contains), new[] { typeof(string) })!, right),
                        _ => throw new HandelException(MessageCodeOption.Bad_002, $"未知的比较符{item.Compare}"),
                    };
                    binarys[idx++] = binary;
                }
                foreach (var item in binarys[1..])
                {
                    binarys[0] = BinaryLogic(input.Logic, binarys[0], item);
                }
                itemExpress = binarys.Length > 0 ? binarys[0] : default;
            }

            if (input.Groups != default && input.Groups.Any())
            {
                var binarys = new Expression[input.Groups.Length];
                var idx = 0;
                foreach (var item in input.Groups)
                {
                    var binary = ConvertToExpression(px, item);
                    if (binary != null)
                        binarys[idx++] = binary;
                }
                foreach (var item in binarys[1..])
                {
                    binarys[0] = BinaryLogic(input.Logic, binarys[0], item);
                }
                groupExpress = binarys.Length > 0 ? binarys[0] : default;
            }
            if (itemExpress != default && groupExpress != default) return BinaryLogic(input.Logic, itemExpress, groupExpress);
            if (itemExpress == default && groupExpress == default) return default;
            if (itemExpress == default) return groupExpress;
            if (groupExpress == default) return itemExpress;
            return default;

            static BinaryExpression BinaryLogic(string logic, Expression left, Expression right)
            {
                return logic switch
                {
                    ConstantOptions.LogicConstant.Or => Expression.OrElse(left, right),
                    ConstantOptions.LogicConstant.And => Expression.AndAlso(left, right),
                    _ => Expression.AndAlso(left, right)
                };
            }
        }
        /// <summary>
        /// 值转换
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConvertObject(Type type, string value)
        {
            try
            {
                var typeName = type.Name;
                if (type.IsNullable())
                {
                    typeName = type.GetGenericArguments()[0].Name;
                }
                return typeName switch
                {
                    ConstantOptions.BaseDataTypeConstant.Int16 => Convert.ToInt16(value),
                    ConstantOptions.BaseDataTypeConstant.Int32 => Convert.ToInt32(value),
                    ConstantOptions.BaseDataTypeConstant.Int64 => Convert.ToInt64(value),
                    ConstantOptions.BaseDataTypeConstant.UInt16 => Convert.ToUInt16(value),
                    ConstantOptions.BaseDataTypeConstant.UInt32 => Convert.ToUInt32(value),
                    ConstantOptions.BaseDataTypeConstant.UInt64 => Convert.ToUInt64(value),
                    ConstantOptions.BaseDataTypeConstant.Byte => Convert.ToByte(value),
                    ConstantOptions.BaseDataTypeConstant.SByte => Convert.ToSByte(value),
                    ConstantOptions.BaseDataTypeConstant.Double => Convert.ToDouble(value),
                    ConstantOptions.BaseDataTypeConstant.Float => Convert.ToSingle(value),
                    ConstantOptions.BaseDataTypeConstant.Decimal => Convert.ToDecimal(value),
                    ConstantOptions.BaseDataTypeConstant.Bool => Convert.ToBoolean(value),
                    ConstantOptions.BaseDataTypeConstant.Char => Convert.ToChar(value),
                    ConstantOptions.BaseDataTypeConstant.DateTime => Convert.ToDateTime(value),
                    _ => value,
                };
            }
            catch (FormatException ex)
            {
                throw new HandelException(MessageCodeOption.Bad_002, ex.Message);
            }
        }
    }
}
