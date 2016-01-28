using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoWhat.Domain.User;
using WhoWhat.Domain.User.Commands;
using WhoWhat.Domain.User.Events;

namespace WhoWhat.Tests.Domain
{
    [TestClass]
    public class BT_UserAggregate : BehaviourTestBase<UserAggregate>
    {
        #region Predefined Events

        private UserRegistered UserRegisteredEvent(string userId)
        {
            return new UserRegistered
            {
                AggregateId = userId,

                FirstName = "Tolik",
                LastName = "Anabolik",

                PhotoBigUri = "http://code9.biz/profile/image123_big.png",
                PhotoSmallUri = "http://code9.biz/profile/image123_small.png",

                ThirdPartyId = _idGenerator.Generate(),
                AccessToken = _idGenerator.Generate(),
                LoginType = UserLoginType.Vk
            };
        }

        #endregion

        #region General

        [TestMethod]
        public void WhenCommand_RegisterUser_ShouldBeEvent_UserRegistered()
        {
            string userId = _idGenerator.Generate();

            Given();

            When(new RegisterUser()
            {
                AggregateId = userId,

                FirstName = "Tolik",
                LastName = "Anabolik",

                PhotoBigUri = "http://code9.biz/profile/image123_big.png",
                PhotoSmallUri = "http://code9.biz/profile/image123_small.png",

                ThirdPartyId = "12456",
                AccessToken = "",
                LoginType = UserLoginType.Vk
                
            }, (aggregate, command) => aggregate.When(command));

            Expected(new UserRegistered
            {
                AggregateId = userId,

                FirstName = "Tolik",
                LastName = "Anabolik",

                PhotoBigUri = "http://code9.biz/profile/image123_big.png",
                PhotoSmallUri = "http://code9.biz/profile/image123_small.png",

                ThirdPartyId = "12456",
                AccessToken = "",
                LoginType = UserLoginType.Vk
            });
        }

        [TestMethod]
        public void WhenCommand_UpdateUserRegistration_ShouldBeEvent_UserRegistrationUpdated()
        {
            string userId = _idGenerator.Generate();

            Given(UserRegisteredEvent(userId));

            When(new UpdateUserRegistration()
            {
                AggregateId = userId,

                FirstName = "Grishka",
                LastName = "Kovrizhka",

                PhotoBigUri = "http://code9.biz/profile/image1234_big.png",
                PhotoSmallUri = "http://code9.biz/profile/image1234_small.png",

                AccessToken = "232",

            }, (aggregate, command) => aggregate.When(command));

            Expected(new UserRegistrationUpdated()
            {
                AggregateId = userId,

                FirstName = "Grishka",
                LastName = "Kovrizhka",

                PhotoBigUri = "http://code9.biz/profile/image1234_big.png",
                PhotoSmallUri = "http://code9.biz/profile/image1234_small.png",

                AccessToken = "232",
            });
        }

        [TestMethod]
        public void WhenCommand_SubscribeUserToPushups_ShouldBeEvent_UserSubscribedToPushups()
        {
            string userId = _idGenerator.Generate();

            Given(UserRegisteredEvent(userId));

            When(new SubscribeUserToPushups()
            {
                AggregateId = userId,

                DeviceOs = DeviceOS.WP,
                Token = ""
                
            }, (aggregate, command) => aggregate.When(command));

            Expected(new UserSubscribedToPushups()
            {
                AggregateId = userId,

                DeviceOs = DeviceOS.WP,
                Token = ""
            });
        }

        #endregion

        #region Reputation

        [TestMethod]
        public void WhenCommand_RaiseUserReputation_ShouldBeEvent_UserReputationRaised()
        {
            string userId = _idGenerator.Generate();

            Given(UserRegisteredEvent(userId));

            When(new ChangeUserReputation()
            {
                AggregateId = userId,
                Shift = 11

            }, (aggregate, command) => aggregate.When(command));

            Expected(new UserReputationChanged()
            {
                AggregateId = userId,
                Shift = 11
            });
        }

        [TestMethod]
        public void WhenCommand_ReduceUserReputation_ShouldBeEvent_UserReputationReduced()
        {
            string userId = _idGenerator.Generate();

            Given(UserRegisteredEvent(userId));

            When(new ChangeUserReputation()
            {
                AggregateId = userId,
                Shift = -12

            }, (aggregate, command) => aggregate.When(command));

            Expected(new UserReputationChanged()
            {
                AggregateId = userId,
                Shift = -12
            });
        }

        #endregion
    }
}
