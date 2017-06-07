using Instagram.Private.API.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PoC;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Instagram.Private.API.Utils
{
    public class HttpClientWrapper
    {
        //public HttpClient _client { get; set; } = new HttpClient(new Handler());

        CookieContainer cookieContainer = new CookieContainer();


        public IEnumerable<Cookie> Cookies => cookieContainer.GetCookies(_client.BaseAddress).Cast<Cookie>();
        public void SetCookies(IEnumerable<Cookie> cookies)
        {
            cookies
                .ToList()
                .ForEach(c =>
                {
                    //cookieContainer.Add(_client.BaseAddress, new Cookie("CookieName", "cookie_value"));
                    cookieContainer.Add(_client.BaseAddress, c);
                });
        }


        public HttpClient _client { get; set; }
        public string _resource { get; set; } = string.Empty;
        public object _param { get; set; }
        public HttpClientWrapper()
        {
            HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            _client = new HttpClient(handler);

            //Default headers
            this.SetHeader("X-IG-Connection-Type", "WIFI")
                .SetHeader("X-IG-Capabilities", "3QI=")
                .SetHeader("User-Agent", $"Instagram {Signature.Latest.ApplicationVesion} Android (20/4.4.4; 515dpi; 2560x1440; Huawei; HUAWEI SCL-L03; hwSCL-Q; en_US)");
        }

        public HttpClientWrapper SetBaseAddress(string baseAddress)
        {

            _client.BaseAddress = new Uri(baseAddress);
            return this;
        }

        public HttpClientWrapper SetResource(string resource)
        {
            _resource = resource;
            return this;
        }
        public HttpClientWrapper SetAuthHeader(string token)
        {
            _client.DefaultRequestHeaders.Add("x-api-key", token);
            return this;
        }

        public HttpClientWrapper SetHeader(string header, string value)
        {
            _client.DefaultRequestHeaders.Add(header, value);
            return this;
        }

        public HttpClientWrapper AddParameter<T>(T parameter) where T : class
        {
            _param = parameter;
            return this;
        }

        public async Task<HttpResponseMessage> PostAsync(FormUrlEncodedContent content)
        {
            return await _client.PostAsync(_client.BaseAddress + _resource, content);
        }

        public async Task<HttpResponseMessage> PostAsMultipart(dynamic payload)
        {
            var content = new MultipartFormDataContent();

            foreach (var prop in payload.GetType().GetProperties())
            {
                var test = prop.GetValue(payload, null);
                content.Add(new StringContent(prop.GetValue(payload, null).ToString()), prop.Name);
            }

            return await PostAsync(content);
        }


        public async Task<HttpResponseMessage> PostAsMultipartWithImage(dynamic payload, byte[] image)
        {
            var content = new MultipartFormDataContent();

            foreach (var prop in payload.GetType().GetProperties())
            {
                var test = prop.GetValue(payload, null);
                content.Add(new StringContent(prop.GetValue(payload, null).ToString()), prop.Name);
            }

            content.Add(new StreamContent(new MemoryStream(image)), "photo", $"{Guid.NewGuid()}.jpg");

            return await PostAsync(content);
        }

        public async Task<HttpResponseMessage> PostAsync(MultipartFormDataContent content)
        {
            return await _client.PostAsync(_client.BaseAddress + _resource, content);
        }

        public async Task<HttpResponseMessage> PostSigned<T>(T payload)
        {
            HMACSHA1Helper hmac = new HMACSHA1Helper();

            var hash = hmac.ComputeHash(Signature.Latest.PrivateKey, payload.Serialize());

            var signedBody = hash + "." + payload.Serialize();

            var sb = new StringContent(signedBody);
            var kv = new StringContent(Signature.Latest.Version);

            var content = new MultipartFormDataContent();
            content.Add(sb, "signed_body");
            content.Add(kv, "ig_sig_key_version");

            return await PostAsync(content);
        }


        //todo easier way to send json?
        public async Task<HttpResponseMessage> PostAsJsonAsync()
        {
            var jsonString = _param.Serialize();
            return await _client.PostAsync(_client.BaseAddress + _resource, new StringContent(jsonString, Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> GetAsync()
        {
            //var jsonString = _param.Serialize();
            return await _client.GetAsync(_client.BaseAddress + _resource);// (_client.BaseAddress + _resource, new StringContent(jsonString, Encoding.UTF8, "application/json"));
        }


        public string GetCookieValue(string name)
        {
            Uri uri = _client.BaseAddress;
            return cookieContainer.GetCookies(uri).Cast<Cookie>().First(x => x.Name == name).Value;
        }
    }


    public static class SerializationExtensions
    {
        public static T Deserialize<T>(this string jsonValue)
        {
            return JsonConvert.DeserializeObject<T>(jsonValue);
        }

        public static string Serialize(this object instance)
        {
            return JsonConvert.SerializeObject(instance, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}
