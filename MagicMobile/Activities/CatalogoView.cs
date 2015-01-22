using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MagicMobile.Activities;
using Android.Content.PM;
using MagicMobile.Views;
using Cirrious.MvvmCross.Droid.Views;
using Magic.Core.ViewModels;
namespace MagicMobile
{
    [Activity(Label = "Catalogo",  Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
	public class CatalogoActivity : Activity
	{
	/*	protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);
           // Window.RequestFeature(WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.Catalogo);

            var Button1 = FindViewById<ImageButton>(Resource.Id.myButtonCateg1);
            var Button2 = FindViewById<ImageButton>(Resource.Id.myButtonCateg2);
            var Button3 = FindViewById<ImageButton>(Resource.Id.myButtonCateg3);
            var Button4 = FindViewById<ImageButton>(Resource.Id.myButtonCateg4);
            var Button5 = FindViewById<ImageButton>(Resource.Id.myButtonCateg5);
            var Button6 = FindViewById<ImageButton>(Resource.Id.myButtonCateg6);
            
			Button1.Click += (sender, e) => {
				var intent = new Intent (this, typeof(Grid1Activity));
				StartActivity (intent);
			};

            Button2.Click += (sender, e) =>
            {
                var intent = new Intent (this, typeof(Grid2Activity));
                StartActivity(intent);
            };

            Button3.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(Grid3Activity));
                StartActivity(intent);
            };

            Button4.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(Grid4Activity));
                StartActivity(intent);
            };

            Button5.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(Grid5Activity));
                StartActivity(intent);
            };

            Button6.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(Grid6Activity));
                StartActivity(intent);
            };
		
		}  */








        string[] items;
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ////////////////////////////////////////////////////  DATOS JALAR DE BASE //////////////////////////////
            //    items = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };

            tableItems.Add(new TableItem() { Heading = "Cartoons", SubHeading = "65 designs", ImageResourceId = Resource.Drawable.categ1 }); 
            tableItems.Add(new TableItem() { Heading = "Casual", SubHeading = "17 designs", ImageResourceId = Resource.Drawable.categ5 }); 
            tableItems.Add(new TableItem() { Heading = "Flower", SubHeading = "5 designs", ImageResourceId = Resource.Drawable.categ6 }); 
            tableItems.Add(new TableItem() { Heading = "Party", SubHeading = "33 designs", ImageResourceId = Resource.Drawable.categ2 }); 
            tableItems.Add(new TableItem() { Heading = "Elegant", SubHeading = "18 designs", ImageResourceId = Resource.Drawable.categ4 }); 
            tableItems.Add(new TableItem() { Heading = "Work", SubHeading = "43 designs", ImageResourceId = Resource.Drawable.categ3 }); 

            SetContentView(Resource.Layout.HomeScreen);
            listView = FindViewById<ListView>(Resource.Id.List);
            listView.Adapter = new HomeScreenAdapter(this, tableItems);
            ///////////////////////////////////////////////////////////////////////////////////       
            //ListView.ChoiceMode = ChoiceMode.Single;
            //ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
            //ListAdapter = new HomeScreenAdapter(this, tableItems);
            listView.ItemClick += OnListItemClick;


        }

        // public override void OnListItemClick(ListView l, View v, int position, long id);


        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = tableItems[e.Position];
            Android.Widget.Toast.MakeText(this, t.Heading, Android.Widget.ToastLength.Short).Show();
            var callHistoryButton1 = (ListView)FindViewById<ListView>(e.Position);
            //obtener el ID de la categoría. e.
            Console.WriteLine(e.Position.ToString());
            string idcategoria = e.Position.ToString();
            //cargar la siguiente unia
            var uniasview = new Intent(this, typeof(UniasView));
            uniasview.PutExtra("id_categoria",idcategoria);

           // var mv = ViewModel as UniasViewModel;
           // mv.IdCategoria = Convert.ToInt16(idcategoria);
            Core.Entities.Resource.categoria = Convert.ToInt16(idcategoria);
            StartActivity(uniasview);
      
            ////////////////////////////////////////////////////// JALAR POR ID //////////////////////////////7 
     //       callHistoryButton1.Click += (senders, e) =>
     //       {
     //           var disenhosPorCatalogo = new Intent(this, typeof(DisenhosPorCatalogoActivity));
     //           StartActivity(disenhosPorCatalogo);
     //       };
        }



        /////////////////////////////////////////

        public class HomeScreenAdapter : BaseAdapter<TableItem>
        {
            //string[] items;
            List<TableItem> items;
            Activity context;
            public HomeScreenAdapter(Activity context, List<TableItem> items)
                : base()
            {
                this.context = context;
                this.items = items;
            }
            public override long GetItemId(int position)
            {
                return position;
            }


            public override TableItem this[int position]
            {
                get { return items[position]; }
            }

            public override int Count
            {
                get { return items.Count; }
            }
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var item = items[position];
                View view = convertView;
                if (view == null)
                { // no view to re-use, create new
                    view = context.LayoutInflater.Inflate(Resource.Layout.CatalogoFinal, null);
                    view.FindViewById<ImageView>(Resource.Id.Image1).SetImageResource(item.ImageResourceId);
                    view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Heading;
                    view.FindViewById<TextView>(Resource.Id.Text2).Text = item.SubHeading;
                    
                } return view;
            }
        }

        /////////////////////////////////////////////////////////////////
    

}
}


