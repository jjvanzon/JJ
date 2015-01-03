//
//  Circle.Framework.Integration.DotNet.Expressions.ExpressionToValueTranslator
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
    public class ExpressionToValueTranslator : ExpressionVisitor
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

        protected override Expression VisitMember(MemberExpression node)
        {
            Members.Insert(0, node.Member);

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            Constant = node.Value;

            return base.VisitConstant(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.ArrayLength:
                    IsArrayLength = true;
                    break;
            }

            return base.VisitUnary(node);
        }
    }
}
