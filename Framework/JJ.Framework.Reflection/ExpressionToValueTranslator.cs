﻿using JJ.Framework.PlatformCompatibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace JJ.Framework.Reflection
{
    internal class ExpressionToValueTranslator
    {
        // Not using the ExpressionVisitor base class performs better.

        private Stack<object> Stack = new Stack<object>();

        public object Result
        {
            get { return Stack.Peek(); }
        }

        public void Visit<T>(Expression<Func<T>> expression)
        {
            Visit((LambdaExpression)expression);
        }

        public void Visit(LambdaExpression expression)
        {
            Visit(expression.Body);
        }

        public void Visit(Expression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.MemberAccess:
                {
                    var memberExpression = (MemberExpression)node;
                    VisitMember(memberExpression);
                    return;
                }

                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                {
                    var unaryExpression = (UnaryExpression)node;
                    VisitConvert(unaryExpression);
                    return;
                }

                case ExpressionType.ArrayLength:
                {
                    var unaryExpression = (UnaryExpression)node;
                    VisitArrayLength(unaryExpression);
                    return;
                }

                case ExpressionType.Call:
                {
                    var methodCallExpression = (MethodCallExpression)node;
                    VisitMethodCall(methodCallExpression);
                    return;
                }

                case ExpressionType.Constant:
                {
                    var constantExpression = (ConstantExpression)node;
                    Stack.Push(constantExpression.Value);
                    return;
                }

                case ExpressionType.ArrayIndex:
                {
                    var binaryExpression = (BinaryExpression)node;
                    VisitArrayIndex(binaryExpression);
                    return;
                }

                case ExpressionType.NewArrayInit:
                {
                    var newArrayExpression = (NewArrayExpression)node;
                    VisitNewArray(newArrayExpression);
                    return;
                }
            }

            throw new ArgumentException(String.Format("Value cannot be obtained from {0}.", node.NodeType));
        }

        private void VisitConstant(UnaryExpression unaryExpression)
        {
            var constantExpression = (ConstantExpression)unaryExpression.Operand;
            Stack.Push(constantExpression.Value);
        }

        private void VisitMember(MemberExpression node)
        {
            // First process 'parent' node.
            if (node.Expression != null)
            {
                Visit(node.Expression);
            }

            // Then process 'child' node.

            MemberTypes_PlatformSafe memberType = node.Member.MemberType_PlatformSafe();
            switch (memberType)
            {
                case MemberTypes_PlatformSafe.Field:
                    VisitField(node);
                    return;

                case MemberTypes_PlatformSafe.Property:
                    VisitProperty(node);
                    return;

                default:
                    throw new NotSupportedException(String.Format("Member types other than FieldInfo and PropertyInfo are not supported. Member type = {0}", node.Member.GetType().Name));
            }
        }

        private void VisitField(MemberExpression node)
        {
            var field = (FieldInfo)node.Member;
            object obj = null;
            if (!field.IsStatic)
            {
                obj = Stack.Pop();
            }
            object value = field.GetValue(obj);
            Stack.Push(value);
        }

        private void VisitProperty(MemberExpression node)
        {
            var property = (PropertyInfo)node.Member;
            object obj = null;
            MethodInfo getterOrSetter = property.GetGetMethod() ?? property.GetSetMethod();
            if (!getterOrSetter.IsStatic)
            {
                obj = Stack.Pop();
            }
            object value = property.GetValue(obj, null);
            Stack.Push(value);
        }

        private void VisitMethodCall(MethodCallExpression node)
        {
            if (!node.Method.IsStatic)
            {
                Visit(node.Object);
            }
            else
            {
                Stack.Push(null);
            }

            for (int i = 0; i < node.Arguments.Count; i++)
            {
                Visit(node.Arguments[i]);
            }
            object[] arguments = new object[node.Arguments.Count];
            for (int i = node.Arguments.Count - 1; i >= 0; i--)
            {
                arguments[i] = Stack.Pop();
            }

            object obj = Stack.Pop();
            object value = node.Method.Invoke(obj, arguments);
            Stack.Push(value);
        }

        private void VisitConvert(UnaryExpression node)
        {
            switch (node.Operand.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)node.Operand;
                    VisitMember(memberExpression);
                    break;

                case ExpressionType.Call:
                    Visit(node.Operand);
                    break;

                case ExpressionType.ArrayIndex:
                    var binaryExpression = (BinaryExpression)node.Operand;
                    VisitArrayIndex(binaryExpression);
                    break;

                case ExpressionType.Constant:
                    VisitConstant(node);
                    break;

                default:
                    throw new ArgumentException(String.Format("Value cannot be obtained from NodeType {0}.", node.Operand.NodeType));
            }
            object obj = Stack.Pop();
            if (obj is IConvertible)
            {
                obj = Convert.ChangeType(obj, node.Type);
            }
            Stack.Push(obj);
        }

        private void VisitArrayLength(UnaryExpression node)
        {
            if (node.Operand.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpression = (MemberExpression)node.Operand;
                VisitMember(memberExpression);
                Array array = (Array)Stack.Pop();
                Stack.Push(array.Length);
                return;
            }

            throw new ArgumentException(String.Format("Value cannot be obtained from NodeType {0}.", node.Operand.NodeType));
        }

        private void VisitArrayIndex(BinaryExpression node)
        {
            var memberExpression = (MemberExpression)node.Left;
            VisitMember(memberExpression);
            var array = (Array)Stack.Pop();

            switch (node.Right.NodeType)
            {
                case ExpressionType.Constant:
                    var constantExpression = (ConstantExpression)node.Right;
                    int index = (int)constantExpression.Value;
                    Stack.Push(array.GetValue(index));
                    break;

                case ExpressionType.MemberAccess:
                    var memberExpression2 = (MemberExpression)node.Right;
                    VisitMember(memberExpression2);
                    break;
            }
        }

        private void VisitNewArray(NewArrayExpression node)
        {
            for (int i = 0; i < node.Expressions.Count; i++)
            {
                Visit(node.Expressions[i]);
            }
            Array array = (Array)Activator.CreateInstance(node.Type, node.Expressions.Count);
            for (int i = node.Expressions.Count - 1; i >= 0; i--)
            {
                object item = Stack.Pop();
                array.SetValue(item, i);
            }
            Stack.Push(array);
        }
    }
}
