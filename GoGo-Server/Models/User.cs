using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GoGo_Server.Models
{
    public class User
    {
        public int idUser { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SeccondName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password {get; set;}
        public bool AdvancedUser { get; set; }

        internal AppDb Db { get; set; }
        internal User(AppDb db) {
            Db = db;
        }
        public User() { 
        }
        public async Task InsertAsync() {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `user` (`FirstName`, `MiddleName`, `SeccondName`, `Age`, `AdvancedUser`, `Email`, `Password`) VALUES (@FirstName, @MiddleName, @SeccondName, @Age, @AdvancedUser, @Email, @Password);";
            bindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            idUser = (int)cmd.LastInsertedId;
        }
        public async Task UpdateAsync() {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `user` SET `FirstName` = @FirstName, `MiddleName` = @MiddleName, `SeccondName` = @SeccondName, `Age` = @Age, `AdvancedUser` = @AdvancedUser, `Email` = @Email, `Password` = @Password WHERE `idUser` = @idUser;";
            bindParams(cmd);
            bindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<User> FindOne(int id) {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `idUser`,`FirstName`, `MiddleName`, `SeccondName`, `Age`, `AdvancedUser`, `Email`, `Password` FROM `user` WHERE `idUser` = @idUser;";
            bindId(cmd, id);
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;   
        }
        public async Task DeleteOneAsync() {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `user` WHERE `idUser` = @idUser";
            bindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<List<User>> ReadAllAsync(DbDataReader reader) {
            var users = new List<User>();
            using (reader)
            {
                while (await reader.ReadAsync()) {
                    var user = new User(Db)
                    {
                        idUser = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        MiddleName = reader.GetString(2),
                        SeccondName = reader.GetString(3),
                        Age = reader.GetInt32(4),
                        AdvancedUser = reader.GetBoolean(5),
                        Email = reader.GetString(6),
                        Password = reader.GetString(7)
                    };
                    users.Add(user);
                }
                return users;
            }
        }

        public void bindId(MySqlCommand cmd, int id = 0) {
            if (id == 0) {
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@idUser",
                    DbType = DbType.Int32,
                    Value = idUser
                });
            } else
            { 
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@idUser",
                    DbType = DbType.Int32,
                    Value = id
                });
            }
        }

        public void bindParams(MySqlCommand cmd) {
           
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@FirstName",
                DbType = DbType.String,
                Value = FirstName
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@MiddleName",
                DbType = DbType.String,
                Value = MiddleName
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@SeccondName",
                DbType = DbType.String,
                Value = SeccondName
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Age",
                DbType = DbType.Int32,
                Value = Age
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@AdvancedUser",
                DbType = DbType.Boolean,
                Value = AdvancedUser
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Email",
                DbType = DbType.String,
                Value = Email
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Password",
                DbType = DbType.String,
                Value = Password
            });
        }
    }
}
