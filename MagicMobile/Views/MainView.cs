using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Cirrious.MvvmCross.Droid.Views;
using Microsoft.WindowsAzure.MobileServices;
using Android.Content.PM;
using Magic.Core.ViewModels;
using System.ComponentModel;
using Android.Telephony;
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Json;
using Magic.util;

using ByteSmith.WindowsAzure.Messaging;
using Gcm.Client;
using MagicMobile.Pushnotification;
//using Android.Support.V4.App.FragmentTransaction;


namespace MagicMobile
{
    [Activity(Label = "Magic", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainView : MvxActivity                 	
	{/*
        public static MobileServiceClient MobileService = new MobileServiceClient(
    "https://notificacionmagic.azure-mobile.net/",
    "YleiVpKNnlCAJJRcNlJSBZSFKHToge65"
);*/

///////////////facebook/////////////////

        Social autenticador;
        static string _name = "";
        static string _id = "";
        static string token = "";
        
        ViewPager vistaImagenes;
        LinearLayout mainLayout;

/// ///////////////FACE////////////////////////////////


       private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

/// /////////////////////////////////////////////


        public static string uuid;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Window.RequestFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.MainView);

            //cargar el registro de notificacion
            RegisterWithGCM();

            autenticador = new Social(this);
            int[] imagenes = {
                            Resource.Drawable.magic3,
                            Resource.Drawable.tutorial1,
                            Resource.Drawable.tutorial2,
                            Resource.Drawable.tutorial3,
                        };

            LinearLayout ly = (LinearLayout)FindViewById(Resource.Id.layout1);
            vistaImagenes = new ViewPager(this);
            ViewPagerImagesAdapter adapter = new ViewPagerImagesAdapter(this, imagenes, Resources, this);
            vistaImagenes.Adapter = adapter;
            ly.AddView(vistaImagenes);


            var callHistoryButton1 = FindViewById<Button>(Resource.Id.button1);
            callHistoryButton1.Click += (sender, e) =>
            {
                var intent3 = new Intent(this, typeof(CatalogoActivity));
                StartActivity(intent3);
            };

            //////////////////////////////BOTON DE FACEBOOK/////////////////////////////////////

            var facebook = FindViewById<ImageButton>(Resource.Id.myButtonFacebook);
            facebook.Click += delegate
            {
                Login();
            };

        } 


  //////////////////////////////////UNA VEZ LOGUEADO ENTRA A BEGIN////////////////////////////

        private async Task Begin()
        {

            List<Account> lista = await logued();

            if (lista.Count > 0)
            {
                // inicializamos parametros y creamos bton siguiente
                // cargamos datos
                string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", _id, "normal", token);
                //await loadUserdata(name, profilePictureUrl);
                // load basic data
                Toast.MakeText(this, "Ya existe cuenta activa", ToastLength.Short);


                var ingresar = FindViewById<ImageButton>(Resource.Id.button1);
                ingresar.Visibility = ViewStates.Visible;
                var facebook = FindViewById<ImageButton>(Resource.Id.myButtonFacebook);
                facebook.Visibility = ViewStates.Invisible;

            }
            else
            {
                var facebook = FindViewById<ImageButton>(Resource.Id.myButtonFacebook);
                facebook.Visibility = ViewStates.Visible;

            }

        }



        private async Task<List<Account>> logued()
        {
            bool log = false;
            Xamarin.Social.Service servicio = autenticador.getService("facebook");
            var a = await autenticador.GetUsers(servicio);
            return a;
        }

        private void Login()
        {
            Xamarin.Social.Service servicio = autenticador.getService("facebook");
            autenticador.Login(servicio, EventoCompletado);

        }

        private void EventoCompletado(Xamarin.Auth.Account obj)
        {
            if (obj != null)
            {
                Xamarin.Social.Service servicio = autenticador.getService("facebook");
                var request = servicio.CreateRequest("GET", new Uri("https://graph.facebook.com/me"), obj);
                request.GetResponseAsync().ContinueWith(t =>
                {
                    var builder = new AlertDialog.Builder(this);
                    if (t.IsFaulted)
                    {
                        builder.SetTitle("Error");
                        builder.SetMessage(t.Exception.Flatten().InnerException.ToString());
                    }
                    else if (t.IsCanceled)
                        builder.SetTitle("Tarea Cancelada");
                    else
                    {
                        var data = JsonValue.Parse(t.Result.GetResponseText());
                        string userid = data["id"];
                        string name = data["name"];
                        string accesstoken = obj.Properties["access_token"];
                        string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", userid, "normal", accesstoken);
                        AccountStore.Create(this).Save(obj, "Facebook");
                        builder.SetTitle("Logged in");
                        builder.SetMessage("Name: " + name);


                    }

                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();


                    var intent4 = new Intent(this, typeof(CatalogoActivity));
                    StartActivity(intent4);


                    var facebook = FindViewById<ImageButton>(Resource.Id.myButtonFacebook);
                    facebook.Visibility = ViewStates.Invisible;
                    var ingresar = FindViewById<ImageButton>(Resource.Id.button1);
                    ingresar.Visibility = ViewStates.Visible;

                }, UIScheduler);
            }

        }
/////////////////////////GALERIA DE IMAGENES///////////////////////////////////////
/*
		Gallery gallery = (Gallery) FindViewById<Gallery>(Resource.Id.gallery);

			gallery.Adapter = new MainSplashActivity (this);

			gallery.ItemClick += delegate (object sender, Android.Widget.AdapterView.ItemClickEventArgs args) {
				Toast.MakeText (this, args.Position.ToString (), ToastLength.Short).Show ();
			};
        */
//////////////////////////////BOTON DE INGRESO//////////////////////////////////////
           
/*
            var callHistoryButton1= FindViewById<Button>(Resource.Id.button1);

            callHistoryButton1.Click += (sender, e) =>
            {
                var intent3 = new Intent(this, typeof(CatalogoActivity));
                
                StartActivity(intent3);
            };  
        */
//////////////////////////////BOTON DE FACEBOOK/////////////////////////////////////
     /*       var facebook = FindViewById<ImageButton>(Resource.Id.myButtonFacebook);
            facebook.Click += delegate
            {
                LoginToFacebook(true);
                   { 
               var intent4 = new Intent(this, typeof(CatalogoActivity));
               StartActivity(intent4);
                    };


            };	
            
		} */

//////////////////////////////////////////////////////////

        //clase para el registro de PushNotification
        private void RegisterWithGCM()
        {
            // Comprueba la configuración correcta 
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);
            // Registro para notificaciones push. 
            System.Diagnostics.Debug.WriteLine("Registering...");
            GcmClient.Register(this, MagicMobile.Pushnotification.Constants.SenderID);

        }
////////////////////////////////////////////////////////////////
   	}
	
}


