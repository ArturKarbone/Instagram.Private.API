using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Instagram.Private.API.Utils;
using Newtonsoft.Json;
using System.Linq;

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
        protected string Id { get; }
        protected string Path => $@"session_{Id}.json";

        public FileSessionStorage(string id)
        {
            this.Id = id;
        }

        public bool Exists => File.Exists(Path);

        public IEnumerable<Cookie> Get()
        {
            return JsonConvert.DeserializeObject<List<System.Net.Cookie>>(File.ReadAllText(Path));
        }
        public void Persist(IEnumerable<Cookie> cookies)
        {
            File.WriteAllText(Path, JsonConvert.SerializeObject(cookies));
        }
    }

    public class Session
    {
        public ISessionStorage Storage { get; set; }
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
            this.Storage = new FileSessionStorage(userName);
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
            else
            {
                Login(userName, password, device);
                this.Cookies = Http.Cookies;
                Storage.Persist(this.Cookies);
            }

            //if (NeedToRefresh)
            //{
            //    Login(userName, password, device);
            //    this.Cookies = Http.Cookies;
            //    Storage.Persist(this.Cookies);
            //}

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

            if (res.StatusCode != HttpStatusCode.OK)
            {
                throw new LoginFailed(userName);
            }

        }
    }


    class LoginFailed : Exception
    {
        public LoginFailed(string message) : base(message)
        {

        }
    }
}