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
using Android.Graphics;
using Android.Content.Res;


namespace MagicMobile
{
	class ViewPagerImagesAdapter:PagerAdapter,View.IOnTouchListener
	{
		Context context;
		Resources res;
		Activity actividad;
		private int[] galImagenes;/*		 = {
			Resource.Drawable.i1SA,
			Resource.Drawable.i2SA,
			Resource.Drawable.i3SA

		};*/

		public ViewPagerImagesAdapter(Context context, int[] galImagenes,Resources res, Activity actividad){
			this.context = context;
			this.galImagenes = galImagenes;
			Console.WriteLine ("CANTIDAD DE IMAGENES  " + galImagenes.Length);
			this.res = res;
			this.actividad = actividad;
		}

		#region implemented abstract members of PagerAdapter


		public override bool IsViewFromObject (View view, Java.Lang.Object @object)
		{
			return view == ((ImageView)@object);
		}


		public override int Count {
			get {
				return galImagenes.Length;
			}
		}

		public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @Object){
			((ViewPager)container).RemoveView ((ImageView)@Object);
		}


		#endregion

		/*		public override int getCount(){

		}*/

		/*public override Boolean isViewFromObject(View view, Object o){

	}*/

	public override Java.Lang.Object InstantiateItem( ViewGroup container, int position){
		ImageView imageView = new ImageView (context);
		//int padding = context.Resources.GetDimensionPixelSize(
		//imageView.SetPadding (0, 0, 0, 0);
		imageView.SetPadding (0,0,0,0);


		/*using (var bm = DecodeSampledBitmapFromResource (res, galImagenes [position], 800, 1280)) {
			imageView.SetImageBitmap (bm);

		}*/

		//imageView.SetPadding (200, 100, 50, 10);
		imageView.SetScaleType (ImageView.ScaleType.CenterInside);
		imageView.SetImageResource (galImagenes [position]);
		imageView.SetOnTouchListener (this);
		((ViewPager) container).AddView(imageView,0);
		return imageView;
	}

	public static Bitmap DecodeSampledBitmapFromResource(Resources res, int resId, int reqWidth, int reqHeight)
	{

		var options = new BitmapFactory.Options {
			InJustDecodeBounds = true,
		};
		using (var dispose = BitmapFactory.DecodeResource(res, resId, options)) {
		}
		// Calculate inSampleSize
		options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

		// Decode bitmap with inSampleSize set
		options.InJustDecodeBounds = false;
		return BitmapFactory.DecodeResource(res, resId, options);
	}

	public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
	{
		// Raw height and width of image
		var height = (float)options.OutHeight;
		var width = (float)options.OutWidth;
		var inSampleSize = 1D;

		if (height > reqHeight || width > reqWidth)
		{
			inSampleSize = width > height
				? height/reqHeight
				: width/reqWidth;
		}

		return (int) inSampleSize;
	}

	public bool OnTouch(View v, MotionEvent e)
	{
		switch (e.Action & MotionEventActions.Mask )
		{

		case MotionEventActions.PointerDown:
			//Console.WriteLine ("ESTAN TOCANDO PINCH");
			//var previous = new Intent (actividad, typeof(MainActivity));
			//	actividad.StartActivity (previous);
			//	actividad.OnBackPressed ();
			actividad.Finish ();




			break;

		}

		return true;
	}



}
}

