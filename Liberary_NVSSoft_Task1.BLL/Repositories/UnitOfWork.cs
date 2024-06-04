using Liberary_NVSSoft_Task1.BLL.Interfaces;
using Liberary_NVSSoft_Task1.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liberary_NVSSoft_Task1.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }
        public IAuthorRepository AuthorRepository { get; set; }
        public IBookRepository BookRepository { get; set; }
        public IBorrowingRepository BorrowingRepository { get; set; }


        public UnitOfWork(IUserRepository UserRepository, IAuthorRepository AuthorRepository, IBookRepository BookRepository, IBorrowingRepository BorrowingRepository)
        {
            this.UserRepository = UserRepository;
            this.AuthorRepository = AuthorRepository;
            this.BookRepository = BookRepository;
            this.BorrowingRepository = BorrowingRepository;
        }
    }
}
