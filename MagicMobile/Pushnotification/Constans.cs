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

namespace MagicMobile.Pushnotification
{
    class Constants
    {
        public const string SenderID = "813863692299"; // Google API Project Number
        // Azure app specific connection string and hub path
        public const string ConnectionString = "Endpoint=sb://magicservicehub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=Qq4AuGvz6brXjIyuswwLX/12XkMOubstOb+34jOvNN8=";
        public const string NotificationHubPath = "magicservicehub";//"<hub path>";

    }
}