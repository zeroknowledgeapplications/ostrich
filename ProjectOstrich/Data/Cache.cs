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
			//Messages.Clear ();
			/*foreach (var m in cache.Messages) {
				Add (m);
				Console.WriteLine (m);
			}*/
			foreach (var m in cache.Messages) {
				if (!Messages.Contains (m)) {
					Add (m);
				}
				else {
					var other = Messages.First ((i) => i.Equals(m));
					other.HopCount = Math.Min (other.HopCount, m.HopCount + 1);
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

