using GoGo_Server.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace GoGo_Server.Controllers
{
    public class Parada
    {
        // Class properties
        public int idParada { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        internal AppDb Db { get; set; }
        // Class constructors
        internal Parada(AppDb db) {
            Db = db;
        }
        public Parada() { 
        }
        // Insert Parada
        public async Task InsertAsync() {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `paradas` (`nombre`,`descripcion`, `latitud`, `longitud`) VALUES (@nombre, @descripcion, @latitud, @longitud);";
            bindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            idParada = (int)cmd.LastInsertedId;
        }
        // Update Parada
        public async Task UpdateAsync() {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `paradas` SET `nombre` = @nombre, `descripcion` = @descripcion, `latitud` = @latitud, `longitud` = @longitud WHERE `idParada` = @idParada;";
            bindParams(cmd);
            bindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
        // FindOne Parada
        public async Task<Parada> FindOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `paradas` WHERE `idParada` = @idParada";
            bindId(cmd, id);
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }
        // Delete Parada

        public async Task DeleteOneAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `paradas` WHERE `idParada` = @idParada;";
            bindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
        // Read parameters from query and asing values into model
        public async Task<List<Parada>> ReadAllAsync(DbDataReader reader) {
            // Create a list
            var paradas = new List<Parada>();
            // Create a disposable block function
            using (reader)
            {
                // While there are values to read
                while (await reader.ReadAsync()) {
                    // Create a new instance of Parada and extract value from datatable
                    var parada = new Parada(Db)
                    {
                        idParada = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        descripcion = reader.GetString(2),
                        latitud = reader.GetDouble(3),
                        longitud = reader.GetDouble(4)
                    };
                    // Add the new Parada
                    paradas.Add(parada);
                }
                // Return the list of paradas
                return paradas;
            }
        }
        // BindId to sql command
        public void bindId(MySqlCommand cmd, int id = 0) {
            // If no id has been passed, the default value is 0
            if (id == 0)
            {
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@idUser",
                    DbType = DbType.Int32,
                    Value = idParada
                });
            }
            // A id has been passed, assing said value
            else {
                cmd.Parameters.Add(new MySqlParameter { 
                    ParameterName = "@idUser",
                    DbType = DbType.Int32,
                    Value = idParada
                });
            }
        }
        public void bindParams(MySqlCommand cmd) {
            cmd.Parameters.Add(new MySqlParameter { 
                ParameterName = "@nombre",
                DbType = DbType.String,
                Value = nombre
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@descripcion",
                DbType = DbType.String,
                Value = descripcion
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@latitud",
                DbType = DbType.Double,
                Value = latitud
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@longitud",
                DbType = DbType.Double,
                Value = longitud
            });
        }


        
        

    }
}
