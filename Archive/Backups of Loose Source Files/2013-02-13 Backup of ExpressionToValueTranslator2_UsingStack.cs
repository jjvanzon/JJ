//
//  Circle.Framework.Integration.DotNet.Expressions.ExpressionToValueTranslator2_UsingStack
//
//      Author: Jan-Joost van Zon
//      Date: 2013-02-12 - 2013-02-13
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Circle.Framework.Integration.DotNet.Expressions
{
    public class ExpressionToValueTranslator2_UsingStack : ExpressionVisitor
    {
        public object Result { get; private set; }

        public void Visit(Expression node)
        {
            var lambdaExpression = node as LambdaExpression;
            if (lambdaExpression != null)
            {
                Visit(lambdaExpression.Body);
                return;
            }

            var memberExpression = node as MemberExpression;
            if (memberExpression != null)
            {
                VisitMember(memberExpression);
                return;
            }

            var unaryExpression = node as UnaryExpression;
            if (unaryExpression != null)
            {
                if (unaryExpression.NodeType == ExpressionType.Convert ||
                    unaryExpression.NodeType == ExpressionType.ConvertChecked)
                {
                    if (unaryExpression.Operand.NodeType == ExpressionType.MemberAccess)
                    {
                        var memberExpression2 = (MemberExpression)unaryExpression.Operand;
                        if (memberExpression2 != null)
                        {
                            VisitMember(memberExpression2);
                        }
                    }
                    return;
                }
                if (unaryExpression.NodeType == ExpressionType.ArrayLength)
                {
                    if (unaryExpression.Operand.NodeType == ExpressionType.MemberAccess)
                    {
                        var memberExpression2 = unaryExpression.Operand as MemberExpression;
                        if (memberExpression2 != null)
                        {
                            VisitMember(memberExpression2);

                            Array array = (Array)Result;
                            Result = array.Length;
                        }
                    }
                    return;
                }
            }

            throw new ArgumentException(String.Format("Name cannot be obtained from {0}.", node.GetType().Name));
        }

        private void VisitMember(MemberExpression node)
        {
            // First process 'parent' node.
            if (node.Expression != null)
            {
                var node2 = node.Expression as MemberExpression;
                if (node2 != null)
                {
                    VisitMember(node2);
                }

                var constantExpression = node.Expression as ConstantExpression;
                if (constantExpression != null)
                {
                    Result = constantExpression.Value;
                }
            }

            // Then process 'child' node.
            switch (node.Member.MemberType)
            {
                case MemberTypes.Field:
                    var field = (FieldInfo)node.Member;
                    Result = field.GetValue(Result);
                    break;

                case MemberTypes.Property:
                    var property = (PropertyInfo)node.Member;
                    Result = property.GetValue(Result, null);
                    break;

                default:
                    // TODO: Add message.
                    throw new NotSupportedException();
            }
        }
    }
}
