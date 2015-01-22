using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Core.Entities
{
    public class Resource
    {
        public string Id { get; set; }
        public string LocalPath { get; set; }
        public string CloudPath { get; set; }
        public string ContainerName { get; set; }
        public string SasQueryString { get; set; }
        public static int categoria = 0;
        public static string uniaurl = "https://lh4.googleusercontent.com/-UlBIFixJ3q8/VK4PBXQMdtI/AAAAAAAAC48/7vuP91oK1I8/w40-h90-no/cari_1.png";
    }
}

