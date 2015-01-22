
//using System.Collections.Generic;


using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MagicMobile.Activities;

namespace MagicMobile
{
	[Activity (Label = "MagicMobile", Icon = "@drawable/icon")]
	public class DisenhosPorCatalogoActivityGrid : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView (Resource.Layout.DisenhosPorCatalogoGrid);
			var gridview = FindViewById<GridView>(Resource.Id.gridview);
			gridview.Adapter = new ImageAdapter(this);
			gridview.ItemClick += (sender, e) => {
				var intent4 = new Intent (this, typeof(CapturaManoActivity));
				StartActivity (intent4);
			};

			//gridview.ItemClick += (sender, args) => Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();


		}
	
	}






}



