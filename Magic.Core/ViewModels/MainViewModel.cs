using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Repository;
using Magic;
using MagicDB;
using Android.Telephony;
using Android.Content;
using Cirrious.MvvmCross.Droid.Views;
using Android.App;

namespace Magic.Core.ViewModels
{
    public class MainViewModel 
		: MvxViewModel
        {
        static int i = 1;
        IRepositoryService _myrepo;
        public MainViewModel(IRepositoryService myrepo)
        {
            _myrepo = myrepo;
        }

        MvxCommand _insertarHistorialCommand;
        public System.Windows.Input.ICommand InsertarHistorialCommand
        {
            get
            {
                _insertarHistorialCommand = _insertarHistorialCommand ?? new MvxCommand(DoInsertarHistorialCommand);
                return _insertarHistorialCommand;
            }
        }

        void DoInsertarHistorialCommand()
        {
            string hoy = System.DateTime.Today.ToString("yyyy-MM-dd");
            TelephonyManager tManager = (TelephonyManager)Application.Context.GetSystemService(Context.TelephonyService);
            string uuid = tManager.DeviceId;
            // var usr = new usuarios { username = "titi", password = "peko", image_url = "http:**", first_name = "ley", last_name = "la", fecha = "2014-12-21", userid = "45" };
            var hst = new historial { dispositivo = uuid, fecha = hoy};
            _myrepo.Insert<historial>(hst);
        }
/*
		private string _hello = "Hello MvvmCross";
        public string Hello
		{ 
			get { return _hello; }
			set { _hello = value; RaisePropertyChanged(() => Hello); }

        }*/
    }
}
