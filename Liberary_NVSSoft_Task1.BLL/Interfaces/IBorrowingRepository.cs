using Liberary_NVSSoft_Task1.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liberary_NVSSoft_Task1.BLL.Interfaces
{
    public interface IBorrowingRepository : IGenericRepository<Borrowing>
    {
        //User GetById(int? id);
        //IEnumerable<User> GetAll();
        //bool Add(User obj);
        //bool Update(User obj);
        //bool Delete(User obj);
        bool GetBookBorrowStatusByBookId(int BookId);

    }
}
