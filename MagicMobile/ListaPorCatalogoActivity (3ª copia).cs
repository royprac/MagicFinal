using System;
//using System.Collections.Generic;


using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace MagicMobile
{
	[Activity (Label = "MagicMobile")]
	public class ListaPorCatalogoActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);
		
			SetContentView (Resource.Layout.ListaPorCatalogo);

	


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


