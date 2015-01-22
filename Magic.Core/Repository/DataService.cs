using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Core.Entities;
using MagicDB;

namespace Magic.Core.Repository
{
    public class DataService : IDataService
    {
        private readonly ISQLiteConnection _connection;

        public DataService(ISQLiteConnectionFactory factory)
        {
            _connection = factory.Create("mag5.sql");
            _connection.CreateTable<unha>();
        }

        public List<unha> KittensMatching(string nameFilter)
        {
            return _connection.Table<unha>()
                              .OrderBy(x => x.id)
                              .Where(x => x.nombre.Contains(nameFilter))
                              .ToList();
        }

        public List<unha> RelacionUnias(int cat_id)
        {
            return _connection.Table<unha>()
                              .OrderBy(x => x.id)
                              .Where(x => x.categoria_id == cat_id)
                              .ToList();
        }

        public unha Get(int nav_id)
        {
            return _connection.Table<unha>()
                              .OrderBy(x => x.id)
                              .Where(x => x.id == nav_id).First();
                              
        }

        public void Insert(unha unha)
        {
            _connection.Insert(unha);
        }

        public void Update(unha unha)
        {
            _connection.Update(unha);
        }

        public void Delete(unha unha)
        {
            _connection.Delete(unha);
        }

        public int Count
        {
            get
            {
                return _connection.Table<unha>().Count();
            }
        }
    }

    /*
     Copiandoo...
     * 
     *
      public class DataService : IDataService
    {
        private readonly ISQLiteConnection _connection;

        public DataService(ISQLiteConnectionFactory factory)
        {
            _connection = factory.Create("magic.sql");
            _connection.CreateTable<Kitten>();
        }

        public List<Kitten> KittensMatching(string nameFilter)
        {
            return _connection.Table<Kitten>()
                              .OrderBy(x => x.Price)
                              .Where(x => x.Name.Contains(nameFilter))
                              .ToList();
        }

        public void Insert(Kitten kitten)
        {
            _connection.Insert(kitten);
        }

        public void Update(Kitten kitten)
        {
            _connection.Update(kitten);
        }

        public void Delete(Kitten kitten)
        {
            _connection.Delete(kitten);
        }

        public int Count
        {
            get
            {
                return _connection.Table<Kitten>().Count();
            }
        }
    }
     */
}