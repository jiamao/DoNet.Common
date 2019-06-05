using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.CodeDom.Compiler;

namespace DoNet.Common.LinqExpression
{
    /// <summary>
    /// 表达式辅助类
    /// </summary>
    public class Helper
    {
        //public static Expression<Func<T, bool>> CompileExpression<T>(string exp)
        //{
        //    var context=new expression
        //    var p = Expression<T>.Parameter(typeof(T), typeof(T).Name);
        //    var e = 
        //}

        /// <summary>
        /// 转换表达式为sql条件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="fun">表达式</param>
        /// <returns>查询条件</returns>
        public static string DserExpressionToWhere<T>(Expression<Func<T, bool>> fun,IDictionary<string,object> pars = null,char parchar = '?',char spstart = ' ',char spend = ' ')
        {
            if (fun.Body is BinaryExpression)
            {
                var be = (BinaryExpression)fun.Body;
                var where = BinarExpressionProvider(be.Left, be.Right, be.NodeType, pars, parchar,spstart,spend);
                return where;
            }
            else
            {
                return ExpressionRouter(fun.Body, pars, parchar, spstart, spend);
            }
        }

        // 条件表达式解析。
        public static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type, IDictionary<string, object> pars = null, char parchar = '?', char spstart = ' ', char spend = ' ')
        {
            string sb = "(";
            //先处理左边
            string tmpStr = ExpressionRouter(left, null);
            if (tmpStr == "null")
            {
                Expression temp = left;
                left = right;
                right = temp;
            }
            sb += ExpressionRouter(left, pars, parchar, spstart, spend);
            sb += ExpressionTypeCast(type);
            //再处理右边
            tmpStr = ExpressionRouter(right, pars, parchar, spstart, spend);
            if (tmpStr == "null")
            {
                if (sb.EndsWith(" ="))
                    sb = spstart + sb.Substring(0, sb.Length - 2) + spend + " is null";
                else if (sb.EndsWith("<>"))
                    sb = spstart + sb.Substring(0, sb.Length - 2) + spend + " is not null";
            }
            else
                sb += tmpStr;
            return sb += ")";
        }
        // 表达式路由。
        static string ExpressionRouter(Expression exp, IDictionary<string, object> pars = null, char parchar = '?', char spstart = ' ', char spend = ' ')
        {
            string sb = string.Empty;
            if (exp is BinaryExpression)
            {
                BinaryExpression be = ((BinaryExpression)exp);
                return BinarExpressionProvider(be.Left, be.Right, be.NodeType, pars, parchar, spstart, spend);
            }
            else if (exp is MemberExpression)
            {
                if (!exp.ToString().StartsWith("value("))
                {
                    MemberExpression me = ((MemberExpression)exp);
                    return spstart +  me.Member.Name + spend;
                }
                else
                {
                    var result = Expression.Lambda(exp).Compile().DynamicInvoke();
                    if (result == null)
                        return "null";

                    string re = "";
                    //如果指定需要参数传递。则生成参数
                    if (pars != null)
                    {
                        re = parchar + "exp_par_" + pars.Count.ToString();
                        pars.Add(re, result);
                    }
                    else
                    {
                        if (result is ValueType)
                        {
                            re = result.ToString();
                        }
                        else if (result is string || result is DateTime || result is char)
                        {
                            re = string.Format("'{0}'", (result as string).Replace("'", "''"));
                        }
                    }
                    return re;
                }
            }
            else if (exp is NewArrayExpression)
            {
                NewArrayExpression ae = ((NewArrayExpression)exp);
                StringBuilder tmpstr = new StringBuilder();
                foreach (Expression ex in ae.Expressions)
                {
                    tmpstr.Append(ExpressionRouter(ex, pars, parchar,spstart,spend));
                    tmpstr.Append(",");
                }
                return tmpstr.ToString(0, tmpstr.Length - 1);
            }
            else if (exp is MethodCallExpression)
            {
                MethodCallExpression mce = (MethodCallExpression)exp;
                if (mce.Method.Name == "Like")
                    return string.Format("({0} like {1})", 
                        ExpressionRouter(mce.Arguments[0], pars, parchar, spstart, spend),
                        ExpressionRouter(mce.Arguments[1], pars, parchar, spstart, spend));
                else if (mce.Method.Name == "NotLike")
                    return string.Format("({0} Not like {1})", 
                        ExpressionRouter(mce.Arguments[0], pars, parchar, spstart, spend),
                        ExpressionRouter(mce.Arguments[1], pars, parchar, spstart, spend));
                else if (mce.Method.Name == "In")
                    return string.Format("{0} In ({1})", 
                        ExpressionRouter(mce.Arguments[0], pars, parchar, spstart, spend),
                        ExpressionRouter(mce.Arguments[1], pars, parchar, spstart, spend));
                else if (mce.Method.Name == "NotIn")
                    return string.Format("{0} Not In ({1})", 
                        ExpressionRouter(mce.Arguments[0], pars, parchar, spstart, spend),
                        ExpressionRouter(mce.Arguments[1], pars, parchar, spstart, spend));
            }
            else if (exp is ConstantExpression)
            {
                ConstantExpression ce = ((ConstantExpression)exp);
                if (ce.Value == null)
                    return "null";
                else if (ce.Value is ValueType)
                    return ce.Value.ToString();
                else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
                    return string.Format("'{0}'", (ce.Value as string).Replace("'", "''"));
            }
            else if (exp is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp);
                return ExpressionRouter(ue.Operand, pars, parchar, spstart, spend);
            }
            return null;
        }
        // 表达式类型转换。
        static string ExpressionTypeCast(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Equal:
                    return " =";
                case ExpressionType.GreaterThan:
                    return " >";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " Or ";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                default:
                    return null;
            }

        }
    }
}
