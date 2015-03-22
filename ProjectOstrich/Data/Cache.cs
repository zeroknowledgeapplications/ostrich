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
			// TODO: add caches
		}

		public static Cache FromJson(string data) {
			return new Cache() {Messages = data.Split ("++##+*$%^".ToCharArray()).Select(s => Message.FromJson(s)).ToList()};
		}

	}
}

