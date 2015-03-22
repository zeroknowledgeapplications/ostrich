using System;

namespace ProjectOstrich
{
	public class Message
	{

		private const string _sep = "@^*^&*%@";

		public string Identifier { get; set; }

		public string Data { get; set; }

		public int HopCount { get; set; }

		public DateTime CreatedAt { get; set; }

		public String ToJson() {
			return Base64Encode (Data) + _sep +
				HopCount + _sep +
				Identifier + _sep +
				CreatedAt.Ticks;
		}

		public Message()
		{
			CreatedAt = DateTime.UtcNow;
			HopCount = 0;
			Identifier = new Random ().NextDouble ().ToString ();
		}

		public static Message FromJson(string data) {
			string[] chucks = data.Split (_sep.ToCharArray());
			return new Message {
				Data = Base64Decode (chucks [0]),
				HopCount = int.Parse (chucks [1]),
				Identifier = chucks [2],
				CreatedAt = new DateTime (long.Parse (chucks [3]), DateTimeKind.Utc)
			};
		}

		public static string Base64Encode(string plainText) {
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData) {
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

	}
}

