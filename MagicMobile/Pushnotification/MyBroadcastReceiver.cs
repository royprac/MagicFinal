using System;
using System.Collections.Generic;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ByteSmith.WindowsAzure.Messaging;
using Gcm.Client;
using Android.Util;
using System.Diagnostics;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

// GET_ACCOUNTS sólo es necesaria para las versiones de Android 4.0.3 y posteriores
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace MagicMobile.Pushnotification
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    class MyBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SENDER_IDS = new string[] { Constants.SenderID };

        public const string TAG = "MyBroadcastReceiver-GCM";

    }

    // GcmServiceBase implementa los metodos OnRegistered(), OnUnRegistered(), OnMessage(), OnRecoverableError(), and OnError().
    // estos métodos se ejecutaran en respuesta a la interacción con la notificación hub.

    [Service] //Se debe usar la etiqueta de servicio
    public class GcmService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }
        private NotificationHub Hub { get; set; }

        public GcmService()
            : base(Constants.SenderID)
        {
            Log.Info(MyBroadcastReceiver.TAG, "Constructor GcmService()");
        }

        // OnRegistered(), se debe tener en cuenta la posibilidad de especificar las etiquetas
        //para registrar especifico canales de mensajería.
        protected override async void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "Registro GCM: " + registrationId);
            RegistrationID = registrationId;

            createNotification("GcmService Registrado...", "El dispositivo ha sido registrado, Pulse para verlo!");

            Hub = new NotificationHub(Constants.NotificationHubPath, Constants.ConnectionString);
            try
            {
                await Hub.UnregisterAllAsync(registrationId);
            }
            catch (Exception ex)
            {
            //    Debug.WriteLine(ex.Message);
            //    Debugger.Break();
            }

            var tags = new List<string>() { "falcons" }; // Se crea el tags 

            try
            {
                var hubRegistration = await Hub.RegisterNativeAsync(registrationId, tags);
                Debug.WriteLine("RegistracionId:" + hubRegistration.RegistrationId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM no Resgitrado: " + registrationId);


            createNotification("GcmService No registrado...", "El dispositivo no ha sido registrado, Pulse para verlo!");
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info(MyBroadcastReceiver.TAG, "Mensaje GCM Recibido!");

            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
            }



            string messageText = intent.Extras.GetString("msg");
            if (!string.IsNullOrEmpty(messageText))
            {
                createNotification("Nuevo Mensaje Hub!", messageText);
                return;
            }

            createNotification("Detalles del mensaje", msg.ToString());
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Log.Warn(MyBroadcastReceiver.TAG, " Error Recuperable: " + errorId);

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error(MyBroadcastReceiver.TAG, "Error GCM: " + errorId);
        }

        void createNotification(string title, string desc)
        {
            //Creacion de notificacion
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Creacion del intent y muestra  la IU
            //Cuando el Mensaje es pulsado no envia a esta actividad (MensajeActivity)
            var uiIntent = new Intent(this, typeof(MensajeActivity));

            //Creacion de notificacion
            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

            //AutoCancel : eliminará la notificación una vez que el usuario lo toca.
            notification.Flags = NotificationFlags.AutoCancel;

            //Envia la informacion de la notificacion 
            // Se usa el intent pendiente, pasando la IU del intent donde sera llamado cuando la notificacion es tocada
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

            //Muestra la notificacion
            notificationManager.Notify(1, notification);
        }

    }

}