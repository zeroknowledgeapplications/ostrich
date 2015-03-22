﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ProjectOstrich
{
	[Activity (Label = "HissActivity", Icon = "@drawable/ostrich")]			
	public class HissActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Hiss);

			// Open the main view if clicked to proceed
			Button button = FindViewById<Button> (Resource.Id.send_hiss);
			button.Click += delegate {
				string text = FindViewById<TextView>(Resource.Id.hiss_text).Text;
				//FIXME add text to cache
				StartActivity(typeof(MainActivity));
			};
		}
	}
}

