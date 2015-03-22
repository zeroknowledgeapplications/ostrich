using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ProjectOstrich
{
	[Activity (Label = "ProjectOstrich", MainLauncher = true, Icon = "@drawable/ostrich")]
	public class MainActivity : ListActivity
	{
		string[] items;
		private ExchangeManager _manager;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			items = new string[] { "Vegetables","Fruits","Flower Buds","Legumes","Bulbs","Tubers" };
			Cache c = new Cache ();
			ListAdapter = new CacheAdapter (this, c);
			SetContentView (Resource.Layout.Main);
			_manager = new ExchangeManager (BaseContext, this, c);
			c.Add (new Message (){ Data = "Hello World!" });
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
			var t = items[position];
			Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater inflater = new MenuInflater (this);
			inflater.Inflate (Resource.Menu.actionbar, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.hiss:
				StartActivity (typeof(HissActivity));
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}
	}
}
