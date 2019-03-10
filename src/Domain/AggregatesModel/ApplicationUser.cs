using Domain.Enumerations;
using Domain.Events;
using Domain.Exceptions;
using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AggregatesModel
{
    public class ApplicationUser : Entity, IEntity, IAggregateRoot
    {
        // constructor used by EntityFrameworkCore
        private ApplicationUser() { }

        public ApplicationUser(string userName, string password)
        {
            Id = SnowflakeId.Default().NextId();
            UserName = userName.NotNull(nameof(userName));
            Password = password.NotNull(nameof(password));
            CreateTime = DateTimeOffset.UtcNow;
            Status = UserStatus.AwaitingActive;
            AddDomainEvent(new UserCreatedEvent(Id, UserName, CreateTime));
        }
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public DateTimeOffset CreateTime { get; private set; }

        public UserStatus Status { get; private set; }

        public void Active()
        {
            if (Status == UserStatus.Active)
            {
                throw new UserDomainException(UserErrorCodes.StatusError, "user is already actived");
            }

            AddDomainEvent(new UserActivedEvent(Status.ToString(), Id, UserName));

            Status = UserStatus.Active;
        }

        public void Block()
        {
            if (Status != UserStatus.Active)
            {
                throw new UserDomainException(UserErrorCodes.StatusError, "user have never been actived");
            }

            AddDomainEvent(new UserBlockedEvent(Status.ToString(), Id, UserName));

            Status = UserStatus.Blocked;
        }

        public override string ToString()
        {
            return $"Id:{Id}  UserName:{UserName}  CreateTime:{CreateTime}";
        }
    }
}
