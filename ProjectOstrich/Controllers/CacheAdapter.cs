using System;
using System.Collections.Generic;

using Android.App;
using Android.Widget;

namespace ProjectOstrich
{
	public class CacheAdapter : BaseAdapter
	{
		public bool ShouldFilter { get; set; }
		public List<string> FilterIdentifiers { get; private set; }

		List<Message> _messages = new List<Message>();

		private Cache _cache;

		public CacheAdapter (Cache cache)
		{
			FilterIdentifiers = new List<string> ();
			_cache = cache;
		}

		public override int Count {
			get {
				return _messages.Count;
			}
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return _messages [position].GetHashCode ();
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var message = _messages [position];
			return null;
		}
	}
}

