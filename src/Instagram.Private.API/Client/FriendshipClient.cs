using Instagram.Private.API.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Instagram.Private.API.Client
{
	public class FriendshipClient
	{
		private HttpClientWrapper wrapper;
		private Device device;

		public FriendshipClient(HttpClientWrapper wrapper, Device device)
		{
			this.wrapper = wrapper;
			this.device = device;
		}

		public async Task<FollowersResponse> Followers(Account account, string next_max_id = "")
		{
			if (!string.IsNullOrWhiteSpace(next_max_id))
			{
				return (await (await wrapper.SetResource($"friendships/{account.Id}/followers/?max_id={next_max_id}").GetAsync()).Content.ReadAsStringAsync()).Deserialize<FollowersResponse>();
			}
			else
			{
				return (await (await wrapper.SetResource($"friendships/{account.Id}/followers/").GetAsync()).Content.ReadAsStringAsync()).Deserialize<FollowersResponse>();
			}
		}

		public IEnumerable<User[]> FollowersCursor(Account account)
		{
			var res = Followers(account).Result;
			yield return res.users;

			while (!string.IsNullOrWhiteSpace(res.next_max_id))
			{
				res = Followers(account, res.next_max_id).Result;
				yield return res.users;
			}
		}


		private async Task<FollowersResponse> GetFollowers(Account account)
		{
			return (await (await wrapper.SetResource($"friendships/{account.Id}/followers/").GetAsync()).Content.ReadAsStringAsync()).Deserialize<FollowersResponse>();
		}

		public async Task<Friendship> GetFriendshipWith(Account account)
		{
			//EnsureAuthentication();

			var friendship = (await (await wrapper.SetResource($"friendships/show/{account.Id}/").GetAsync()).Content.ReadAsStringAsync()).Deserialize<Friendship>();
			return friendship;

			//https://i.instagram.com/api/v1/friendships/show/3738653469/
		}
		public async Task<Friendship> Invite(Account account)
		{
			//EnsureAuthentication();

			var payload = new
			{
				_uuid = Guid.NewGuid().ToString(),
				device_id = this.device.Id,
				_csrftoken = wrapper.GetCookieValue("csrftoken"),
				user_id = account.Id
			};

			var friendship = wrapper
				.SetResource($"friendships/create/{account.Id}/")
				.PostSigned(payload).Result
				.Content.ReadAsStringAsync().Result
				.Deserialize<Friendship>();

			//var friendship = (await (await wrapper.SetResource($"friendships/create/{account.Id}/").PostSigned(payload)).Content.ReadAsStringAsync()).Deserialize<Friendship>();
			return friendship;
		}


		public async Task<Friendship> Uninvite(Account account)
		{
			//EnsureAuthentication();
			var payload = new
			{
				_uuid = Guid.NewGuid().ToString(),
				device_id = this.device.Id,
				_csrftoken = wrapper.GetCookieValue("csrftoken"),
				user_id = account.Id
			};

			var friendship = wrapper
				.SetResource($"friendships/destroy/{account.Id}/")
				.PostSigned(payload).Result
				.Content.ReadAsStringAsync().Result
				.Deserialize<Friendship>();

			//var friendship = (await (await wrapper.SetResource($"friendships/destroy/{account.Id}/").PostAsync(content)).Content.ReadAsStringAsync()).Deserialize<Friendship>();
			return friendship;

			//https://i.instagram.com/api/v1/friendships/show/3738653469/
		}

	}
}
