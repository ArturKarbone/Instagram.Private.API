using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Instagram.Private.API.Tests
{
    public class MediaTests
    {
        //[Fact(Skip = "used ad hoc")]
        //[Fact]
        //[Trait("Media", "Functional")]
        /////dotnet test --filter "FullyQualifiedName=Instagram.Private.API.Tests.MediaTests.Should_Configure_Photo"

        //public void Should_Configure_Photo()
        //{
        //    var photo =
        //    Fixtures.Clients.Default
        //      .Media
        //      .UploadPhoto(@"C:\temp\img1.jpg").Result;

        //    photo.status.ShouldBe("ok");

        //    Fixtures.Clients.Default
        //       .Media
        //       .ConfigurePhoto(photo.upload_id, "Chatting with my friend #friends").Result
        //       .status
        //       .ShouldBe("ok");
        //}


        [Fact(Skip = "used ad hoc")]       
        [Trait("Media", "Functional")]
        ///dotnet test --filter "FullyQualifiedName=Instagram.Private.API.Tests.MediaTests.Should_Configure_Photo"

        public async Task Should_Obtain_Media_By_Url()
        {
            var media = await
            Fixtures.Clients.Default
              .Media
              .Get(new Uri("https://www.instagram.com/p/BVHSfdWBmI9/?taken-by=jennygreene_91"));

            media.status.ShouldBe("ok");
        }

        [Fact]
        [Trait("Media", "Functional")]
        ///dotnet test --filter "FullyQualifiedName=Instagram.Private.API.Tests.MediaTests.Should_Configure_Photo"

        public async Task Should_Delete_Media_By_Url()
        {
            var media = await
            Fixtures.Clients.Default
              .Media
              .Delete(new Uri("https://www.instagram.com/p/BVHTZ6zhybP/?taken-by=jennygreene_91"));

            media.status.ShouldBe("ok");
        }      

    }
}