using System.Collections.Generic;
using System.Threading.Tasks;

namespace VE.DataAccessLayer.Interface
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<int> Insert(T data);
    }
}
