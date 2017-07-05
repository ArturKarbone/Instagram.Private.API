using Instagram.Private.API.Utils;
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
       // [Fact]
        [Trait("Media", "Functional")]
        ///dotnet test --filter "FullyQualifiedName=Instagram.Private.API.Tests.MediaTests.Should_Configure_Photo"

        public void Should_Configure_Photo()
        {
            var photo =
            Fixtures.Clients.Default
              .Media
              .UploadPhoto(new OnlinePhoto(new Uri("https://scontent-arn2-1.cdninstagram.com/t51.2885-15/e35/19051009_324506701301905_7499267126520184832_n.jpg"))).Result;

            photo.status.ShouldBe("ok");

            Fixtures.Clients.Default
               .Media
               .ConfigurePhoto(photo.upload_id, "Chatting with my friend #friends").Result
               .status
               .ShouldBe("ok");
        }


        [Fact(Skip = "used ad hoc")]
        [Trait("Media", "Functional")]
        ///dotnet test --filter "FullyQualifiedName=Instagram.Private.API.Tests.MediaTests.Should_Configure_Photo"

        public async Task Should_Obtain_Media_By_Url()
        {
            var media = await
            Fixtures.Clients.Default
              .Media
              .Get(new Uri("https://www.instagram.com/p/BWKSQ_vhSzJ/?taken-by=jennygreene_91"));

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