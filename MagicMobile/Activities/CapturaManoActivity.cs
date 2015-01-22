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
using Android.Hardware;
using Android.Content.PM;
using Java.IO;
using MagicMobile.Algoritmo;
using Environment = Android.OS.Environment;

namespace MagicMobile.Activities
{
    [Activity(Label = "Cuadre la mano al contorno", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CapturaManoActivity : Activity, Android.Hardware.Camera.IPictureCallback, Android.Hardware.Camera.IPreviewCallback, Android.Hardware.Camera.IShutterCallback, ISurfaceHolderCallback
    {
        Camera camera;
        private File _dir;
        private File _file;
        private string id_unia;
        //private ImageView _imageView;
        // String PICTURE_FILENAME = "picture.jpg";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.CapturaMano);
            id_unia = Intent.GetStringExtra("id_unia") ?? "uña no disponible";
            // colocamos el elemento surface que tenndrá la captura del stream de la cámara
            SurfaceView surface = (SurfaceView)FindViewById(Resource.Id.surface);
            ISurfaceHolder holder = surface.Holder;
            holder.AddCallback(this);
            //holder.SetType(SurfaceType.PushBuffers);
            FindViewById(Resource.Id.boton).Click += delegate
            {
                Android.Hardware.Camera.Parameters p = camera.GetParameters();
                p.PictureFormat = Android.Graphics.ImageFormatType.Jpeg;
                camera.SetParameters(p);
                camera.SetDisplayOrientation(90);
                camera.TakePicture(this, this, this);
            };
        }

        void Camera.IPictureCallback.OnPictureTaken(byte[] data, Android.Hardware.Camera camera)
        {
            //código que se ejecuta luego de tomar una foto. con "Capturar"
            FileOutputStream outStream = null;
            if (data != null)
            {
                CreateDirectoryForPictures();
                _file = new File(_dir, String.Format("magic_{0}.jpg", Guid.NewGuid()));
                try
                {
                   
                    
                    outStream = new FileOutputStream(_file);
                    outStream.Write(data);
                    outStream.Close();
                    //ahora que está guardado, procedemos a cargar en una imagen.
                   
                    var intent = new Intent(this, typeof(RealidadAumentadaActivity));
                    intent.PutExtra ("path", _file.AbsolutePath);
                    intent.PutExtra("id_unha", id_unia);
                    StartActivity(intent);
                    //Vision algoritmo = new Vision();
                   // bitmap = algoritmo.Convertir(bitmap);
                    //aquí la imagen ya está convertida.
                   // _imageView.SetImageBitmap(bitmap);
                    
                    //arrancamos el activity
                   
                
                     }
                catch //(FileNotFoundException e)
                {
                   // System.Console.Out.WriteLine(e.Message);
                }/*
                catch (IOException ie)
                {
                    System.Console.Out.WriteLine(ie.Message);
                }*/
            }
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                camera = Android.Hardware.Camera.Open();
                Android.Hardware.Camera.Parameters p = camera.GetParameters();

                p.PictureFormat = Android.Graphics.ImageFormatType.Jpeg;
                camera.SetParameters(p);
                camera.SetDisplayOrientation(90);
                camera.SetPreviewCallback(this);
                camera.Lock();
                camera.SetPreviewDisplay(holder);
                camera.StartPreview();
            }
            catch
            {
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {

            camera.Unlock();
            camera.StopPreview();
            camera.Release();
        }

        void Camera.IPreviewCallback.OnPreviewFrame(byte[] b, Android.Hardware.Camera c)
        {

        }

        void Camera.IShutterCallback.OnShutter()
        {
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format f, int i, int j)
        {

        }
        //creación de la carpeta donde se almacena las unias.
        private void CreateDirectoryForPictures()
        {
            _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "MagicApp");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }
        
    }
}