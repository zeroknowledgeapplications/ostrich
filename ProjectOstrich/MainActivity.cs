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
	public class MainActivity : Activity
	{
		int count = 1;
		private ExchangeManager _manager;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);
			_manager = new ExchangeManager (BaseContext, this);
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
