using Shouldly;
using System.Linq;
using Xunit;

namespace Instagram.Private.API.Tests
{
    public class MediaTests
    {
        [Fact(Skip = "used ad hoc")]
        [Trait("Media", "Functional")]
        ///dotnet test --filter "FullyQualifiedName=Instagram.Private.API.Tests.MediaTests.Should_Configure_Photo"

        public void Should_Configure_Photo()
        {
            var photo =
            Fixtures.Clients.Default
              .Media
              .UploadPhoto(@"C:\temp\img1.jpg").Result;

            photo.status.ShouldBe("ok");

            Fixtures.Clients.Default
               .Media
               .ConfigurePhoto(photo.upload_id, "Chatting with my friend #friends").Result
               .status
               .ShouldBe("ok");
        }       
    }
}