using System;
using System.Diagnostics.Contracts;

namespace Monadic
{
    public static class OptionExtensions
    {
        public static IOption<TOut> Map<TIn, TOut>(this IOption<TIn> option, Func<TIn, TOut> mapper)
        {
            Contract.Requires(option != null);
            Contract.Requires(mapper != null);
            Contract.Ensures(Contract.Result<IOption<TOut>>() != null);

            if (!option.HasValue)
            {
                return Option.Failure<TOut>(option.Message);
            }

            var result = mapper(option.Value);

            if (result == null)
            {
                throw new ArgumentException("A mapper must return non-null. Consider using Bind.", nameof(mapper));
            }

            return Option.Success(result);
        }

        public static IOption<TOut> Bind<TIn, TOut>(this IOption<TIn> option, Func<TIn, IOption<TOut>> binder)
        {
            Contract.Requires(option != null);
            Contract.Requires(binder != null);
            Contract.Ensures(Contract.Result<IOption<TOut>>() != null);

            if (!option.HasValue)
            {
                return Option.Failure<TOut>(option.Message);
            }

            var result = binder(option.Value);

            if (result == null)
            {
                throw new ArgumentException("A binder must return non-null. Consider returning a Failure.", nameof(binder));
            }

            return result;
        }

        public static T Or<T>(this IOption<T> option, T t)
        {
            Contract.Requires(option != null);

            return option.HasValue ? option.Value : t;
        }

        public static T Or<T>(this IOption<T> option, Func<T> t)
        {
            Contract.Requires(option != null);
            Contract.Requires(t != null);

            return option.HasValue ? option.Value : t();
        }

        public static IOption<T> Or<T>(this IOption<T> option, IOption<T> alternative)
        {
            Contract.Requires(option != null);
            Contract.Requires(alternative != null);
            Contract.Ensures(Contract.Result<IOption<T>>() != null);

            return option.HasValue ? option : alternative;
        }

        public static IOption<T> Or<T>(this IOption<T> option, Func<IOption<T>> alternative)
        {
            Contract.Requires(option != null);
            Contract.Requires(alternative != null);
            Contract.Ensures(Contract.Result<IOption<T>>() != null);

            if (option.HasValue)
            {
                return option;
            }

            var result = alternative();

            if (result == null)
            {
                throw new ArgumentException("The alternative must always be non-null. Consider returning a Failure.", nameof(alternative));
            }

            return result;
        }

        public static IOption<IOption<T>> Materialize<T>(this IOption<T> option)
        {
            Contract.Requires(option != null);

            return Option.Success(option);
        }

        public static IOption<T> Do<T>(this IOption<T> option, Action<IOption<T>> probe)
        {
            Contract.Requires(option != null);
            Contract.Requires(probe != null);
            Contract.Ensures(Contract.Result<IOption<T>>() != null);

            probe(option);
            return option;
        }
    }

    namespace Low
    {
        public static class OptionExtensions
        {
            public static IOption<Tuple<T1, T2>> And<T1, T2>(
                this IOption<T1> option, Func<T2> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2>>>() != null);

                return option.Map(_ => new Tuple<T1, T2>(_, next()));
            }


            public static IOption<Tuple<T1, T2, T3>> And<T1, T2, T3>(
                this IOption<Tuple<T1, T2>> option, Func<T3> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3>>>() != null);

                return option.Map(_ => new Tuple<T1, T2, T3>(_.Item1, _.Item2, next()));
            }

            public static IOption<Tuple<T1, T2, T3, T4>> And<T1, T2, T3, T4>(
                this IOption<Tuple<T1, T2, T3>> option, Func<T4> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3, T4>>>() != null);

