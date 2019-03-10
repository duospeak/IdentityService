using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Internal
{
    public class TestOptions<T> : IOptions<T>
        where T : class, new()
    {
        public TestOptions(T value) => Value = value;

        public T Value { get; }
    }
}
