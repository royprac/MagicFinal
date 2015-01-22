using System;
using System.Text;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace Magic.Core.Repository
{

    public class Kitten
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Price);
        }
    }
}
