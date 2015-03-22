using System.Collections.Generic;

namespace ProjectOstrich
{
	public class Cache
	{

		List<Message> Messages {get; set; }

		public List<Message> GetMessage() {
			return new List<Message> ();
		}

		public string ToJson() {
			return "{}"; //JsonConvert.SerializeObject(Messages);
		}

		public void Add(Cache cache) {
			// TODO: add caches
		}

		public static Cache FromJson(string json) {
			return new Cache(); //JsonConvert.DeserializeObject<Cache> (json);
		}

	}
}

