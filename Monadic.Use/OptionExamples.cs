using System;
using Monadic.Low;

namespace Monadic.Use
{
    public class OptionExamples
    {
        public IOption<string> Simple(string name)
        {
            switch (name)
            {
                case null:

                case "foo":
                    return Option.Success("bar");
                case "bar":
                    return Option.Failure<string>("Only foo allowed");
                default:
                    return Option<string>.Failure();
            }
        }

        public IOption<int> GetById(int id = 0)
        {
            return Option.Success(42);
        }

        public double GetStrict(string name, int id)
        {
            return 3.141;
        }

        public double Then()
        {
            return Simple(null)
            .AndThen(() => GetById(2))
            .Map(GetStrict).Or(Math.E);
        }

        public static double FinalStep(double d1, double d2, double d3)
        {
            throw new NotImplementedException();
        }

        public object Convoluted()
        {
            return Simple("Not")
                .AndThen(() => GetById(2))
                .Map(GetStrict)
                .And(Then)
                .And(Then)
                .Map(FinalStep)
                .Map(_ => (int)_)
                .OrElse(GetById())
                .Or(() => GetById().Or(42));
        }

        public IOption<string> QuerySyntax()
        {
            return
                from i in GetById()
                where i == 42
                from j in Simple("")
                let x = GetStrict(j, i)
                select $"({i},{j}) = {x}";
        }
    }
}
