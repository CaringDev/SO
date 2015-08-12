namespace Monadic
{
    using System;

    class OptionExamples
    {
        public IOption<string> Simple(string name)
        {
            switch (name)
            {
                case null:
                    try
                    {
                        throw new ArgumentNullException(nameof(name));
                    }
                    catch (Exception ex)
                    {
                        return Option<string>.Failure(ex);
                    }
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
            return this.Simple(null)
            .AndThen(() => this.GetById(2))
            .Map2(this.GetStrict).Or(Math.E);
        }

        public static double FinalStep(double d1, double d2, double d3)
        {
            throw new NotImplementedException();
        }

        public object Convoluted()
        {
            return this.Simple("Not")
                .AndThen(() => this.GetById(2))
                .Map2(this.GetStrict)
                .And(this.Then)
                .And(this.Then)
                .Map3(FinalStep)
                .Map(_ => (int)_)
                .OrElse(this.GetById())
                .Or(() => this.GetById().Or(42));
        }

        public IOption<string> QuerySyntax()
        {
            return
                from i in this.GetById() where i == 42
                from j in this.Simple("")
                let x = this.GetStrict(j, i)
                select $"({i},{j}) = {x}";
        }
    }
}
