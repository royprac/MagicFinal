//using System.Collections.Generic;
using System.Collections.Generic;
using Core.Entities;
using MagicDB;
namespace Magic.Core.Repository
{
    public interface IDataService
    {
        List<unha> KittensMatching(string nameFilter);
        List<unha> RelacionUnias(int cat_id);
        unha Get(int nav_id);
        void Insert(unha unha);
        void Update(unha unha);
        void Delete(unha unha);
        int Count { get; }
    }
}