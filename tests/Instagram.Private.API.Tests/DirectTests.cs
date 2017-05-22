using Shouldly;
using Xunit;

namespace Instagram.Private.API.Tests
{
    public class DirectTests
    {
        [Fact]
        [Trait("Direct","Functional")]
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

            foreach (var inbox in cursor)
            {
            }
        }
    }
}