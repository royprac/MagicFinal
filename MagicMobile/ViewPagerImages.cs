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
using Android.Support.V4.View;

namespace MagicMobile
{
	[Activity (Label = "ViewPagerImages")]			
	public class ViewPagerImages : Activity//,View.IOnTouchListener
	{

		ViewPager vistaImagenes;
		LinearLayout mainLayout;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			int[] imagenes = {
				Resource.Drawable.unha11, Resource.Drawable.unha11,
				Resource.Drawable.unha12, Resource.Drawable.unha13
			};
			Bundle saveConfig = Intent.Extras;
			Console.WriteLine ("INDICEEEEEEEEEEEEEEEEEEEEEEEEE = "+saveConfig.GetString("idSlideImage"));

			mainLayout = new LinearLayout (this);
			mainLayout.LayoutParameters = new LinearLayout.LayoutParams (-1, -1);
			mainLayout.Orientation = Android.Widget.Orientation.Vertical;


			vistaImagenes = new ViewPager (this);
			ViewPagerImagesAdapter adapter = new ViewPagerImagesAdapter (this,imagenes,Resources,this);
			vistaImagenes.Adapter = adapter;

			mainLayout.AddView (vistaImagenes);
			vistaImagenes.SetCurrentItem (Convert.ToInt32(saveConfig.GetString("idSlideImage")),true);

			//	vistaImagenes.SetOnTouchListener (this);
			SetContentView (mainLayout);
			// Create your application here
		}

		public bool OnTouch(View v, MotionEvent e)
		{

			//ImageView view = (ImageView)v;


			switch (e.Action & MotionEventActions.Mask )
			{

			case MotionEventActions.PointerDown:

				//view.Alpha = 0;

				/*	Console.WriteLine ("ESTAN TOCANDO PINCH");
				var previous = new Intent (this, typeof(MainActivity));
				StartActivity(previous);
				OnBackPressed ();
*/
				/*Intent MainActivityIntent = new Intent (ViewPagerImages, MainActivity); 
				StartActivity (MainActivityIntent);

				*/			
				break;

				/*case MotionEventActions.Up:

				Console.WriteLine("SUELTO UN DEDO ??????");
				break;
			case MotionEventActions.PointerUp:
				Console.WriteLine ("SUELTO EL DEDOS");

				break;
			case MotionEventActions.Move:

				break;			*/
			}

			return true;
		}

	}
}

