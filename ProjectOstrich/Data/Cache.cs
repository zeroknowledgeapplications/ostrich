using System.Collections.Generic;
using System.Linq;

namespace ProjectOstrich
{
	public class Cache
	{

		List<Message> Messages {get; set; }

		public List<Message> GetMessage() {
			return new List<Message> ();
		}

		public string ToJson() {
			return string.Join ("++##+*$%^", Messages.Select (m => m.ToJson ()));
		}

		public void Add(Cache cache) {
			Dictionary<string, Message> messages = new Dictionary<string, Message> ();
			foreach (Message message in Messages) {
				messages.Add (message.Identifier + message.Data, message);
			}
			foreach(Message message in cache.Messages) {
				if (messages.ContainsKey (message.Identifier + message.Data)) {
					if (message.HopCount + 1 < messages [message.Identifier + message.Data].HopCount) {
						messages [message.Identifier + message.Data].HopCount = message.HopCount + 1;
					}
				} else {
					message.HopCount++;
					messages.Add(message.Identifier + message.Data, message);
				}
			}
			Messages = messages.Values.ToList();
		}

		public static Cache FromJson(string data) {
			return new Cache() {Messages = data.Split ("++##+*$%^".ToCharArray()).Select(s => Message.FromJson(s)).ToList()};
		}

	}
}

