
using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Xamarin.Social.Services;
using Xamarin.Auth;
using System.Json;
using Android.Views;
using System.Net;
using Android.Graphics;
using Cirrious.MvvmCross.Droid.Views;
using Android.Util;
using MagicMobile;
using Xamarin.Social;

namespace Magic.util
{
  
    public class Social 
    {

        #region Fields

        private static FacebookService mFacebook;
        private static FlickrService mFlickr;
        private static TwitterService mTwitter;
        #endregion
        Context context;


        public Social(Context _context)
        {
            context = _context;
        }

        public Xamarin.Social.Service getService( string value)
        { 
         
             if (value == "facebook") return Facebook;
             if (value == "flicker") return Flickr;
             if (value == "twitter") return Twitter;
             else return null;
         
        }
        public static FacebookService Facebook
        {
            get
            {
                if (mFacebook == null)
                {
                    mFacebook = new FacebookService()
                    {
                        ClientId = "421814437973972",
                        RedirectUrl = new Uri("http://www.facebook.com/connect/login_success.html")
                    };
                }

                return mFacebook;
            }
        }

        public static FlickrService Flickr
        {
            get
            {
                if (mFlickr == null)
                {
                    mFlickr = new FlickrService()
                    {
                        ConsumerKey = "Key from http://www.flickr.com/services/apps/by/me",
                        ConsumerSecret = "Secret from http://www.flickr.com/services/apps/by/me",
                    };
                }

                return mFlickr;
            }
        }

        public static TwitterService Twitter
        {
            get
            {
                if (mTwitter == null)
                {
                    mTwitter = new TwitterService
                    {
                        ConsumerKey = "Consumer key from https://dev.twitter.com/apps",
                        ConsumerSecret = "Consumer secret from https://dev.twitter.com/apps",
                        CallbackUrl = new Uri("Callback URL from https://dev.twitter.com/apps")
                    };
                }

                return mTwitter;
            }
        }

        public async Task Share(Xamarin.Social.Service service, string text, string url)
        {
            Item item = new Item
            {
                Text = text,
                Links = new List<Uri> {
					new Uri (url),
				},
            };

            Account cuentamaestra = new Account();
            // verificar si existe cuenta activa

            await service.GetAccountsAsync((MvxActivity)context).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    ShowMessage("Share Error" + t.Exception);
                }
                else
                {
                    var cuentas = t.Result;
                    foreach (Account a in cuentas)
                    {
                        cuentamaestra = a;
                        break;
                    }

                    if (cuentamaestra.Properties.Count > 0)
                    {
                        try
                        {
                            service.ShareItemAsync(item, cuentamaestra);
                            ShowDialog(new List<string>() { "Datos Compartidos", "Lograste compartir tus logros" });
                                    
                        }
                        catch (Exception ee)
                        {
                            //ShowMessage("errores encontrados" + ee.ToString());
                            Log.Info("Errores", "errores");
                        }
                    }
                    else
                    {
                        ShowDialog(new List<string>(){"Error en cuenta","No tiene cuenta activa , vuelva a loguearse"});
                    }
                }
            });



        }


      public  async Task<List<Account>> GetUsers(Xamarin.Social.Service service)
        {

            List<Account> Maestro = new List<Account>();
            await service.GetAccountsAsync((MvxActivity)context).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                   // ShowMessage("Error en Lista de usuarios" + t.Exception);
                    //throw;
                    Log.Info("Errores","errores");
                }
                else
                {
                    var cuentas = t.Result;
                    foreach (Account a in cuentas)
                    {
                        Maestro.Add(a);
                    }
                }
            });
            return Maestro;

        }


      public  async Task logout(Xamarin.Social.Service service)
        {

            var usuarios = await GetUsers(service);
            // eleiminamos cuentas
            foreach (Account i in usuarios)
                service.DeleteAccount(context, i);
            //Util.DeleteFile();
        }

      public  void Login(Xamarin.Social.Service service, Action<Account> EventoCompletado )
        {

            Intent intent = service.GetAuthenticateUI((MvxActivity)context, EventoCompletado );
            context.StartActivity(intent);
        }



      
        private void ShowMessage(String message)
        {
            Toast.MakeText(context, message, ToastLength.Long).Show();

        }

       


        public static async Task Bitmap(Context _context, ImageView ln, string url)
        {
            try
            {
                Bitmap bm = await downloadAsync(url);
                // CircleImageView im = new CircleImageView(_context);
                //im.LayoutParameters = new ViewGroup.LayoutParams(150, 150);
                //im.SetImageBitmap(bm);
                ln.SetImageBitmap(bm);
                //ln.AddView(bm);
            }
            catch (Exception e)
            { }
        }


        public static async Task<Bitmap> downloadAsync(string path)
        {
            WebClient webClient;
            webClient = new WebClient();
            var url = new Uri(path);
            byte[] imageBytes = null;
            try
            {
                imageBytes = await webClient.DownloadDataTaskAsync(url);
            }
            catch (Exception ee)
            {
                //Mvx.Trace(ee.Message);
            }
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            return bitmap;
        }

        public void ShowDialog(List<string> datos)
        {
            try
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(context);
                builder.SetTitle(datos[0]);
                //builder.SetIcon(Resource.Drawable.bg);
                builder.SetMessage(datos[1]);
                builder.SetPositiveButton("OK", delegate { builder.Dispose(); });

                builder.Show();
            }
            catch (Exception e)
            {
                Log.Info(e.Message, "Errores" + e.Message);
            }
        }

    }
}