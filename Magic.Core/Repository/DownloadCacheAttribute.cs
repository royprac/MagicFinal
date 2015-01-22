﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DownloadCache
{
    [AttributeUsage(AttributeTargets.Property)] 
    public class DownloadCacheAttribute : System.Attribute
    {
        public override string ToString()
        {
            return "DownloadCacheable";
        }
    }   
}
