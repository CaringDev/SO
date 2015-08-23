using System;
using Xunit;

namespace Monadic.Test
{
    public class StaticOptionTest
    {
        [Fact]
        public void SuccessCanBeCreated()
        {
            var opt = Option.Success("");
            Assert.NotNull(opt);
        }

        [Fact]
        public void FailureCanBeCreatedWithMessage()
        {
            var opt = Option.Failure<object>("");
            Assert.NotNull(opt);
        }

        [Fact]
        public void FailureCanBeCreatedWithAnException()
        {
            var opt = Option.Failure<object>(new NotImplementedException());
            Assert.NotNull(opt);
        }

        [Fact]
        public void TryPassesThroughSuccess()
        {
            var expected = 42;
            var actual = Option.Try(() => expected);
            Assert.Equal(expected, actual.Value);
        }

        [Fact]
        public void TryWrapsNull()
        {
            var actual = Option.Try<object>(() => null);
            Assert.False(actual.HasValue);
        }

        [Fact]
        public void TryWrapsException()
        {
            var actual = Option.Try<object>(() => { throw new ApplicationException(); });
            Assert.False(actual.HasValue);
        }
    }
}
