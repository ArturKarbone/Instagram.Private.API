using Instagram.Private.API.Client.Direct;
using Instagram.Private.API.Utils;
using PoC;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Private.API.Client
{
	public class Account
	{
		public Account(string Id)
		{
			this.Id = Id;
		}
		public string Id { get; set; }
	}

	public class Device
	{
		public string Id { get; set; }
	}

	public class InstagramClient
	{
		private string _password;
		private string _userName;
		private Device device;
		private HttpClientWrapper wrapper = new HttpClientWrapper();
		public FriendshipClient Friendship;
        public DirectClient Direct;

		public InstagramClient(string userName, string password, Device device)
		{
			_password = password;
			_userName = userName;
			this.device = device;

			EnsureAuthentication();

			Friendship = new FriendshipClient(wrapper, device);
            Direct = new DirectClient(wrapper, device);
        }


		#region relationships


		/*
		 * 
		 * function invite(counter = 1) {
        getFollowers('rejaniced')
            .then(function (followers) {
                console.log(chalk.bgBlue(`================== New iteration ${counter} staterd==================`));
                var followers_ids = _.map(followers, x => x.id);
                console.log(`followers ${followers_ids.length}`)
                //followers_ids = _.first(followers_ids, 3);

                invite_accounts(followers_ids)
                    .then((result) => {
                        handle_invitation(++counter);
                    });

            });
    }

    function invite_accounts(accountIds) {
        return new Promise(function (resolve, reject) {
            var counter = {
                relations: 0
            };

            console.log(`inviting ${accountIds.length} followers`);
            delayed_processor
                .process(accountIds, (accountId) => {
                    return invite_single_account(accountId, counter);
                }).then(() => {
                    console.log(`========== ${chalk.green(counter.relations)} relations created out of ${chalk.green(accountIds.length)} accounts =========`);
                    resolve();
                })
        });
    }

    function invite_single_account(accountId, counter) {
        return new Promise(function (resolve, reject) {
            get_relationship(accountId)
                .then((relationship) => {
                    console.log(`Relationship for account ${accountId}`);
                    console.log(relationship);
                    if (!relationship.outgoingRequest && !relationship.following) {
                        create_relationship(accountId)
                            .then((relationship_results) => {
                                counter.relations++;
                                console.log(`========== ${counter.relations} relations_created =========`);
                                console.log(relationship_results);
                                resolve({ delay_next_action: true });
                            });
                    } else {
                        resolve({ delay_next_action: false });
                    }
                });
        })
    }
		 */

		#endregion

		#region Private Helpers

		private void EnsureAuthentication()
		{
			var payload = new
			{
				username = _userName,
				password = _password,
				login_attempt_count = 0,
				_uuid = Guid.NewGuid().ToString(),
				device_id = device.Id,//extract
									  //_csrftoken = "aKU55F2zXkd7XSWT9r4qCngE243sMmSb", // missing
				_csrftoken = "missing" //???
			};

			var res = wrapper
				.SetBaseAddress("https://i.instagram.com/api/v1/")
				.SetResource("accounts/login/")				
				.PostSigned(payload).Result;

			var status = res.StatusCode;
			var cnt = res.Content.ReadAsStringAsync().Result;
		}

		#endregion
	}

	public class IntagramResponse
	{
		public string status { get; set; }
	}

	public class Friendship : IntagramResponse
	{
		public bool following { get; set; }
		public bool followed_by { get; set; }
		public bool blocking { get; set; }
		public bool is_private { get; set; }
		public bool incoming_request { get; set; }
		public bool outgoing_request { get; set; }
		public bool is_blocking_reel { get; set; }
		public bool is_muting_reel { get; set; }

	}

	//public class FollowersRequest
	//{
	//	public string AccountId { get; set; }
	//}

	public class FollowersResponse : IntagramResponse
	{
		public User[] users { get; set; }
		public bool big_list { get; set; }
		public string next_max_id { get; set; }
		public int page_size { get; set; }
	}

	public class User
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
}
