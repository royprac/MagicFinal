
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Core.DownloadCache; 
using Microsoft.WindowsAzure.MobileServices;
using System.Reflection;

using Core.Entities.json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{

    public class WAMSRepositoryService : IRepositoryService
    {

        public MobileServiceClient MobileService;
        public WAMSRepositoryService()
        {
            MobileService = new MobileServiceClient("https://magicservicio.azure-mobile.net/",
    "mkcJrBUYDfByQMGygvdxekTHvbpCwv58");
        }
        async public Task Insert<T>(T obj)
        {

            await MobileService.GetTable<T>().InsertAsync(obj);



        }


        async public Task<List<T>> Where<T>(System.Linq.Expressions.Expression<Func<T, bool>> condition)
        {
            List<T> list = await MobileService.GetTable<T>().Where(condition).ToListAsync();

            return list;
        }
    }
}
