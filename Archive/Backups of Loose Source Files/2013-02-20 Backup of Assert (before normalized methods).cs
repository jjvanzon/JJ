//
//  Circle.Framework.Testing.Assert
//
//      Author: Jan-Joost van Zon
//      Date: 2011-09-15 - 2011-09-15
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Framework.Coding.Conditions;
using System.Threading;
using System.Linq.Expressions;
using Circle.Framework.Integration.DotNet.Expressions;

namespace Circle.Framework.Testing
{
    public static class Assert
    {
        public static void NotEqual<T>(T a, T b, string message = "")
        {
            if (Object.Equals(a, b))
            {
                string fullMessage = Assert.GetNotEqualFailedMessage(a, b, message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void NotEqual<T>(T a, Expression<Func<T>> bExpresion)
        {
            T b = ExpressionHelper.GetValue(bExpresion);

            if (Object.Equals(a, b))
            {
                string name = ExpressionHelper.GetName(bExpresion);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetNotEqualFailedMessage(a, b, message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void AreEqual<T>(T expected, T actual, string message = "")
        {
            Assert.ExpectedActual(expected, actual, Object.Equals(expected, actual), "AreEqual", message);

            //if (!Object.Equals(expected, actual))
            //{
            //    string fullMessage = Assert.GetExpectedActualMessage("AreEqual", expected, actual, message);
            //    throw new AssertFailedException(fullMessage);
            //}
        }

        public static void AreEqual<T>(T expected, Expression<Func<T>> actualExpression)
        {
            Assert.ExpectedActual(expected, actualExpression, (e, a) => Object.Equals(e, a), "AreEqual");
            //T actual = ExpressionHelper.GetValue(actualExpression);
            //if (!Object.Equals(expected, actual))
            //{
            //    string name = ExpressionHelper.GetName(actualExpression);
            //    string message = TestHelper.FormatTestedPropertyMessage(name);
            //    string fullMessage = Assert.GetExpectedActualMessage("AreEqual", expected, actual, message);
            //    throw new AssertFailedException(fullMessage);
            //}
        }

        private static void ExpectedActual<T>(T expected, T actual, bool condition, string methodName, string message = "")
        {
            if (!condition)
            {
                string fullMessage = Assert.GetExpectedActualMessage(methodName, expected, actual, message);
                throw new AssertFailedException(fullMessage);
            }
        }

        private static void ExpectedActual<T>(T expected, Expression<Func<T>> actualExpression, Func<T, T, bool> condition, string methodName)
        {
            T actual = ExpressionHelper.GetValue(actualExpression);
            if (!condition(expected, actual))
            {
                string name = ExpressionHelper.GetName(actualExpression);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetExpectedActualMessage(methodName, expected, actual, message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void AreSame<T>(T expected, T actual, string message = "")
        {
            if (!Object.ReferenceEquals(expected, actual))
            {
                string fullMessage = Assert.GetExpectedActualMessage("AreSame", expected, actual, message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void AreSame<T>(T expected, Expression<Func<T>> actualExpression)
        {
            T actual = ExpressionHelper.GetValue(actualExpression);
            if (!Object.ReferenceEquals(expected, actual))
            {
                string name = ExpressionHelper.GetName(actualExpression);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetExpectedActualMessage("AreSame", expected, actual, message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsTrue(bool condition, string message = "")
        {
            if (!condition)
            {
                string fullMessage = Assert.GetFailureMessage("IsTrue", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsTrue(Expression<Func<bool>> expression)
        {
            bool condition = ExpressionHelper.GetValue(expression);
            if (!condition)
            {
                string name = ExpressionHelper.GetName(expression);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetFailureMessage("IsTrue", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsFalse(bool condition, string message = "")
        {
            if (condition)
            {
                string fullMessage = Assert.GetFailureMessage("IsFalse", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsFalse(Expression<Func<bool>> expression)
        {
            bool condition = ExpressionHelper.GetValue(expression);
            if (condition)
            {
                string name = ExpressionHelper.GetName(expression);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetFailureMessage("IsFalse", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsNull(object obj, string message = "")
        {
            if (obj != null)
            {
                string fullMessage = Assert.GetFailureMessage("IsNull", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsNull(Expression<Func<object>> expression)
        {
            object obj = ExpressionHelper.GetValue(expression);
            if (obj != null)
            {
                string name = ExpressionHelper.GetName(expression);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetFailureMessage("IsNull", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsNotNull(object obj, string message = "")
        {
            if (obj == null)
            {
                string fullMessage = Assert.GetFailureMessage("IsNotNull", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsNotNull(Expression<Func<object>> expression)
        {
            object obj = ExpressionHelper.GetValue(expression);
            if (obj == null)
            {
                string name = ExpressionHelper.GetName(expression);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetFailureMessage("IsNotNull", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void Fail(string message = "")
        {
            string fullMessage = Assert.GetAssertFailFailedMessage(message);
            throw new AssertFailedException(fullMessage);
        }

        public static void ThrowsExceptionOnOtherThread(Action statement)
        {
            bool exceptionWasThrown = false;
            Action action = new Action(() =>
            {
                try
                {
                    statement();
                }
                catch
                {
                    exceptionWasThrown = true;
                }
            });

            ThreadStart threadStart = new ThreadStart(action);
            Thread thread = new Thread(threadStart);
            thread.Start();
            thread.Join();

            if (!exceptionWasThrown)
            {
                Assert.Fail("An exception should have been raised.");
            }
        }

        public static void ThrowsException(Action statement)
        {
            try
            {
                statement();
            }
            catch
            {
                return;
            }

            Assert.Fail("An exception should have been raised.");
        }

        public static void ThrowsException(Action statement, string expectedMessage)
        {
            try
            {
                statement();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedMessage, ex.Message);
                return;
            }

            Assert.Fail("An exception should have been raised.");
        }

        public static void ThrowsException(Action statement, Type exceptionType)
        {
            try
            {
                statement();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exceptionType, ex.GetType());
                return;
            }

            Assert.Fail("An exception should have been raised.");
        }

        public static void ThrowsException(Action statement, Type exceptionType, string expectedMessage)
        {
            try
            {
                statement();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exceptionType, ex.GetType());
                Assert.AreEqual(expectedMessage, ex.Message);
                return;
            }

            Assert.Fail("An exception should have been raised.");
        }

        public static void ThrowsException<ExceptionType>(Action statement)
        {
            Assert.ThrowsException(statement, typeof(ExceptionType));
        }

        public static void ThrowsException<ExceptionType>(Action statement, string expectedMessage)
        {
            Assert.ThrowsException(statement, typeof(ExceptionType), expectedMessage);
        }

        public static void IsNullOrEmpty(string value, string message = "")
        {
            if (!String.IsNullOrEmpty(value))
            {
                string fullMessage = Assert.GetFailureMessage("IsNullOrEmpty", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsNullOrEmpty(Expression<Func<string>> expression)
        {
            string value = ExpressionHelper.GetValue(expression);
            if (!String.IsNullOrEmpty(value))
            {
                string name = ExpressionHelper.GetName(expression);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetFailureMessage("IsNullOrEmpty", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void NotNullOrEmpty(string value, string message = "")
        {
            if (String.IsNullOrEmpty(value))
            {
                string fullMessage = Assert.GetFailureMessage("NotNullOrEmpty", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void NotNullOrEmpty(Expression<Func<string>> expression)
        {
            string value = ExpressionHelper.GetValue(expression);
            if (String.IsNullOrEmpty(value))
            {
                string name = ExpressionHelper.GetName(expression);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetFailureMessage("NotNullOrEmpty", message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsOfType<T>(object obj, string message)
        {
            Condition.NotNull(() => obj);
            Type expected = typeof(T);
            Type actual = obj.GetType();

            if (expected != actual)
            {
                string fullMessage = Assert.GetExpectedActualMessage("IsOfType", expected, actual, message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void IsOfType<T>(Expression<Func<object>> expression)
        {
            object obj = ExpressionHelper.GetValue(expression);

            Condition.NotNull(() => obj);
            Type expected = typeof(T);
            Type actual = obj.GetType();

            if (expected != actual)
            {
                string name = ExpressionHelper.GetName(expression);
                string message = TestHelper.FormatTestedPropertyMessage(name);
                string fullMessage = Assert.GetExpectedActualMessage("IsOfType", expected, actual, message);
                throw new AssertFailedException(fullMessage);
            }
        }

        public static void Inconclusive(string message = null)
        {
            throw new AssertInconclusiveException(message);
        }

        private static string GetNotEqualFailedMessage<T>(T a, T b, string message)
        {
            return
                String.Format("Assert.NotEqual failed. Both values are <{0}>.{1}{2}",
                    a != null ? a.ToString() : "null",
                    !String.IsNullOrEmpty(message) ? " " : "",
                    message);
        }

        private static string GetExpectedActualMessage<T>(string methodName, T expected, T actual, string message)
        {
            return
                String.Format("Assert.{0} failed. Expected <{1}>, Actual <{2}>.{3}{4}",
                    methodName,
                    expected != null ? expected.ToString() : "null",
                    actual != null ? actual.ToString() : "null",
                    !String.IsNullOrEmpty(message) ? " " : "",
                    message);
        }

        private static string GetFailureMessage(string methodName, string message)
        {
            return
                String.Format("Assert.{0} failed.{1}{2}",
                    methodName,
                    !String.IsNullOrEmpty(message) ? " " : "",
                    message);
        }

        private static string GetAssertFailFailedMessage(string message)
        {
            return
                String.Format("Assert failed.{0}{1}",
                    !String.IsNullOrEmpty(message) ? " " : "",
                    message);
        }
    }
}
