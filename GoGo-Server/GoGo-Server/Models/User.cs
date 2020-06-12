using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
            cmd.CommandText = @"INSERT INTO `User` (`idUser`,`FirstName`, `MiddleName`, `SeccondName`, `Age`, `AdvancedUser`, `Password`) VALUES (@idUser, @FirstName, @MiddleName, @SeccondName, @Age, @AdvancedUser, @Password)";
            bindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            idUser = (int)cmd.LastInsertedId;
        }

        public void bindParams(MySqlCommand cmd) {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@idUser",
                DbType = DbType.Int32,
                Value = idUser
            });
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
                ParameterName = "@Password",
                DbType = DbType.String,
                Value = Password
            });
        }
    }
}
