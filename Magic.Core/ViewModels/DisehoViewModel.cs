using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Droid.Views;

using MagicDB;
using Core.Entities;
using Magic.Core.Repository;
using System.Windows.Input;

using Magic.Core;
using Core.Repository;
using System.Collections.ObjectModel;

namespace Magic.Core.ViewModels
{
    class DisehoViewModel
        : MvxViewModel
    {
        private readonly IDataService _dataService;
        private unha _item;

        public DisehoViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public class Nav
        {
            public int Id { get; set; }
        }

        public void Init(Nav navigation)
        {
            Item = _dataService.Get(navigation.Id);
            Resource.uniaurl = Item.url;
        }

        public unha Item
        {
            get { return _item; }
            set { _item = value; RaisePropertyChanged(() => Item); }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    //_collectionService.Delete(Item);
                    Close(this);
                });
            }
        }
    }

}

/*
  private readonly ICollectionService _collectionService;
        private Collected _item;

        public DisehoViewModel(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        public class Nav
        {
            public int Id { get; set; }
        }

        public void Init(Nav navigation)
        {
            Item = _collectionService.Get(navigation.Id);
        }

        public CollectedItem Item
        {
            get { return _item; }
            set { _item = value; RaisePropertyChanged(() => Item); }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new MvxCommand(() =>
                    {
                        _collectionService.Delete(Item);
                        Close(this);
                    });
            }
        }
 */
