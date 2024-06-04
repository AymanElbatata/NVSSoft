using Liberary_NVSSoft_Task1.BLL.Interfaces;
using Liberary_NVSSoft_Task1.DAL.Entities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liberary_NVSSoft_Task1.BLL.Repositories
{
    public class BorrowRepository : GenericRepository<Borrowing>, IBorrowingRepository
    {
        private readonly string _connectionString;
        private readonly BookRepository BookRepository;
        private readonly UserRepository UserRepository;

        public BorrowRepository(BookRepository BookRepository, UserRepository UserRepository)
        {
            _connectionString = "server=(local);Database=Liberary_NVSSoft_Task1;Integrated Security=true";
            this.BookRepository = BookRepository;
            this.UserRepository = UserRepository;
        }

        public BorrowRepository():this(new BookRepository(),new UserRepository())
        {
        }

        public bool ExistsBookBorrow(int BookId, int UserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //var query = "SELECT COUNT(*) FROM BorrowingTBL WHERE BookId = @BookId AND UserId = @UserId AND IsDone = 0 AND IsDeleted = 0"; // Adding IsDeleted condition
                var query = "SELECT COUNT(*) FROM BorrowingTBL WHERE BookId = @BookId AND UserId = @UserId AND IsDeleted = 0"; // Adding IsDeleted condition
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", BookId);
                    command.Parameters.AddWithValue("@UserId", UserId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool BookBorrowExistsById(int BorrowingId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM BorrowingTBL WHERE BorrowingId = @BorrowingId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowingId", BorrowingId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        public bool GetBookBorrowStatusByBookId(int BookId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM BorrowingTBL WHERE BookId = @BookId AND IsDone = 0 AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", BookId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool Add(Borrowing obj)
        {
            if (ExistsBookBorrow((int)obj.BookId, (int)obj.UserId))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO BorrowingTBL (UserId,BookId,IsDone) VALUES (@UserId,@BookId,@IsDone)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", obj.UserId);
                    command.Parameters.AddWithValue("@BookId", obj.BookId);
                    command.Parameters.AddWithValue("@IsDone", 0);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool Update(Borrowing obj)
        {
            //if (!BookBorrowExistsById(obj.BorrowingId ?? 0) || !ExistsBookBorrow((int)obj.BookId, (int)obj.UserId))
            if (!BookBorrowExistsById(obj.BorrowingId ?? 0) || !ExistsBookBorrow((int)obj.BookId, (int)obj.UserId))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE BorrowingTBL SET UserId = @UserId, BookId=@BookId, IsDone=@IsDone WHERE BorrowingId = @BorrowingId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", obj.UserId);
                    command.Parameters.AddWithValue("@BookId", obj.BookId);
                    command.Parameters.AddWithValue("@IsDone", obj.IsDone);
                    command.Parameters.AddWithValue("@BorrowingId", obj.BorrowingId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool Delete(Borrowing obj)
        {
            if (!BookBorrowExistsById(obj.BorrowingId ?? 0))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE BorrowingTBL SET IsDeleted = @IsDeleted WHERE BorrowingId = @BorrowingId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsDeleted", 1);
                    command.Parameters.AddWithValue("@BorrowingId", obj.BorrowingId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public IEnumerable<Borrowing> GetAll()
        {
            var Borrowings = new List<Borrowing>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM BorrowingTBL WHERE IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var borrowing = new Borrowing
                            {
                                BorrowingId = reader.GetInt32(reader.GetOrdinal("BorrowingId")),
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                IsDone = reader.GetBoolean(reader.GetOrdinal("IsDone")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                            };
                            borrowing.User = UserRepository.GetById(borrowing.UserId);
                            borrowing.Book = BookRepository.GetById(borrowing.BookId);
                            Borrowings.Add(borrowing);
                        }
                    }
                }
            }

            return Borrowings;        
    }

        public Borrowing GetById(int? id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM BorrowingTBL WHERE BorrowingId = @BorrowingId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BorrowingId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var borrowing =  new Borrowing
                            {
                                BorrowingId = reader.GetInt32(reader.GetOrdinal("BorrowingId")),
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                IsDone = reader.GetBoolean(reader.GetOrdinal("IsDone")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))

                            };
                            borrowing.User = UserRepository.GetById(borrowing.UserId);
                            borrowing.Book = BookRepository.GetById(borrowing.BookId);
                            return borrowing;
                        }
                        else
                        {
                            // If no user found with the specified UserId, return null or throw an exception as needed
                            return new Borrowing();
                        }
                    }
                }
            }
        }


    }
}
