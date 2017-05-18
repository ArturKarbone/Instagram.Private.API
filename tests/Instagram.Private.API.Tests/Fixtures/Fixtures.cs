using Instagram.Private.API.Client;
using System;

namespace Instagram.Private.API.Tests
{
    public class Fixtures
    {
        public class Accounts
        {
            public static Account To => new Account(Environment.GetEnvironmentVariable("INSTA_ACCOUNT_TO"));          
        }

        public class Clients
        {
            protected static string UserName => Environment.GetEnvironmentVariable("INSTA_USER_NAME");
            protected static string Password => Environment.GetEnvironmentVariable("INSTA_PASSWORD");
            protected static string DeviceId => Environment.GetEnvironmentVariable("INSTA_DEVICE_ID");
            public static InstagramClient Default => new InstagramClient(UserName, Password, new Device { Id = DeviceId });           
        }
    }
}