using System;
using System.IO;

namespace ProjectOstrich
{
	public class Message
	{
		public Message ()
		{
		}

		/// <summary>
		/// Phone number hash.
		/// </summary>
		/// <value>The identifier.</value>
		public string Identifier { get; set; }

		public string Data { get; set; }

		public int HopCount { get; set; }

		public DateTime CreatedAt { get; set; }

		public String ToJson { 
			get {
				return "";
			}
		}

	}
}

