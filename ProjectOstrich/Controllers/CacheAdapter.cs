using System;
using System.Collections.Generic;

using Android.App;
using Android.Widget;
using Android.Views;

namespace ProjectOstrich
{
	public class CacheAdapter : BaseAdapter
	{
		public bool ShouldFilter { get; set; }
		public List<string> FilterIdentifiers { get; private set; }

		Activity _activity;
		List<Message> _messages = new List<Message>();

		private Cache _cache;

		public CacheAdapter (Activity activity, Cache cache)
		{
			_activity = activity;
			FilterIdentifiers = new List<string> ();
			_cache = cache;
			_cache.OnMessageReceived += (sender, e) => {
				if (ShouldFilter)
					if (!FilterIdentifiers.Contains (e.Identifier))
						return;

				_messages.Add (e);
				this.NotifyDataSetChanged ();
			};
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

			var view = convertView ?? _activity.LayoutInflater.Inflate (
				Android.Resource.Layout.SimpleListItem1, parent, false);
			var textview = (TextView)view.FindViewById (Android.Resource.Id.Text1);
			textview.Text = message.Data;

			return view;
		}
	}
}

