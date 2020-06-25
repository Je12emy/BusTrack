using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace GoGo_Server.Models
{
    public class Ruta
    {
        public int idRuta { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        internal AppDb Db { get; set; }
        internal Ruta(AppDb db) {
            Db = db;
        }
        public Ruta() { 
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `rutas`(`nombre`, `descripcion`) VALUES (@nombre, @descripcion);";
            bindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            idRuta = (int)cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `rutas` SET `nombre`= @nombre, `descripcion` = @descripcion WHERE `idRuta` = @idRuta;";
            bindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            idRuta = (int)cmd.LastInsertedId;
        }

        public async Task<Ruta> FindOne(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `nombre`, `descripcion` FROM `paradas` WHERE `idRuta` = @idRuta;";
            bindId(cmd, id);
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task DeleteOneAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `rutas` WHERE `idRuta` = @idParada;";
            bindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<List<Ruta>> ReadAllAsync(DbDataReader reader) {
            var rutas = new List<Ruta>();
            using (reader)
            {
                while (await reader.ReadAsync()) {
                    var ruta = new Ruta(Db)
                    {
                        idRuta = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        descripcion = reader.GetString(2)
                    };
                    rutas.Add(ruta);
                }
                return rutas;
            }
        }
        public void bindId(MySqlCommand cmd, int id = 0) {
            if (id == 0)
            {
                // Adds a new MySql Parameter
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@idRuta",
                    DbType = DbType.Int32,
                    Value = idRuta
                });
            }
            else
            {
                // Adds a new MySql Parameter
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@idRuta",
                    DbType = DbType.Int32,
                    Value = id
                });
            }
        }
        public void bindParams(MySqlCommand cmd) {
            // Adds a new MySql Parameter
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@nombre",
                DbType = DbType.String,
                Value = nombre
            });
            // Adds a new MySql Parameter
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@descripcion",
                DbType = DbType.String,
                Value = descripcion
            });
        }
    }
}
