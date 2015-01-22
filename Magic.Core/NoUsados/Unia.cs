using System;
using System.Text;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace Magic.Core.Repository
{

    public class Unia
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int price { get; set; }
        
        public override string ToString()
        {
            return string.Format("{0} ({1})", name, price);
        }
    }
}