                return option.Map(_ => new Tuple<T1, T2, T3, T4>(_.Item1, _.Item2, _.Item3, next()));
            }

            public static IOption<Tuple<T1, T2, T3, T4, T5>> And<T1, T2, T3, T4, T5>(
                this IOption<Tuple<T1, T2, T3, T4>> option, Func<T5> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3, T4, T5>>>() != null);

                return option.Map(_ => new Tuple<T1, T2, T3, T4, T5>(_.Item1, _.Item2, _.Item3, _.Item4, next()));
            }

            public static IOption<Tuple<T1, T2, T3, T4, T5, T6>> And<T1, T2, T3, T4, T5, T6>(
                this IOption<Tuple<T1, T2, T3, T4, T5>> option, Func<T6> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3, T4, T5, T6>>>() != null);

                return option.Map(_ => new Tuple<T1, T2, T3, T4, T5, T6>(_.Item1, _.Item2, _.Item3, _.Item4, _.Item5, next()));
            }

            public static IOption<Tuple<T1, T2, T3, T4, T5, T6, T7>> And<T1, T2, T3, T4, T5, T6, T7>(
                this IOption<Tuple<T1, T2, T3, T4, T5, T6>> option, Func<T7> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3, T4, T5, T6, T7>>>() != null);

                return option.Map(_ => new Tuple<T1, T2, T3, T4, T5, T6, T7>(_.Item1, _.Item2, _.Item3, _.Item4, _.Item5, _.Item6, next()));
            }

            public static IOption<Tuple<T1, T2>> And<T1, T2>(
                this IOption<T1> option, Func<IOption<T2>> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2>>>() != null);

                return option.Bind(t => next().Map(u => new Tuple<T1, T2>(t, u)));
            }

            public static IOption<Tuple<T1, T2, T3>> And<T1, T2, T3>(
                this IOption<Tuple<T1, T2>> option, Func<IOption<T3>> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3>>>() != null);

                return option.Bind(t => next().Map(u => new Tuple<T1, T2, T3>(t.Item1, t.Item2, u)));
            }

            public static IOption<Tuple<T1, T2, T3, T4>> And<T1, T2, T3, T4>(
                this IOption<Tuple<T1, T2, T3>> option, Func<IOption<T4>> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3, T4>>>() != null);

                return option.Bind(t => next().Map(u => new Tuple<T1, T2, T3, T4>(t.Item1, t.Item2, t.Item3, u)));
            }

            public static IOption<Tuple<T1, T2, T3, T4, T5>> And<T1, T2, T3, T4, T5>(
                this IOption<Tuple<T1, T2, T3, T4>> option, Func<IOption<T5>> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3, T4, T5>>>() != null);

                return option.Bind(t => next().Map(u => new Tuple<T1, T2, T3, T4, T5>(t.Item1, t.Item2, t.Item3, t.Item4, u)));
            }

            public static IOption<Tuple<T1, T2, T3, T4, T5, T6>> And<T1, T2, T3, T4, T5, T6>(
                this IOption<Tuple<T1, T2, T3, T4, T5>> option, Func<IOption<T6>> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3, T4, T5, T6>>>() != null);

                return option.Bind(t => next().Map(u => new Tuple<T1, T2, T3, T4, T5, T6>(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, u)));
            }

            public static IOption<Tuple<T1, T2, T3, T4, T5, T6, T7>> And<T1, T2, T3, T4, T5, T6, T7>(
                this IOption<Tuple<T1, T2, T3, T4, T5, T6>> option, Func<IOption<T7>> next)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<Tuple<T1, T2, T3, T4, T5, T6, T7>>>() != null);

                return option.Bind(t => next().Map(u => new Tuple<T1, T2, T3, T4, T5, T6, T7>(t.Item1, t.Item2, t.Item3, t.Item4, t.Item5, t.Item6, u)));
            }

            public static IOption<TOut> Map<T1, T2, TOut>(
                this IOption<Tuple<T1, T2>> option, Func<T1, T2, TOut> mapper)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<TOut>>() != null);

                return option.Map(_ => mapper(_.Item1, _.Item2));
            }

            public static IOption<TOut> Map<T1, T2, T3, TOut>(
                this IOption<Tuple<T1, T2, T3>> option, Func<T1, T2, T3, TOut> mapper)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<TOut>>() != null);

                return option.Map(_ => mapper(_.Item1, _.Item2, _.Item3));
            }

            public static IOption<TOut> Map<T1, T2, T3, T4, TOut>(
                this IOption<Tuple<T1, T2, T3, T4>> option, Func<T1, T2, T3, T4, TOut> mapper)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<TOut>>() != null);

                return option.Map(_ => mapper(_.Item1, _.Item2, _.Item3, _.Item4));
            }

            public static IOption<TOut> Map<T1, T2, T3, T4, T5, TOut>(
                this IOption<Tuple<T1, T2, T3, T4, T5>> option, Func<T1, T2, T3, T4, T5, TOut> mapper)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<TOut>>() != null);

                return option.Map(_ => mapper(_.Item1, _.Item2, _.Item3, _.Item4, _.Item5));
            }

            public static IOption<TOut> Map<T1, T2, T3, T4, T5, T6, TOut>(
                this IOption<Tuple<T1, T2, T3, T4, T5, T6>> option, Func<T1, T2, T3, T4, T5, T6, TOut> mapper)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<TOut>>() != null);

                return option.Map(_ => mapper(_.Item1, _.Item2, _.Item3, _.Item4, _.Item5, _.Item6));
            }

            public static IOption<TOut> Map<T1, T2, T3, T4, T5, T6, T7, TOut>(
                this IOption<Tuple<T1, T2, T3, T4, T5, T6, T7>> option, Func<T1, T2, T3, T4, T5, T6, T7, TOut> mapper)
            {
                Contract.Requires(option != null);
                Contract.Ensures(Contract.Result<IOption<TOut>>() != null);

                return option.Map(_ => mapper(_.Item1, _.Item2, _.Item3, _.Item4, _.Item5, _.Item6, _.Item7));
            }
        }
    }
}