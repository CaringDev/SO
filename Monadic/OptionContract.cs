using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Monadic
{
    [ContractClassFor(typeof(IOption<>))]
    [ExcludeFromCodeCoverage]
    internal abstract class OptionContract<TValue> : IOption<TValue>
    {
        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(!((IOption<TValue>)this).HasValue || ((IOption<TValue>)this).Value != null);
        }

        bool IOption<TValue>.HasValue
        {
            get { throw new NotImplementedException(); }
        }

        string IOption<TValue>.Message
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                throw new NotImplementedException();
            }
        }

        TValue IOption<TValue>.Value
        {
            get { throw new NotImplementedException(); }
        }

        IOption<TValue> IOption<TValue>.Where(Func<TValue, bool> predicate)
        {
            throw new NotImplementedException();
        }

        IOption<T> IOption<TValue>.Select<T>(Func<TValue, T> selector)
        {
            Contract.Requires(selector != null);
            throw new NotImplementedException();
        }

        IOption<TOut> IOption<TValue>.SelectMany<TMid, TOut>(Func<TValue, IOption<TMid>> binder, Func<TValue, TMid, TOut> selector)
        {
            throw new NotImplementedException();
        }
    }
}