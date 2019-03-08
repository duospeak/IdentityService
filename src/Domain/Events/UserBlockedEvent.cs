using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public class UserBlockedEvent : DomainEvent
    {
        public UserBlockedEvent(string previousStatus, long userId, string userName)
        {
            PreviousStatus = previousStatus;
            UserId = userId;
            UserName = userName;
        }

        public string PreviousStatus { get; }

        public long UserId { get; }

        public string UserName { get; }
    }
}
