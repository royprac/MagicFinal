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
using Java.IO;
using Android.Graphics;
using Android.Graphics.Drawables;
using MagicMobile.Algoritmo;
using Android.Content.PM;
using Java.Net;
using System.Net;

namespace MagicMobile.Activities
{
    [Activity(Label = "RealidadAumentadaActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class RealidadAumentadaActivity : Activity
    {
        private File _file;
        private string path_bmp;
        private string unia_url;
        //private int id_unha;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);
            //aquí comiennza
            SetContentView(Resource.Layout.RealidadAumentada);
            // Create your application here
            path_bmp = Intent.GetStringExtra("path") ?? "Imagen No Disponible";
            //string temp = Intent.GetStringExtra("id_unha") ?? "Unia no disponible";
            // var imgMVX = FindViewById<ImageView>(Resource.Id.imageMayor1);
            // id_unha = Convert.ToInt32(Resource);
            _file = new File(path_bmp);
            //+++++++++++++++++++++++++++++++++++++++++++++ algoritmo

            //guardamos y rotamos una copia de la uña en una versión pequeña para el algorimo
            Bitmap bmp_pequenio = _file.Path.LoadAndResizeBitmap(200, 120);
            Matrix mat = new Matrix();
            mat.PostRotate(90);
            Bitmap bmp_algoritmo = Bitmap.CreateBitmap(bmp_pequenio, 0, 0, bmp_pequenio.Width, bmp_pequenio.Height, mat, true);
            //creamos el algoritmo
            Vision vision = new Vision();
            vision.bmp_original = bmp_algoritmo;
            bmp_algoritmo = vision.Construir();
            //recuperaos la unia
            Context contexto = this;
            Android.Content.Res.Resources res = contexto.Resources;
            // Bitmap bmp_unia = BitmapFactory.DecodeResource(res, id_unha);
            //***BitmapDrawable draw = (BitmapDrawable) imgMVX.Drawable;
            //***Bitmap bmp_unia = draw.Bitmap;
            //URL url = new URL("https://lh4.googleusercontent.com/-UlBIFixJ3q8/VK4PBXQMdtI/AAAAAAAAC48/7vuP91oK1I8/w40-h90-no/cari_1.png");
            //--Bitmap bmp_unia = BitmapFactory.DecodeResource(res, Resource.Drawable.unia_1_01);
            unia_url = Core.Entities.Resource.uniaurl;
            Bitmap bmp_unia = GetImageBitmapFromUrl(unia_url);
            //bmp_unia = Bitmap.CreateScaledBitmap(bmp_unia, 40, 90, true);
            //bitmap que se reducirá al tamaño de la pantalla
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = Resources.DisplayMetrics.WidthPixels;
            Bitmap bitmap = _file.Path.LoadAndResizeBitmap(height, width);
            Matrix matrix = new Matrix();
            matrix.PostRotate(90);
            Bitmap bmp_final = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
            //añadimos las uñas sólo al modelo bmp_final
            Bitmap bmp_final2 = bmp_final.Copy(Bitmap.Config.Argb8888, true);
            //bmp_final2 = vision.Superponer(bmp_final2, bmp_unia, 50, 100);
            bmp_final2 = vision.Ubicar(bmp_final2, bmp_algoritmo, bmp_unia, bmp_unia);
            //finalmente mostramos el resultado de la superposición.
            var imageView = FindViewById<ImageView>(Resource.Id.imgaumentada);
            imageView.SetImageBitmap(bmp_final2);

            //+++++++++++++++++++++++++++++++++++++++++++++ algoritmo

            var boton_procesar = FindViewById<Button>(Resource.Id.ra_ok);
            boton_procesar.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CatalogoActivity));
                StartActivity(intent);
            };


            var boton_nogusto = FindViewById<Button>(Resource.Id.ra_nogusto);
            boton_nogusto.Click += (sender, e) =>
            {
                var intent2 = new Intent(this, typeof(ModoAvatar));
                StartActivity(intent2);
            }; 
        }

        

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}