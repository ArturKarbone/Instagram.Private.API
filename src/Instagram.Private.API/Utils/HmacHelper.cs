using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PoC
{
	public class HMACSHA1Helper
	{
		//public byte[] ComputeHash(string hashKey, string message)
		//{
		//    ASCIIEncoding encoding = new ASCIIEncoding();

		//    return new HMACSHA256(encoding.GetBytes(hashKey))
		//        .ComputeHash(encoding.GetBytes(message));
		//}

		public string ComputeHash(string hashKey, string message)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();

			var hash = new HMACSHA256(encoding.GetBytes(hashKey))
				.ComputeHash(encoding.GetBytes(message));

			return ByteArrayToString(hash);
		}

		private static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
				hex.AppendFormat("{0:x2}", b);
			return hex.ToString();
		}

		private static string ByteArrayToString2(byte[] ba)
		{
			string hex = BitConverter.ToString(ba);
			return hex.Replace("-", "");
		}
	}
}
