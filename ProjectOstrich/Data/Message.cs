using System;

namespace ProjectOstrich
{
	public class Message
	{

		public string Identifier { get; set; }

		public string Data { get; set; }

		public int HopCount { get; set; }

		public DateTime CreatedAt { get; set; }

		public String ToJson() {
			return "{}"; //JsonConvert.SerializeObject(this);
		}

		public static Message FromJson(string json) {
			return new Message (); //JsonConvert.DeserializeObject<Message>(json);
		}

	}
}

