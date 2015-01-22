using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Widget;
using MagicMobile.Algoritmo;
using Java.IO;
using Cirrious.MvvmCross.Droid.Views;
using System.Net;
namespace MagicMobile.Views
{

    using Environment = Android.OS.Environment;
    using Uri = Android.Net.Uri;
    using MagicMobile.Activities;
    public static class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }
    [Activity(Label = "Catalogo", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class DisehoView : MvxActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            string id_uniainter;
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisenhoView);

            System.Console.WriteLine(Core.Entities.Resource.uniaurl);
            System.Console.WriteLine("ok_________________-");

            //WebClient 

            ////////////Estrellas de Rating//////////////
            RatingBar ratingbar = FindViewById<RatingBar>(Resource.Id.ratingbar);
            ratingbar.NumStars = 5;
            //
            ratingbar.RatingBarChange += (o, e) =>
            {
                Toast.MakeText(this, "New Rating: " + ratingbar.Rating.ToString(), ToastLength.Short);
            };

            var callHistoryButton11 = FindViewById<Button>(Resource.Id.button1);

            ///////Recibe el codigo de la uña///////
            id_uniainter = Intent.GetStringExtra("id_unia") ?? "uña no disponible";

            /////// Envio de Codigo de uña a otro activiti ////////
            callHistoryButton11.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CapturaManoActivity));
                intent.PutExtra("id_unia", id_uniainter);
                StartActivity(intent);
            };
        }

    }
}