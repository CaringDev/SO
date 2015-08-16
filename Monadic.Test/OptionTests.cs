using System;
using Xunit;

namespace Monadic.Test
{
    public class OptionTests
    {
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

        [Fact]
        public void ItsFailureIncludesThePreviousStackTrace()
        {
            var className = typeof(ThrowingClass).Name;
            var methodName = nameof(ThrowingClass.ThrowAndCatch);
            var stackTrace = Assert.Throws<ApplicationException>(() => ThrowingClass.ThrowAndCatch().Value).StackTrace;
            Assert.Contains($"{className}.{methodName}", stackTrace);
        }

        [Fact]
        public void ItCanRecover()
        {
            var expected = 42;
            var actual = Option<int>.Failure("Yuck!").Or(expected);
            Assert.Equal(expected, actual);
        }
    }
}
