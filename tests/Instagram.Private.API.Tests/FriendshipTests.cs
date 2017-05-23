using Xunit;
using Shouldly;
using System.Linq;

namespace Instagram.Private.API.Tests
{
    [Trait("Friendship", "Functional")]
    public class FrienshipTests
    {
        [Fact]
        public void Should_invite_existing_account()
        {
            Fixtures.Clients.Default
              .Friendship
              .Invite(Fixtures.Accounts.To).Result
              .status
              .ShouldBe("ok");
        }

        [Fact]
        public void Should_get_frienship_with_existing_account()
        {
            Fixtures.Clients.Default
             .Friendship
             .GetFriendshipWith(Fixtures.Accounts.To).Result
             .status
             .ShouldBe("ok");
        }

        [Fact]
        public void Should_uninvite_existing_account()
        {
            Fixtures.Clients.Default
             .Friendship
             .Uninvite(Fixtures.Accounts.To).Result
             .status
             .ShouldBe("ok");
        }

        [Fact]
        public void Should_obtain_followers()
        {
            Fixtures.Clients.Default
             .Friendship
             .FollowersCursor(Fixtures.Accounts.To)
                .Take(1)
                .First()
             .status
             .ShouldBe("ok");
        }
    }
}
