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
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly string _connectionString;

        public AuthorRepository()
        {
            _connectionString = "server=(local);Database=Liberary_NVSSoft_Task1;Integrated Security=true";
        }


        public bool ExistsAuthorName(string AuthorName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM AuthorTBL WHERE AuthorName = @AuthorName AND IsDeleted = 0"; // Adding IsDeleted condition
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AuthorName", AuthorName);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool AuthorExistsById(int AuthorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM AuthorTBL WHERE AuthorId = @AuthorId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AuthorId", AuthorId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool Add(Author obj)
        {
            if (ExistsAuthorName(obj.AuthorName))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO AuthorTBL (AuthorName) VALUES (@AuthorName)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AuthorName", obj.AuthorName);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool Update(Author obj)
        {
            if (!AuthorExistsById(obj.AuthorId ?? 0) || ExistsAuthorName(obj.AuthorName))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE AuthorTBL SET AuthorName = @AuthorName WHERE AuthorId = @AuthorId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AuthorName", obj.AuthorName);
                    command.Parameters.AddWithValue("@AuthorId", obj.AuthorId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool Delete(Author obj)
        {
            if (!AuthorExistsById(obj.AuthorId ?? 0))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE AuthorTBL SET IsDeleted = @IsDeleted WHERE AuthorId = @AuthorId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsDeleted", 1);
                    command.Parameters.AddWithValue("@AuthorId", obj.AuthorId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public IEnumerable<Author> GetAll()
        {
            var authors = new List<Author>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM AuthorTBL WHERE IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var author = new Author
                            {
                                AuthorId = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                AuthorName = reader.GetString(reader.GetOrdinal("AuthorName")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                            };
                            authors.Add(author);
                        }
                    }
                }
            }

            return authors;        
    }

        public Author GetById(int? id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM AuthorTBL WHERE AuthorId = @AuthorId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AuthorId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Author
                            {
                                AuthorId = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                AuthorName = reader.GetString(reader.GetOrdinal("AuthorName")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                            };
                        }
                        else
                        {
                            // If no user found with the specified UserId, return null or throw an exception as needed
                            return new Author();
                        }
                    }
                }
            }
        }



    }
}
