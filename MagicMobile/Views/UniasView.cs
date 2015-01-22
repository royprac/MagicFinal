using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using Android.Content.PM;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.Widget;
using Android.Content.PM;

namespace MagicMobile.Views
{
    [Activity(Label = "Catalogo", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class UniasView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            string id_categoria = Intent.GetStringExtra("id_categoria") ?? "Unia no disponible";
           // int cal_aux = id_categoria + 1;
            // Console.WriteLine(id_categoria);
            SetContentView(Resource.Layout.UniasView);
            //var TextCategoria = FindViewById<TextView>(Resource.Id.textCat);
            //TextCategoria.Text = id_categoria + 1;
                        
            //acción del botón de la uña
     /*
            var But = FindViewById<Button>(Resource.Id.but);
            But.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DisenhoVistaActivity));
                Console.WriteLine(But.Text);
                Console.WriteLine("544");
                intent.PutExtra("id_unia", Convert.ToString(Resource.Drawable.unia_1_01));
                StartActivity(intent);
            };*/
        }
    }
}