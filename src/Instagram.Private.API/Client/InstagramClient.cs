﻿using Instagram.Private.API.Client.Direct;
using Instagram.Private.API.Utils;

namespace Instagram.Private.API.Client
{
    public class Account
    {
        public Account(string id)
        {
            this.Id = id;
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
        private HttpClientWrapper wrapper = new HttpClientWrapper()
                                                .SetBaseAddress("https://i.instagram.com/api/v1/");
        public FriendshipClient Friendship { get; }
        public DirectClient Direct { get; }
        public AccountClient Account { get; }
        public MediaClient Media { get; }

        public InstagramClient(string userName, string password, Device device)
        {
            _password = password;
            _userName = userName;
            this.device = device;

            EnsureAuthentication();

            Friendship = new FriendshipClient(wrapper, device);
            Direct = new DirectClient(wrapper, device);
            Account = new AccountClient(wrapper, device);
            Media = new MediaClient(wrapper, device);
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
            var session = Session
                .Init(this._userName, this._password, this.device)
                .Ensure();

            wrapper.SetCookies(session.Cookies);
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
