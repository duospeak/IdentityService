using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UnitTests.Internal
{
    static class MockReturnDefaults<T>
    {
        public static Func<T, CancellationToken, T> ItselfReturns => (T source, CancellationToken ct) => source;
    }
}
