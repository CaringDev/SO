using Xunit;

namespace Monadic.Test
{
    public class OptionExtensionTest
    {
        [Fact]
        public void ItCanRecoverUsingValue()
        {
            const int expected = 42;
            var actual = Option<int>.Failure("Yuck!").Or(expected);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ItCanRecoverUsingDelayedValue()
        {
            const int expected = 42;
            var actual = Option<int>.Failure("Yuck!").Or(() => expected);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ItDoesNotCalculateDelayedValueNeedlessly()
        {
            const int expected = 42;
            var actual = Option<int>.Success(expected).Or(() => {
                Assert.False(true);
                return 0;
            });
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ItCanRecoverUsingOption()
        {
            const int expected = 42;
            var actual = Option<int>.Failure("Yuck!").Or(Option<int>.Success(expected));
            Assert.Equal(expected, actual.Value);
        }

        [Fact]
        public void ItCanRecoverUsingDelayedOption()
        {
            const int expected = 42;
            var actual = Option<int>.Failure("Yuck!").Or(() => Option<int>.Success(expected));
            Assert.Equal(expected, actual.Value);
        }

        [Fact]
        public void ItDoesNotCalculateDelayedOptionNeedlessly()
        {
            const int expected = 42;
            var actual = Option<int>.Success(expected).Or(() => {
                Assert.False(true);
                return Option<int>.Success(0);
            });
            Assert.Equal(expected, actual.Value);
        }
    }
}
