using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Magic.Core;
using Magic.Core.Repository;
using Core.Repository;
using MagicDB;
using System.Collections.ObjectModel;
using Cirrious.MvvmCross.Droid.Views;
using Core.Entities;
using System.Windows.Input;

namespace Magic.Core.ViewModels
{
    public class UniasViewModel
        : MvxViewModel
    {
        //** cambio de repositorio magic ***//
        IRepositoryService _myrepo;

        public UniasViewModel(IRepositoryService myrepo, IDataService dataService)
        {
             _idCategoria = Resource.categoria + 1;
            // preguntar a SQLite si hay datos.
            //------cargamos datos de SQLlite.
            _dataService = dataService;
            CargarDeSqlite();
            int n_datos = ListaUnias.Count;
            //------si no hay datos... empezamos a jalar de la nube
            if (n_datos == 0)
            {
                //mientras se van cargando.. se deben de ir insertando.
                _myrepo = myrepo;
                LoadUnias();
            }
                      
        }

        async private void LoadUnias()
        {
            var list = await _myrepo.Where<unha>(c => c.categoria_id == _idCategoria & c.pulgar==0);
            ListaUnias = new ObservableCollection<unha>(list);
            //insertar unias en Persistencia
            foreach (unha item in ListaUnias)
            {
                _dataService.Insert(item);
            }
        }

        ObservableCollection<unha> _listaUnias;
        public ObservableCollection<unha> ListaUnias
        {
            get { return _listaUnias; }
            set { _listaUnias = value; RaisePropertyChanged("ListaUnias"); }
        }

        private void CargarDeSqlite()
        {
            var Glist = new List<unha>();
            Glist = _dataService.RelacionUnias(_idCategoria);
            ListaUnias = new ObservableCollection<unha>(Glist);
        }

        private readonly IDataService _dataService;
        //private readonly IKittenGenesisService _kittenGenesisService;

        private int _idCategoria;
        public int IdCategoria
        {
            get { return _idCategoria; }
            set { _idCategoria = value; RaisePropertyChanged(() => IdCategoria); }
        }

        //listar hacer clic en los subcomandos
        public ICommand ShowDetailCommand
        {
            get
            {
                return new MvxCommand<unha>(item => ShowViewModel<DisehoViewModel>(new DisehoViewModel.Nav() { Id = item.id }));
            }
        }
    }
}
