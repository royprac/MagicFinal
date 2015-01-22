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

namespace MagicMobile.Activities
{
    [Activity(Label = "ModoAvatar")]
    public class ModoAvatar : Activity
    {
        ViewPager vistaImagenes;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ModoAvatar);
            int[] imagenes = {
                            Resource.Drawable.mano_blanca,
                            Resource.Drawable.mano_morena,
                            Resource.Drawable.mano_roja,
                            Resource.Drawable.mano_colorada,
                        };

            LinearLayout ly = (LinearLayout)FindViewById(Resource.Id.layoutx);
            vistaImagenes = new ViewPager(this);
            ViewPagerImagesAdapter adapter = new ViewPagerImagesAdapter(this, imagenes, Resources, this);
            vistaImagenes.Adapter = adapter;
            ly.AddView(vistaImagenes);
            // Create your application here
            var boton_retornar = FindViewById<Button>(Resource.Id.butregresar);
            boton_retornar.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CatalogoActivity));
                StartActivity(intent);
            };    
        }
    }
}