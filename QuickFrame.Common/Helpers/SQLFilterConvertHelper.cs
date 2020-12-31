using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace QuickFrame.Common
{
    /// <summary>
    /// SQLWhere转为Expression
    /// </summary>
    public class SQLFilterConvertHelper
    {
        /// <summary>
        /// SQLWhere转为Expression
        /// </summary>
        public static Expression<Func<TEntity, bool>> ConvertToExpression<TEntity>(string sqlwhere)
            where TEntity : class, new()
        {
            try
            {
                if (sqlwhere.Length < 1) return px => true;
                var works = LexicalAnalysis(sqlwhere);
                if (works.Count(x => x.ID != LexicalTokenID.OpenParen) == works.Count(x => x.ID == LexicalTokenID.CloseParen)) throw new HandelException(MessageCodeOption.Bad_002, "语法错误");
                var index = 0;
                var tree = NextToken(works, ref index);
                var px = Expression.Parameter(typeof(TEntity), "px");
                var expression = ConvertToExpression(px, tree);
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
        /// <param name="block"></param>
        /// <returns></returns>
        private static Expression? ConvertToExpression(ParameterExpression px, BlockToken block)
        {
            Expression? exp = default;
            foreach (var item in block.Items)
            {
                if (item is ItemToken it)
                {
                    var left = Expression.Property(px, it.Field);
                    var right = Expression.Constant(ObjFilterConvertHelper.ConvertObject(left.Type, it.Value ?? string.Empty), left.Type);
                    Expression binary = it.Compare switch
                    {
                        "=" => Expression.Equal(left, right),
                        "!=" => Expression.NotEqual(left, right),
                        "<" => Expression.LessThan(left, right),
                        "<=" => Expression.LessThanOrEqual(left, right),
                        ">" => Expression.GreaterThan(left, right),
                        ">=" => Expression.GreaterThanOrEqual(left, right),
                        _ => Expression.Call(left, left.Type.GetMethod(nameof(string.Contains), new[] { typeof(string) })!, right),
                    };
                    if (exp == default)
                    {
                        exp = binary;
                    }
                    else
                    {
                        exp = it.Logic switch
                        {
                            ConstantOptions.LogicConstant.Or => Expression.OrElse(exp, binary),
                            ConstantOptions.LogicConstant.And => Expression.AndAlso(exp, binary),
                            _ => Expression.AndAlso(exp, binary)
                        };
                    }
                }
                else if (item is BlockToken bk)
                {
                    var binary = ConvertToExpression(px, bk) ?? throw new HandelException(MessageCodeOption.Bad_002, "语法错误");
                    if (exp == default)
                    {
                        exp = binary;
                    }
                    else
                    {
                        exp = bk.Logic switch
                        {
                            ConstantOptions.LogicConstant.Or => Expression.OrElse(exp, binary),
                            ConstantOptions.LogicConstant.And => Expression.AndAlso(exp, binary),
                            _ => Expression.AndAlso(exp, binary)
                        };
                    }
                }
            }
            return exp;
        }
        /// <summary>
        /// 语法分析(转为树结构)
        /// </summary>
        private static BlockToken NextToken(List<WorkToken> works, ref int index)
        {
            var block = new BlockToken();
            string? blockLogic = default;
            ItemToken? currItem = default;
            while (index < works.Count)
            {
                var item = works[index];
                var back = index < 1 ? works[0] : works[index - 1];//上一个
                var next = index == works.Count - 1 ? works[^1] : works[index + 1];//下一个
                index++;
                switch (item.ID)
                {
                    case LexicalTokenID.OpenParen:
                        if (back.ID == LexicalTokenID.Contains) break;
                        var bk = NextToken(works, ref index);
                        if (blockLogic != default)
                        {
                            bk.Logic = blockLogic;
                            blockLogic = default;
                        }
                        block.Items.Add(bk);
                        break;
                    case LexicalTokenID.CloseParen:
                        if (index - 5 > 0 && back.ID == LexicalTokenID.Semicolon && works[index - 3].ID == LexicalTokenID.Word && works[index - 4].ID == LexicalTokenID.Semicolon && works[index - 5].ID == LexicalTokenID.OpenParen) break;//如果括号是contains的括号则忽略
                        return block;
                    case LexicalTokenID.Word:
                        var xi = next.ID == LexicalTokenID.Space ? 2 : 1;//倒数第二哥单词的处理
                        if (index + xi >= works.Count)
                        {
                            _ = currItem ?? throw new HandelException(MessageCodeOption.Bad_002, $"{item.Value}附近存在语法错误");
                            if (currItem != default)
                            {
                                currItem.Value = item.Value;
                                block.Items.Add(currItem);
                                currItem = default;
                                break;
                            }
                        }
                        if (next.ID == LexicalTokenID.Space)
                            next = works[index + 1];
                        if (next.ID is (LexicalTokenID.Equal or LexicalTokenID.NotEqual or LexicalTokenID.Less or LexicalTokenID.LessEq or LexicalTokenID.Greater or LexicalTokenID.GreaterEq) || (next.ID == LexicalTokenID.Spot && works[index + 1].ID == LexicalTokenID.Contains))
                        {
                            currItem ??= new ItemToken();
                            currItem.Field = item.Value;
                        }
                        else
                        {
                            _ = currItem ?? throw new HandelException(MessageCodeOption.Bad_002, $"{item.Value}附近存在语法错误");
                            currItem.Value = item.Value;
                            block.Items.Add(currItem);
                            currItem = default;
                        }
                        break;
                    case LexicalTokenID.Greater:
                    case LexicalTokenID.GreaterEq:
                    case LexicalTokenID.Less:
                    case LexicalTokenID.LessEq:
                    case LexicalTokenID.Equal:
                    case LexicalTokenID.NotEqual:
                        if (back.ID is (LexicalTokenID.Word or LexicalTokenID.Space) && next.ID is (LexicalTokenID.Word or LexicalTokenID.Space or LexicalTokenID.Semicolon or LexicalTokenID.DSemicolon))
                        {
                            _ = currItem ?? throw new HandelException(MessageCodeOption.Bad_002, $"{item.Value}附近存在语法错误");
                            currItem.Compare = item.Value;
                            break;
                        }
                        throw new HandelException(MessageCodeOption.Bad_002, $"{item.Value}附近存在语法错误");
                    case LexicalTokenID.Contains:
                        if (back.ID == LexicalTokenID.Spot && next.ID == LexicalTokenID.OpenParen)
                        {
                            _ = currItem ?? throw new HandelException(MessageCodeOption.Bad_002, $"{item.Value}附近存在语法错误");
                            currItem.Compare = item.Value;
                            break;
                        }
                        throw new HandelException(MessageCodeOption.Bad_002, $"{item.Value}附近存在语法错误");
                    case LexicalTokenID.And:
                    case LexicalTokenID.Or:
                        if (index + 1 > works.Count) throw new HandelException(MessageCodeOption.Bad_002, $"{item.Value}附近存在语法错误");
                        var next_1 = works[index + 1];//下一个的下一个
                        if ((next.ID == LexicalTokenID.Space && next_1.ID == LexicalTokenID.OpenParen) || next.ID == LexicalTokenID.OpenParen)
                        {
                            blockLogic = item.Value;
                            break;
                        }
                        else if (back.ID == LexicalTokenID.Space && next.ID == LexicalTokenID.Space)
                        {
                            currItem = new ItemToken
                            {
                                Logic = item.Value
                            };
                            break;
                        }
                        throw new HandelException(MessageCodeOption.Bad_002, $"{item.Value}附近存在语法错误");
                    case LexicalTokenID.Semicolon:
                        break;
                    case LexicalTokenID.DSemicolon:
                        break;
                    case LexicalTokenID.Spot:
                        break;
                    case LexicalTokenID.Space:
                        break;
                    default:
                        throw new HandelException(MessageCodeOption.Bad_002, $"{item.Value}附近存在语法错误");
                }
            }
            return block;
        }
        /// <summary>
        /// 词法分析
        /// </summary>
        private static List<WorkToken> LexicalAnalysis(string sql)
        {
            var builder = new StringBuilder(sql);
            var works = new List<WorkToken>(20);
            var workBuilder = new StringBuilder();
            var spaceBuilder = new StringBuilder();
            var index = 0;
            while (index < builder.Length)
            {
                var item = builder[index++];
                switch (item)
                {
                    case '(':
                        CreateWork();
                        works.Add(new WorkToken("(", LexicalTokenID.OpenParen));
                        break;
                    case ')':
                        CreateWork();
                        works.Add(new WorkToken(")", LexicalTokenID.CloseParen));
                        break;
                    case '>':
                        CreateWork();
                        if (builder[index] == '=')
                        {
                            works.Add(new WorkToken(">=", LexicalTokenID.GreaterEq));
                            index++;
                        }
                        else
                        {
                            works.Add(new WorkToken(">", LexicalTokenID.Greater));
                        }
                        break;
                    case '<':
                        CreateWork();
                        if (builder[index] == '=')
                        {
                            works.Add(new WorkToken("<=", LexicalTokenID.LessEq));
                            index++;
                        }
                        else
                        {
                            works.Add(new WorkToken("<", LexicalTokenID.Less));
                        }
                        break;
                    case '=':
                        CreateWork();
                        works.Add(new WorkToken("=", LexicalTokenID.Equal));
                        break;
                    case '!':
                        CreateWork();
                        if (builder[index] == '=')
                        {
                            works.Add(new WorkToken("!=", LexicalTokenID.NotEqual));
                            index++;
                        }
                        else
                        {
                            workBuilder.Append(item);
                        }
                        break;
                    case '\'':
                        CreateWork();
                        works.Add(new WorkToken("'", LexicalTokenID.Semicolon));
                        if (builder[index] == '\'')
                        {
                            works.Add(new WorkToken("", LexicalTokenID.Word));
                            works.Add(new WorkToken("'", LexicalTokenID.Semicolon));
                            index++;
                            break;
                        }
                        do
                        {
                            workBuilder.Append(builder[index]);
                            index++;
                            if (builder[index] == '\'' && builder[index - 1] != '\\')
                            {
                                CreateWork();
                                works.Add(new WorkToken("'", LexicalTokenID.Semicolon));
                                index++;
                                goto end;
                            }
                        } while (index < builder.Length);
                    end: break;
                        throw new HandelException(MessageCodeOption.Bad_002, $"'没有结束标记");
                    case '"':
                        CreateWork();
                        works.Add(new WorkToken("\"", LexicalTokenID.DSemicolon));
                        break;
                    case '.':
                        CreateWork();
                        works.Add(new WorkToken(".", LexicalTokenID.Spot));
                        break;
                    case ' ':
                        CreateWork();
                        spaceBuilder.Append(' ');
                        while (index < builder.Length)
                        {
                            if (builder[index] == ' ')
                            {
                                spaceBuilder.Append(' ');
                                index++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        works.Add(new WorkToken(spaceBuilder.ToString(), LexicalTokenID.Space));
                        spaceBuilder.Clear();
                        break;
                    default:
                        workBuilder.Append(item);
                        if (workBuilder.Equals("and"))
                        {
                            works.Add(new WorkToken("and", LexicalTokenID.And));
                            workBuilder.Clear();
                        }
                        else if (workBuilder.Equals("or"))
                        {
                            works.Add(new WorkToken("or", LexicalTokenID.Or));
                            workBuilder.Clear();
                        }
                        else if (workBuilder.Equals("contains"))
                        {
                            works.Add(new WorkToken("contains", LexicalTokenID.Contains));
                            workBuilder.Clear();
                        }
                        break;
                }
            }
            CreateWork();

            void CreateWork()
            {
                if (workBuilder?.Length > 0)
                {
                    var token = new WorkToken(workBuilder.ToString(), LexicalTokenID.Word);
                    works.Add(token);
                    workBuilder.Clear();
                }
            }
            return works;
        }
        /// <summary>
        /// 描述词法最小单元
        /// </summary>
        internal enum LexicalTokenID : byte
        {
            /// <summary>
            /// 未知
            /// </summary>
            UnKnow,
            /// <summary>
            /// 括号开始
            /// </summary>
            OpenParen,
            /// <summary>
            /// 括号结束
            /// </summary>
            CloseParen,
            /// <summary>
            /// 单词
            /// </summary>
            Word,
            /// <summary>
            /// 单引号
            /// </summary>
            Semicolon,
            /// <summary>
            /// 双引号
            /// </summary>
            DSemicolon,
            /// <summary>
            /// 大于
            /// </summary>
            Greater,
            /// <summary>
            /// 大于等于
            /// </summary>
            GreaterEq,
            /// <summary>
            /// 小于
            /// </summary>
            Less,
            /// <summary>
            /// 小于等于
            /// </summary>
            LessEq,
            /// <summary>
            /// 等于
            /// </summary>
            Equal,
            /// <summary>
            /// 不等于
            /// </summary>
            NotEqual,
            /// <summary>
            /// 点
            /// </summary>
            Spot,
            /// <summary>
            /// 包含
            /// </summary>
            Contains,
            /// <summary>
            /// 空格
            /// </summary>
            Space,
            /// <summary>
            /// 并且
            /// </summary>
            And,
            /// <summary>
            /// 或者
            /// </summary>
            Or,
        }
        /// <summary>
        /// 单词结构体
        /// </summary>
        internal readonly struct WorkToken
        {
            /// <summary>
            /// 值
            /// </summary>
            public readonly string Value;
            /// <summary>
            /// 类型
            /// </summary>
            public readonly LexicalTokenID ID;

            public WorkToken(string value, LexicalTokenID id)
            {
                Value = value;
                ID = id;
            }
        }
        /// <summary>
        /// 条件块
        /// </summary>
        internal class BlockToken : TokenModel
        {
            public string? Logic { get; set; }
            public List<TokenModel> Items { get; set; } = new List<TokenModel>(10);
        }
        /// <summary>
        /// 条件
        /// </summary>
        internal class ItemToken : TokenModel
        {
            public string Logic { get; set; } = string.Empty;
            public string Field { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
            public string Compare { get; set; } = string.Empty;
        }
        internal class TokenModel { }
    }
}
