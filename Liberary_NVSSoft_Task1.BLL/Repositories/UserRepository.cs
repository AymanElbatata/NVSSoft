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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository()
        {
            _connectionString = "server=(local);Database=Liberary_NVSSoft_Task1;Integrated Security=true";
        }


        public bool UserExistsUserName(string userName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM UserTBL WHERE UserName = @UserName AND IsDeleted = 0"; // Adding IsDeleted condition
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool UserExistsById(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM UserTBL WHERE UserId = @UserId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool Add(User obj)
        {
            if (UserExistsUserName(obj.UserName))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO UserTBL (UserName, UserPW) VALUES (@UserName, @UserPW)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", obj.UserName);
                    command.Parameters.AddWithValue("@UserPW", obj.UserPW);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool Update(User obj)
        {
            if (!UserExistsById(obj.UserId ?? 0))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE UserTBL SET UserName = @UserName, UserPW = @UserPW WHERE UserId = @UserId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", obj.UserName);
                    command.Parameters.AddWithValue("@UserPW", obj.UserPW);
                    command.Parameters.AddWithValue("@UserId", obj.UserId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool Delete(User obj)
        {
            if (!UserExistsById(obj.UserId ?? 0))
            {
                return false;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE UserTBL SET IsDeleted = @IsDeleted WHERE UserId = @UserId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsDeleted", 1);
                    command.Parameters.AddWithValue("@UserId", obj.UserId);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM UserTBL WHERE IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                UserPW = reader.GetString(reader.GetOrdinal("UserPW")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                            };
                            users.Add(user);
                        }
                    }
                }
            }

            return users;        
    }

        public User GetById(int? id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM UserTBL WHERE UserId = @UserId AND IsDeleted = 0";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                UserPW = reader.GetString(reader.GetOrdinal("UserPW")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                            };
                        }
                        else
                        {
                            // If no user found with the specified UserId, return null or throw an exception as needed
                            return new User();
                        }
                    }
                }
            }
        }



    }
}
