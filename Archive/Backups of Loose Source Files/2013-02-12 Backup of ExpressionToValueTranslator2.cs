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
                    var lambda = (LambdaExpression)node;
                    Visit(lambda.Body);
                    return;

                case  ExpressionType.MemberAccess:
                    VisitMember((MemberExpression)node);
                    return;

                case ExpressionType.ArrayLength:
                    IsArrayLength = true;

                    // This is 'bad code', but faster.
                    var unary = (UnaryExpression)node;
                    if (unary.Operand.NodeType == ExpressionType.MemberAccess)
                    {
                        var member = (MemberExpression)unary.Operand;
                        {
                            VisitMember(member);
                        }
                    }

                    return;
            }

            throw new ArgumentException(String.Format("Name cannot be obtained from {0}.", node.GetType().Name));
        }

        private void VisitMember(MemberExpression node)
        {
            Members.Insert(0, node.Member);

            if (node.Expression != null)
            {
                // Visit(node.Expression); // The below should be slightly faster.
                switch (node.Expression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        var node2 = (MemberExpression)node.Expression;
                        VisitMember(node2);
                        return;

                    case ExpressionType.Constant:
                        var constantExpression = (ConstantExpression)node.Expression;
                        Constant = constantExpression.Value;
                        return;
                }
            }
        }
    }
}
