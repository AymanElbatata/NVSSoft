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
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly string _connectionString;
        private readonly AuthorRepository AuthorRepository;

        public BookRepository(AuthorRepository AuthorRepository)
        {
            _connectionString = "server=(local);Database=Liberary_NVSSoft_Task1;Integrated Security=true";
            this.AuthorRepository = AuthorRepository;
        }

        public BookRepository():this(new AuthorRepository())
        {
        }

        public bool ExistsBookISB(string ISBN)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM BookTBL WHERE ISBN = @ISBN AND IsDeleted = 0"; // Adding IsDeleted condition
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ISBN", ISBN);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool BookExistsById(int BookId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM BookTBL WHERE BookId = @BookId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", BookId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        //public bool GetBookBorrowStatusById(int BookId)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        var query = "SELECT COUNT(*) FROM BorrowingTBL WHERE BookId = @BookId AND IsDone = 0 AND IsDeleted = 0";
        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@BookId", BookId);
        //            int count = (int)command.ExecuteScalar();
        //            return count > 0;
        //        }
        //    }
        //}

        public bool Add(Book obj)
        {
            if (ExistsBookISB(obj.ISBN))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO BookTBL (ISBN,Title,AuthorId) VALUES (@ISBN,@Title,@AuthorId)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ISBN", obj.ISBN);
                    command.Parameters.AddWithValue("@Title", obj.Title);
                    command.Parameters.AddWithValue("@AuthorId", obj.AuthorId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool Update(Book obj)
        {
            if (!BookExistsById(obj.BookId ?? 0) || ExistsBookISB(obj.ISBN))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE BookTBL SET ISBN = @ISBN, Title=@Title, AuthorId=@AuthorId  WHERE BookId = @BookId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ISBN", obj.ISBN);
                    command.Parameters.AddWithValue("@Title", obj.Title);
                    command.Parameters.AddWithValue("@AuthorId", obj.AuthorId);
                    command.Parameters.AddWithValue("@BookId", obj.BookId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool Delete(Book obj)
        {
            if (!BookExistsById(obj.BookId ?? 0))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE BookTBL SET IsDeleted = @IsDeleted WHERE BookId = @BookId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsDeleted", 1);
                    command.Parameters.AddWithValue("@BookId", obj.BookId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public IEnumerable<Book> GetAll()
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM BookTBL WHERE IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new Book
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                AuthorId = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                            };
                            book.Author = AuthorRepository.GetById(book.AuthorId);
                            books.Add(book);
                        }
                    }
                }
            }

            return books;        
    }

        public Book GetById(int? id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM BookTBL WHERE BookId = @BookId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var newBook =  new Book
                            {
                                BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                AuthorId = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))

                            };
                            newBook.Author = AuthorRepository.GetById(newBook.AuthorId);
                            return newBook;
                        }
                        else
                        {
                            // If no user found with the specified UserId, return null or throw an exception as needed
                            return new Book();
                        }
                    }
                }
            }
        }


    }
}
