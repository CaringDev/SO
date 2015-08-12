namespace Monadic
{
    using System;

    public interface IOption<out TValue>
    {
        bool HasValue { get; }
        string Message { get; }
        TValue Value { get; }

        IOption<TValue> Where(Func<TValue, bool> predicate);

        IOption<T> Select<T>(Func<TValue, T> selector);

        IOption<TOut> SelectMany<TOut>(Func<TValue, IOption<TOut>> binder);

        IOption<TOut> SelectMany<TMid, TOut>(Func<TValue, IOption<TMid>> binder, Func<TValue, TMid, TOut> selector);
    }
}