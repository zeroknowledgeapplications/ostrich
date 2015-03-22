
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
	[Activity (Label = "HissActivity", Icon = "@drawable/ostrich", WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateAlwaysVisible)]			
	public class HissActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Hiss);


			Button button = FindViewById<Button> (Resource.Id.send_hiss);
			EditText editText = FindViewById<EditText> (Resource.Id.hiss_text);

			// Open the main view if clicked to proceed
			button.Click += delegate {
				// Store message
				CacheController.Cache.Add (new Message (){ Data = FindViewById<TextView>(Resource.Id.hiss_text).Text });
				CacheController.Save();

				// Go back to main menu
				StartActivity(typeof(MainActivity));
			};

			editText.TextChanged += delegate {
				button.Enabled = editText.Text.Length > 2 && editText.Text.Length < 140;
			};

		}
	}
}

