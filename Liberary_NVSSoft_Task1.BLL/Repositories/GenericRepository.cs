using Liberary_NVSSoft_Task1.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liberary_NVSSoft_Task1.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _connectionString;

        public GenericRepository()
        {
            _connectionString = "server=(local);Database=Liberary_NVSSoft_Task1;Integrated Security=true";
        }

        public bool Add(T obj)
        {
            return false;
        }

        public bool Delete(T obj)
        {
            return false;
        }

        public IEnumerable<T> GetAll()
        {
            return null;
        }

        public T GetById(int? id)
        {
            return null;
        }

        public bool Update(T obj)
        {
            return false;
        }
    }
}
