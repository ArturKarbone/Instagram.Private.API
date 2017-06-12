using Instagram.Private.API.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Instagram.Private.API.Client
{
    public class AccountClient
    {
        #region Messages

        public class SearchResult
        {
            public int num_results { get; set; }
            public User[] users { get; set; }
            public bool has_more { get; set; }
            public string rank_token { get; set; }
            public string status { get; set; }
        }

        public class User
        {
            public long pk { get; set; }
            public string username { get; set; }
            public string full_name { get; set; }
            public bool is_private { get; set; }
            public string profile_pic_url { get; set; }
            public string profile_pic_id { get; set; }
            public Friendship_Status friendship_status { get; set; }
            public bool is_verified { get; set; }
            public bool has_anonymous_profile_picture { get; set; }
            public int follower_count { get; set; }
            public string byline { get; set; }
            public float mutual_followers_count { get; set; }
            public string social_context { get; set; }
            public string search_social_context { get; set; }
        }

        public class Friendship_Status
        {
            public bool following { get; set; }
            public bool is_private { get; set; }
            public bool incoming_request { get; set; }
            public bool outgoing_request { get; set; }
        }


        #endregion


        private HttpClientWrapper wrapper;
        private Device device;

        public AccountClient(HttpClientWrapper wrapper, Device device)
        {
            this.wrapper = wrapper;
            this.device = device;
        }

        public async Task<SearchResult> Search(string userName)
        {
            var resource = $"users/search/?is_typehead=true&q={userName}&rank_token={BuildRankToken()}";
            return (await (await wrapper.SetResource(resource).GetAsync()).Content.ReadAsStringAsync())
                .Deserialize<SearchResult>();
        }

        public async Task<User> SearchUser(string userName)
        {
            var searchResult = await Search(userName);

            return searchResult.users.First(x => x.username == userName);
        }

        public string GetAccountId()
        {
            return wrapper.GetCookieValue("ds_user_id");
        }

        private string BuildRankToken()
        {
            return $"{GetAccountId()}_{Guid.NewGuid()}";
        }
    }
}
