using Liberary_NVSSoft_Task1.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liberary_NVSSoft_Task1.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        T GetById(int? id);
        IEnumerable<T> GetAll();
        bool Add(T obj);
        bool Update(T obj);
        bool Delete(T obj);
    }
}
