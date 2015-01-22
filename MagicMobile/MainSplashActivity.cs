using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MagicMobile
{
	public class MainSplashActivity : BaseAdapter
	{
		Context context;

		public MainSplashActivity (Context c)
		{
			context = c;
		}

		public override int Count { get { return thumbIds.Length; } }

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		// create a new ImageView for each item referenced by the Adapter
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ImageView i = new ImageView (context);

			i.SetImageResource (thumbIds[position]);
			i.LayoutParameters = new Gallery.LayoutParams (720, 600);
			i.SetScaleType (ImageView.ScaleType.FitXy);

			return i;
		}

		// references to our images
		int[] thumbIds = {
			Resource.Drawable.magic3,
			Resource.Drawable.tutorial1,
			Resource.Drawable.tutorial2,
			Resource.Drawable.tutorial3

		};
	}

}
