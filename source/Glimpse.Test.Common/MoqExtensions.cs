using System;
using System.Linq.Expressions;
using Moq;
using Moq.Language.Flow;

namespace Glimpse.Test.Common
{
    public static class MoqExtensions
    {
        public static void VerifyAll<T>(this T obj) where T : class
        {
            Mock.Get(obj).VerifyAll();
        }

        public static void Verify<T, TResult>(this T obj, Expression<Func<T, TResult>> expression) where T : class
        {
            Mock.Get(obj).Verify(expression);
        }

        public static void Verify<T, TResult>(this T obj, Expression<Func<T, TResult>> expression, Times times) where T : class
        {
            Mock.Get(obj).Verify(expression, times);
        }

        public static void Verify<T>(this T obj, Expression<Action<T>> expression) where T : class
        {
            Mock.Get(obj).Verify(expression);
        }

        public static void Verify<T>(this T obj, Expression<Action<T>> expression, Times times) where T : class
        {
            Mock.Get(obj).Verify(expression, times);
        }

        public static ISetup<T> Setup<T>(this T obj, Expression<Action<T>> expression) where T : class
        {
            return Mock.Get(obj).Setup(expression);
        }

        public static ISetup<T, TResult> Setup<T, TResult>(this T obj, Expression<Func<T, TResult>> expression) where T : class
        {
            return Mock.Get(obj).Setup(expression);
        }

        public static void VerifySet<T>(this T obj, Action<T> expression) where T : class
        {
            Mock.Get(obj).VerifySet(expression);
        }
    }
}