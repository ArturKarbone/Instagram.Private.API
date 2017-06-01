using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Instagram.Private.API.Utils;
using Newtonsoft.Json;

namespace Instagram.Private.API.Client
{
    public interface ISessionStorage
    {
        IEnumerable<Cookie> Get();
        void Persist(IEnumerable<Cookie> cookies);
        bool Exists { get; }

    }
    public class FileSessionStorage : ISessionStorage
    {
        public bool Exists => File.Exists("session.json");
        public IEnumerable<Cookie> Get()
        {
            return JsonConvert.DeserializeObject<List<System.Net.Cookie>>(File.ReadAllText(@"session.json"));
        }
        public void Persist(IEnumerable<Cookie> cookies)
        {
            File.WriteAllText(@"session.json", JsonConvert.SerializeObject(cookies));
        }
    }

    public class Session
    {
        public ISessionStorage Storage { get; set; } = new FileSessionStorage();
        public IEnumerable<Cookie> Cookies { get; private set; }
        public HttpClientWrapper Http = new HttpClientWrapper();

        public bool Authenticated { get; set; }
        public bool NeedToRefresh => !Storage.Exists || !Authenticated;

        string userName; string password; Device device;
        protected Session(string userName, string password, Device device)
        {
            this.userName = userName;
            this.password = password;
            this.device = device;
        }

        public Session Ensure()
        {
            //Read cookies
            if (Storage.Exists)
            {
                var cookies = Storage.Get();

                new HttpClientWrapper()
                    .SetBaseAddress("https://i.instagram.com/api/v1/")
                    .SetCookies(cookies);

                //make simple api call   

                //check status
                this.Authenticated = true;
                this.Cookies = cookies;
            }

            if (NeedToRefresh)
            {
                Login(userName, password, device);
                this.Cookies = Http.Cookies;
                Storage.Persist(this.Cookies);
            }

            return this;
        }

        public static Session Init(string userName, string password, Device device)
        {
            return new Session(userName, password, device);
        }

        private void Login(string userName, string password, Device device)
        {
            var payload = new
            {
                username = userName,
                password = password,
                login_attempt_count = 0,
                _uuid = Guid.NewGuid().ToString(),
                device_id = device.Id,//extract
                                      //_csrftoken = "aKU55F2zXkd7XSWT9r4qCngE243sMmSb", // missing
                _csrftoken = "missing" //???
            };

            var res = Http
                .SetBaseAddress("https://i.instagram.com/api/v1/")
                .SetResource("accounts/login/")
                .PostSigned(payload).Result;
        }
    }

}