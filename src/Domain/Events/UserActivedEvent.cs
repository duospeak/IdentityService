using Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public class UserActivedEvent : DomainEvent
    {
        public UserActivedEvent(string previousStatus, long userId, string userName)
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
