using System;

namespace Monadic
{
    using System.Diagnostics.Contracts;
    using System.Runtime.ExceptionServices;

    public sealed class Option<TValue> : IOption<TValue>
    {
        private readonly Exception exception;

        private readonly TValue value;
        private Option(TValue value)
        {
            this.value = value;
            this.HasValue = true;
            this.Message = string.Empty;
        }

        private Option(string message)
        {
            this.Message = message;
            this.exception = new InvalidOperationException(message);
            this.HasValue = false;
        }

        private Option(Exception exception)
        {
            this.exception = exception;
            this.Message = exception.Message;
            this.HasValue = false;
        }

        public bool HasValue { get; }

        public TValue Value
        {
            get
            {
                if (this.HasValue)
                {
                    return this.value;
                }

                ExceptionDispatchInfo.Capture(this.exception).Throw();
                return default(TValue);
            }
        }

        public IOption<TValue> Where(Func<TValue, bool> predicate)
        {
            return this.Bind(_ => predicate(_) ? this : Failure());
        }

        public IOption<T> Select<T>(Func<TValue, T> selector)
        {
            return this.Map(selector);
        }

        public IOption<TOut> SelectMany<TOut>(Func<TValue, IOption<TOut>> binder)
        {
            return this.Bind(binder);
        }

        public IOption<TOut> SelectMany<TMid, TOut>(Func<TValue, IOption<TMid>> binder, Func<TValue, TMid, TOut> selector)
        {
            return this.Bind(t => binder(t).Map(u => selector(t, u)));
        }

        public string Message { get; }

        public static IOption<TValue> Success(TValue value)
        {
            return new Option<TValue>(value);
        }

        public static IOption<TValue> Failure(string message = "")
        {
            Contract.Requires(message != null);
            return new Option<TValue>(message);
        }

        public static IOption<TValue> Failure(Exception exception)
        {
            Contract.Requires(exception != null);
            return new Option<TValue>(exception);
        }
    }

    public static class Option
    {
        public static IOption<T> Success<T>(T value)
        {
            return Option<T>.Success(value);
        }

        public static IOption<T> Failure<T>(string message)
        {
            return Option<T>.Failure(message);
        }

        public static IOption<T> Failure<T>(Exception exception)
        {
            return Option<T>.Failure(exception);
        }

        public static IOption<TOut> Map<TIn, TOut>(this IOption<TIn> option, Func<TIn, TOut> mapper)
        {
            return option.HasValue ? Success(mapper(option.Value)) : Failure<TOut>(option.Message);
        }

        public static IOption<TOut> Bind<TIn, TOut>(this IOption<TIn> option, Func<TIn, IOption<TOut>> binder)
        {
            return option.HasValue ? binder(option.Value) : Failure<TOut>(option.Message);
        }

        public static IOption<Tuple<T1, T2>> And<T1, T2>(this IOption<T1> option, Func<T2> next)
        {
            return option.Map(_ => new Tuple<T1, T2>(_, next()));
        }

        public static IOption<Tuple<T1, T2>> AndThen<T1, T2>(this IOption<T1> option, Func<IOption<T2>> next)
        {
            return option.Bind(t => next().Map(u => new Tuple<T1, T2>(t, u)));
        }

        public static IOption<TOut> Map2<T1, T2, TOut>(
            this IOption<Tuple<T1, T2>> option,
            Func<T1, T2, TOut> mapper)
        {
            return option.Map(_ => mapper(_.Item1, _.Item2));
        }

        public static IOption<TOut> Map3<T1, T2, T3, TOut>(
            this IOption<Tuple<Tuple<T1, T2>, T3>> option,
            Func<T1, T2, T3, TOut> mapper)
        {
            return option.Map(_ => mapper(_.Item1.Item1, _.Item1.Item2, _.Item2));
        }

        public static T Or<T>(this IOption<T> option, T t)
        {
            return option.HasValue ? option.Value : t;
        }

        public static T Or<T>(this IOption<T> option, Func<T> t)
        {
            return option.HasValue ? option.Value : t();
        }

        public static IOption<T> OrElse<T>(this IOption<T> option, IOption<T> t)
        {
            return option.HasValue ? option : t;
        }

        public static IOption<T> OrElse<T>(this IOption<T> option, Func<IOption<T>> t)
        {
            return option.HasValue ? option : t();
        }

        public static IOption<IOption<T>> Materialize<T>(this IOption<T> option)
        {
            return Success(option);
        }

        public static IOption<T> Do<T>(this IOption<T> option, Action<IOption<T>> probe)
        {
            probe(option);
            return option;
        }
    }
}
