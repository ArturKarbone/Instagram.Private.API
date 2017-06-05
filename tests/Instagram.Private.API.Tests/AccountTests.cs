using Shouldly;
using System.Linq;
using Xunit;

namespace Instagram.Private.API.Tests
{
    ///<remarks>
    ///dotnet test --filter FullyQualifiedName~Instagram.Private.API.Tests.AccountTests
    ///</remarks>
    public class AccountTests
    {
        [Fact]
        [Trait("Account", "Functional")]
        ///<remarks
        ///dotnet test --filter "FullyQualifiedName=Instagram.Private.API.Tests.AccountTests.Should_Search_For_Users"
        ///dotnet test --filter FullyQualifiedName=Instagram.Private.API.Tests.AccountTests.Should_Search_For_Users
        ///</remarks>       
        public void Should_Search_For_Users()
        {
            var res = Fixtures.Clients.Default
               .Account
               .Search("test").Result;

            res.status.ShouldBe("ok");
            res.users.ShouldNotBeEmpty();
        }

        [Fact]
        public void Should_Search_For_Concrete_User()
        {
            Fixtures.Clients.Default
              .Account
              .SearchUser("test").Result
              .username
              .ShouldBe("test");
        }
    }
}