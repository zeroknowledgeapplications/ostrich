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
		private ExchangeManager _manager;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			CacheController.Load ();

			ListAdapter = new CacheAdapter (this, CacheController.Cache);
			_manager = new ExchangeManager (BaseContext, this, CacheController.Cache);
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
				CacheController.Save ();
				StartActivity (typeof(HissActivity));
				return true;

			default:
				return base.OnOptionsItemSelected(item);
			}
		}
	}
}
