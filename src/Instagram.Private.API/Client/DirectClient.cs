using Instagram.Private.API.Utils;
using System;
using System.Threading.Tasks;

namespace Instagram.Private.API.Client.Direct
{

    #region Messages

    public class SendMessageResponse
    {
        public Thread[] threads { get; set; }
        public string status { get; set; }
    }

    public class Thread
    {
        public string thread_id { get; set; }
        public User[] users { get; set; }
        public object[] left_users { get; set; }
        public Item[] items { get; set; }
        public long last_activity_at { get; set; }
        public bool muted { get; set; }
        public bool named { get; set; }
        public bool canonical { get; set; }
        public bool pending { get; set; }
        public string thread_type { get; set; }
        public long viewer_id { get; set; }
        public string thread_title { get; set; }
        public Inviter inviter { get; set; }
        public bool has_older { get; set; }
        public bool has_newer { get; set; }
        public Last_Seen_At last_seen_at { get; set; }
        public string newest_cursor { get; set; }
        public string oldest_cursor { get; set; }
    }

    public class Inviter
    {
        public long pk { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public bool is_private { get; set; }
        public string profile_pic_url { get; set; }
        public string profile_pic_id { get; set; }
        public bool is_verified { get; set; }
        public bool has_anonymous_profile_picture { get; set; }
    }

    public class Last_Seen_At
    {
        public _3738653469 _3738653469 { get; set; }
        public _4480548799 _4480548799 { get; set; }
    }

    public class _3738653469
    {
        public string timestamp { get; set; }
        public string item_id { get; set; }
    }

    public class _4480548799
    {
        public string timestamp { get; set; }
        public string item_id { get; set; }
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
    }

    public class Friendship_Status
    {
        public bool following { get; set; }
        public bool blocking { get; set; }
        public bool is_private { get; set; }
        public bool incoming_request { get; set; }
        public bool outgoing_request { get; set; }
    }

    public class Item
    {
        public string item_id { get; set; }
        public long user_id { get; set; }
        public long timestamp { get; set; }
        public string item_type { get; set; }
        public string text { get; set; }
        public string client_context { get; set; }
    }


    #endregion

    public class DirectClient
    {
        private HttpClientWrapper wrapper;
        private Device device;

        public DirectClient(HttpClientWrapper wrapper, Device device)
        {
            this.wrapper = wrapper;
            this.device = device;
        }

        public async Task<SendMessageResponse> SendMessage(Account to, string message)
        {            
            var payload = new
            {
                _uuid = Guid.NewGuid().ToString(),
                device_id = this.device.Id,
                _csrftoken = wrapper.GetCookieValue("csrftoken"),
                client_context = Guid.NewGuid().ToString(),
                //recipient_users = new string[] { to.Id },
                recipient_users = $"[[ {to.Id} ]]",
                text = message
            };


            var response = wrapper
                .SetResource($"direct_v2/threads/broadcast/text/ ")
                .PostAsMultipart(payload).Result
                .Content.ReadAsStringAsync().Result
                .Deserialize<SendMessageResponse>();

            return response;
        }

        public async Task<SendMessageResponse> SendLike(Account to)
        {            
            var payload = new
            {
                _uuid = Guid.NewGuid().ToString(),
                device_id = this.device.Id,
                _csrftoken = wrapper.GetCookieValue("csrftoken"),
                client_context = Guid.NewGuid().ToString(),               
                recipient_users = $"[[ {to.Id} ]]"
            };


            var response = wrapper
                .SetResource($"direct_v2/threads/broadcast/like/ ")
                .PostAsMultipart(payload).Result
                .Content.ReadAsStringAsync().Result
                .Deserialize<SendMessageResponse>();

            return response;
        }

        public async Task<SendMessageResponse> SendPhoto(Account to, string path)
        {
            var payload = new
            {
                _uuid = Guid.NewGuid().ToString(),
                device_id = this.device.Id,
                _csrftoken = wrapper.GetCookieValue("csrftoken"),
                client_context = Guid.NewGuid().ToString(),
                type = "media",
                recipient_users = $"[[ {to.Id} ]]"
            };

            byte[] image = System.IO.File.ReadAllBytes(path);
          
            var response = wrapper
                .SetResource($"direct_v2/threads/broadcast/upload_photo/ ")
                .PostAsMultipartWithImage(payload, image).Result
                .Content.ReadAsStringAsync().Result
                .Deserialize<SendMessageResponse>();

            return response;
        }

    }
}
