using Shouldly;
using System.Linq;
using Xunit;

namespace Instagram.Private.API.Tests
{
    public class DirectTests
    {
        [Fact]
        [Trait("Direct", "Functional")]
        ///dotnet test --filter "FullyQualifiedName=Instagram.Private.API.Tests.DirectTests.Should_Send_Direct_Message"

        public void Should_Send_Direct_Message()
        {
            Fixtures.Clients.Default
               .Direct
               .SendMessage(Fixtures.Accounts.To, "Hola!").Result
               .status
               .ShouldBe("ok");
        }

        [Fact]
        public void Should_Send_Like()
        {
            Fixtures.Clients.Default
              .Direct
              .SendLike(Fixtures.Accounts.To).Result
              .status
              .ShouldBe("ok");
        }

        [Fact]
        public void Should_Send_Photo()
        {
            Fixtures.Clients.Default
                .Direct
                .SendPhoto(Fixtures.Accounts.To, @"C:\temp\fun.jpg").Result
                .status
                .ShouldBe("ok");
        }


        [Fact]
        public void Should_Query_Inbox()
        {
            var cursor = Fixtures.Clients.Default
                .Direct
                .InboxCursor();

            cursor.Take(3).ToList().ForEach(inboxResponse =>
            {
                inboxResponse.status.ShouldBe("ok");
            });
        }


        [Fact]
        public void Should_Query_Inbox_Items()
        {
            var client = Fixtures.Clients.Default;

            var cursor = client
                .Direct
                .InboxCursor();

            cursor.Take(3).ToList().ForEach(inboxResponse =>
            {
                inboxResponse.status.ShouldBe("ok");

                var threadResponse = client
                    .Direct
                    .TheadCursor(inboxResponse.inbox.threads.First().thread_id)
                    .Take(10)
                    .First();

                threadResponse.status.ShouldBe("ok");
            });
        }
    }
}