//
//  Circle.Framework.Integration.DotNet.Expressions.ExpressionHelper
//
//      Author: Jan-Joost van Zon
//      Date: 2013-02-05 - 2013-02-05
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;

namespace Circle.Framework.Integration.DotNet.Expressions
{
    public static class ExpressionHelper
    {
        // GetName

        public static string GetName(LambdaExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return GetNameFromLambdaExpression(expression);
        }

        private static string GetNameFromLambdaExpression(LambdaExpression lambdaExpression)
        {
            var memberExpression = lambdaExpression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return GetNameFromMemberExpression(memberExpression);
            }

            var unaryExpression = lambdaExpression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                return GetNameFromUnaryExpression(unaryExpression);
            }

            throw new ArgumentException(String.Format("Name cannot be obtained from {0}.", lambdaExpression.Body.GetType().Name));
        }

        private static string GetNameFromUnaryExpression(UnaryExpression unaryExpression)
        {
            MemberExpression memberExpression = null;

            switch (unaryExpression.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    memberExpression = unaryExpression.Operand as MemberExpression;
                    if (memberExpression != null)
                    {
                        return GetNameFromMemberExpression(memberExpression);
                    }
                    break;

                case ExpressionType.ArrayLength:
                    memberExpression = unaryExpression.Operand as MemberExpression;
                    if (memberExpression != null)
                    {
                        return GetNameFromMemberExpression(memberExpression) + ".Length";
                    }
                    break;
            }

            throw new ArgumentException(String.Format("Name cannot be obtained from {0}.", unaryExpression.Operand.GetType().Name));
        }

        private static string GetNameFromMemberExpression(MemberExpression memberExpression)
        {
            string name = memberExpression.Member.Name;

            var parentMemberExpression = memberExpression.Expression as MemberExpression;

            if (parentMemberExpression != null)
            {
                string qualifier = GetNameFromMemberExpression(parentMemberExpression);
                return qualifier + "." + name;
            }
            else
            {
                return name;
            }
        }

        // GetValue

        public static T GetValue<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return GetValueFromExpressionOfFunc(expression);
        }

        private static T GetValueFromExpressionOfFunc<T>(Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                return GetValueFromMemberExpression(expression, memberExpression);
            }

            var unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                return GetValueFromUnaryExpression(expression, unaryExpression);
            }

            throw new ArgumentException(String.Format("Value cannot be obtained from {0}.", expression.Body.GetType().Name));
        }

        private static T GetValueFromUnaryExpression<T>(Expression<Func<T>> expression, UnaryExpression unaryExpression)
        {
            MemberExpression memberExpression = null;

            switch (unaryExpression.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    memberExpression = unaryExpression.Operand as MemberExpression;
                    if (memberExpression != null)
                    {
                        return GetValueFromMemberExpression(expression, memberExpression);
                    }
                    break;

                case ExpressionType.ArrayLength:
                    memberExpression = unaryExpression.Operand as MemberExpression;
                    if (memberExpression != null)
                    {
                        object obj = GetValueFromMemberExpression(expression, memberExpression);
                        //return obj.Length;
                    }
                    break;
            }

            throw new ArgumentException(String.Format("Value cannot be obtained from {0}.", unaryExpression.Operand.GetType().Name));
        }

        private static T GetValueFromMemberExpression<T>(Expression<Func<T>> expression, MemberExpression memberExpression)
        {
            return (T)GetValueFromMemberExpression_WithMemberInfos(memberExpression);
        }

        // With Pure Compilation

        public static T GetValueFromMemberExpression_WithPureCompilation<T>(Expression<Func<T>> expression, MemberExpression memberExpression)
        {
            Func<T> function = expression.Compile();
            return function();
        }

        // With FuncCache

        private static object FuncCacheLock = new object();

        public static T GetValueFromMemberExpression_WithFuncCache<T>(Expression<Func<T>> expression, MemberExpression memberExpression)
        {
            Func<T> function;

            object cacheKey = GetMemberExpressionKey(memberExpression);

            lock (FuncCacheLock)
            {
                if (FuncCache<T>.ContainsKey(cacheKey))
                {
                    function = FuncCache<T>.GetItem(cacheKey);
                }
                else
                {
                    function = expression.Compile();
                    FuncCache<T>.SetItem(cacheKey, function);
                }
            }
            T value = function();
            return value;
        }

        private static string guid = Guid.NewGuid().ToString();

        private static object GetMemberExpressionKey(MemberExpression memberExpression)
        {
            //return memberExpression.ToString();
            object constant = GetOuterMostConstant(memberExpression);
            return memberExpression.ToString() + guid + constant.GetHashCode();
        }

        private static object GetOuterMostConstant(Expression expression)
        {
            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
            {
                return constantExpression.Value;
            }

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                return GetOuterMostConstant(memberExpression.Expression);
            }

            throw new Exception("OuterMostConstantExpression could not be retrieved.");
        }

        // With MemberInfos

        public static object GetValueFromMemberExpression_WithMemberInfos(MemberExpression memberExpression)
        {
            var members = new List<MemberInfo>();

            object constant = GetOuterMostConstantAndAddMembers(memberExpression, members);

            object value = constant;

            foreach (MemberInfo member in members)
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

                    case MemberTypes.Method:
                        throw new NotSupportedException("Retrieving values from expressions with method calls in it, is not supported.");
                }
            }

            return value;
        }

        private static object GetOuterMostConstantAndAddMembers(Expression expression, List<MemberInfo> members)
        {
            var constantExpression = expression as ConstantExpression;
            if (constantExpression != null)
            {
                return constantExpression.Value;
            }

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                members.Insert(0, memberExpression.Member);
                return GetOuterMostConstantAndAddMembers(memberExpression.Expression, members);
            }

            throw new Exception("OuterMostConstantExpression could not be retrieved.");
        }
    }
}