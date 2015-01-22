using Core.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IRepositoryService
    {
        Task Insert<T>(T obj);
        Task<List<T>> Where<T>(Expression<Func<T, bool>> condition);
    }
}
