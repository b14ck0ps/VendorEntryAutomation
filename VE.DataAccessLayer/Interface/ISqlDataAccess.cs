using System.Collections.Generic;
using System.Threading.Tasks;

namespace VE.DataAccessLayer.Interface
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string sqlQuery, U parameters);
        Task<int> SaveData<T>(string sqlQuery, T parameters);
    }
}
