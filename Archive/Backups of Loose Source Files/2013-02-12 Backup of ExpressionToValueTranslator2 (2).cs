//
//  Circle.Framework.Integration.DotNet.Expressions.ExpressionToValueTranslator2
//
//      Author: Jan-Joost van Zon
//      Date: 2013-02-12 - 2013-02-12
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
    public class ExpressionToValueTranslator2 : ExpressionVisitor
    {
        private List<MemberInfo> Members = new List<MemberInfo>();
        private object Constant;
        private bool IsArrayLength;

        public object Result 
        {
            get
            {
                object value = Constant;

                foreach (MemberInfo member in Members)
                {
                    switch (member.MemberType)
                    {
                        case MemberTypes.Field:
                            var field = (FieldInfo)member;
                            value = field.GetValue(value);
                            break;

                        case MemberTypes.Property:
                            var property = (PropertyInfo)member;
                            value = property.GetValue(value, null);
                            break;
                    }
                }

                if (IsArrayLength)
                {
                    Array array = (Array)value;
                    return array.Length;
                }
                else
                {
                    return value;
                }
            }
        }

        public void Visit(Expression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Lambda:
                    var lambdaExpression = (LambdaExpression)node;
                    Visit(lambdaExpression.Body);
                    return;

                case  ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)node;
                    VisitMember(memberExpression);
                    return;

                case ExpressionType.ArrayLength:
                    var unaryExpression = (UnaryExpression)node;
                    Visit(unaryExpression.Operand);
                    IsArrayLength = true;
                    return;

                case ExpressionType.Constant:
                    var constantExpression = (ConstantExpression)node;
                    Constant = constantExpression.Value;
                    return;

            }

            throw new ArgumentException(String.Format("Name cannot be obtained from {0}.", node.GetType().Name));
        }

        private void VisitMember(MemberExpression node)
        {
            Members.Insert(0, node.Member);

            if (node.Expression != null)
            {
                Visit(node.Expression);
            }
        }
    }
}
