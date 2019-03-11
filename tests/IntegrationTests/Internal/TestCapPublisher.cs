using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationTests.Internal
{
    class TestCapPublisher : ICapPublisher
    {
        public ICapTransaction Transaction { get; }

        public void Publish<T>(string name, T contentObj, string callbackName = null)
        {
            // no command
        }
        public Task PublishAsync<T>(string name, T contentObj, string callbackName = null, CancellationToken cancellationToken = default)
        {
            // no command

            return Task.CompletedTask;
        }
    }
}
