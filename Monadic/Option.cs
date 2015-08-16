using System;
using System.Diagnostics.Contracts;
using System.Runtime.ExceptionServices;

namespace Monadic
{
    public sealed class Option<TValue> : IOption<TValue>
    {
        private readonly Exception _exception;

        private readonly TValue _value;
        private Option(TValue value)
        {
            _value = value;
            HasValue = true;
            Message = string.Empty;
        }

        private Option(string message)
        {
            Message = message;
            _exception = new InvalidOperationException(message);
            HasValue = false;
        }

        private Option(Exception exception)
        {
            Contract.Requires(exception != null);
            _exception = exception;
            Message = exception.Message;
            HasValue = false;
        }

        public bool HasValue { get; }

        public TValue Value
        {
            get
            {
                if (HasValue)
                {
                    return _value;
                }

                ExceptionDispatchInfo.Capture(_exception)?.Throw();

                throw _exception;
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

        public IOption<TOut> SelectMany<TOut>(Func<TValue, IOption<TOut>> selector)
        {
            return this.Bind(selector);
        }

        public IOption<TOut> SelectMany<TMid, TOut>(Func<TValue, IOption<TMid>> binder, Func<TValue, TMid, TOut> selector)
        {
            return this.Bind(t => binder(t).Map(u => selector(t, u)));
        }

        public string Message { get; }

        public static IOption<TValue> Success(TValue value)
        {
            Contract.Requires(value != null);
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
            Contract.Requires(value != null);
            return Option<T>.Success(value);
        }

        public static IOption<T> Failure<T>(string message)
        {
            Contract.Requires(message != null);
            return Option<T>.Failure(message);
        }

        public static IOption<T> Failure<T>(Exception exception)
        {
            Contract.Requires(exception != null);
            return Option<T>.Failure(exception);
        }
    }
}
