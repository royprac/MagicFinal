using System;
//using System.Collections.Generic;


using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;



namespace MagicMobile
{
    [Activity(Label = "MagicMobile", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
	public class DisenhosPorCatalogoActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);
		
			SetContentView (Resource.Layout.DisenhosPorCatalogo);

	
			var callListButton2 = FindViewById<ImageButton> (Resource.Id.myButtonDisenho1);

			callListButton2.Click += (sender, e) => {
				//var intent4 = new Intent (this, typeof(DisehoView));
				//StartActivity (intent4);
			};

		}




	/*	public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle saveInstanceState)
		{
			View vista = inflater.Inflate(Resources.GetLayout.Catalogo ,container, false);
			Boton = vista.FindViewById<Button>(Resource.Id.myButton);
			Boton.SetText ("FASF", TextView.BufferType.Normal);
			return vista;
		}
		public Button Boton { get; set; }
		} */

}
}


