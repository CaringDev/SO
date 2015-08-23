using System;
using Xunit;

namespace Monadic.Test
{
    public class OptionTest
    {
        [Fact]
        public void SuccessCanBeCreated()
        {
            var opt = Option<string>.Success("");
            Assert.NotNull(opt);
        }

        [Fact]
        public void SuccessHasValue()
        {
            var opt = Option<int>.Success(42);
            Assert.True(opt.HasValue);
        }

        [Fact]
        public void SuccessValueIsOriginal()
        {
            var expected = new object();
            var opt = Option<object>.Success(expected);
            Assert.Same(expected, opt.Value);
        }

        [Fact]
        public void FailureDoesNotHaveValue()
        {
            var opt = Option<object>.Failure();
            Assert.False(opt.HasValue);
        }

        [Fact]
        public void AccessFailureValueThrowsException()
        {
            var opt = Option<object>.Failure();
            Assert.ThrowsAny<Exception>(() => opt.Value);
        }

        [Fact]
        public void FailureContainsMessage()
        {
            var expected = "Oops";
            var opt = Option<object>.Failure(expected);
            Assert.Same(expected, opt.Message);
        }

        [Fact]
        public void FailureIncludesTheOriginalStackTrace()
        {
            var className = typeof(ThrowingClass).Name;
            var methodName = nameof(ThrowingClass.ThrowAndCatch);
            var stackTrace = Assert.Throws<ApplicationException>(() => ThrowingClass.ThrowAndCatch().Value).StackTrace;
            Assert.Contains($"{className}.{methodName}", stackTrace);
        }

        [Fact]
        public void SupportsLinqSelect()
        {
            const string expected = "Foo";
            var actual =
                from o in Option<string>.Success(expected)
                select o;

            Assert.Same(expected, actual.Value);
        }

        [Fact]
        public void SupportsLinqWhereTrue()
        {
            var actual =
                from o in Option<string>.Success("Foo")
                where o.Contains("Foo")
                select o;

            Assert.True(actual.HasValue);
        }

        [Fact]
        public void SupportsLinqWhereFalse()
        {
            var actual =
                from o in Option<string>.Success("Foo")
                where o.Contains("Bar")
                select o;

            Assert.False(actual.HasValue);
        }

        [Fact]
        public void SupportsLinqSelectMany()
        {
            var actual =
                from o in Option<double>.Success(3.14)
                from i in Option<int>.Success(42)
                select o + i;

            Assert.Equal(45.14, actual.Value);
        }

        [Fact]
        public void SupportsCovariance()
        {
             var opt = Option<Derived>.Failure();
            Assert.IsAssignableFrom<IOption<Base>>(opt);
        }

        [Fact]
        public void SupportsContravariance()
        {
            Action<IOption<Base>> action = _ => {};
            var opt = Option<Derived>.Failure();
            action(opt);
        }

        private class ThrowingClass
        {
            public static IOption<object> ThrowAndCatch()
            {
                try
                {
                    throw new ApplicationException();
                }
                catch (Exception ex)
                {
                    return Option<string>.Failure(ex);
                }
            }
        }

        private class Base { }

        private class Derived : Base { }
    }
}
