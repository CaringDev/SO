using System;

namespace Monadic
{
    using Xunit;

    public class OptionTests
    {
        [Fact]
        public void ItsFailureIncludesThePreviousStackTrace()
        {
            var throwSite = typeof(OptionExamples).FullName;
            var stackTrace = Assert.Throws<ArgumentNullException>(() => new OptionExamples().Simple(null).Value).StackTrace;
            Assert.Contains(throwSite, stackTrace);
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
