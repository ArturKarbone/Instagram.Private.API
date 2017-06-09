using System;
using System.Net.Http;

namespace Instagram.Private.API.Utils
{
    public abstract class Photo
    {
        public abstract byte[] Content();
    }

    public class OnlinePhoto : Photo
    {
        public Uri Uri { get; }
        public OnlinePhoto(Uri uri)
        {
            this.Uri = uri;
        }

        public override byte[] Content()
        {
            return new HttpClient()
                    .GetByteArrayAsync(Uri.ToString())
                    .Result;
        }
    }
}