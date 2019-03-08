using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public class UserCreatedEvent : DomainEvent
    {
        public UserCreatedEvent(long userId, string userName, DateTimeOffset createTime)
        {
            UserId = userId;
            UserName = userName;
            CreateTime = createTime;
        }

        public long UserId { get; }

        public string UserName { get; }

        public DateTimeOffset CreateTime { get; }
    }
}
