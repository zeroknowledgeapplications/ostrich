using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjectOstrich
{
	public class Cache
	{
		public event EventHandler<Message> OnMessageReceived = delegate { };

		public List<Message> Messages {get; set; }

		public Cache()
		{
			Messages = new List<Message> ();
		}

		public string ToJson() {
			return string.Join ("++##+*$%^", Messages.Select (m => m.ToJson ()));
		}

		public void Add(Message m)
		{
			Messages.Add (m);
			OnMessageReceived (this, m);
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
					this.Messages.Add (message);
					OnMessageReceived (this, message);
				}
			}
		}

		public static Cache FromJson(string data) {
			List<Message> messages = data.Split (new string[]{"++##+*$%^"}, StringSplitOptions.RemoveEmptyEntries).Where(d => !String.IsNullOrWhiteSpace(d)).Select (s => Message.FromJson (s)).ToList ();
			messages = messages.Where (
				m => ((m.CreatedAt + TimeSpan.FromDays(14)) > DateTime.UtcNow && m.HopCount < 10)
			).ToList();
			return new Cache { Messages = messages };
		}

	}
}

