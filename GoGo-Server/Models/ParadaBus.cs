using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace GoGo_Server.Models
{
    public class ParadaBus
    {
        public int idRuta { get; set; }
        public int idParada { get; set; }
        public double longitud { get; set; }
        public double latitud { get; set; }
        internal AppDb Db { get; set; }
        internal ParadaBus(AppDb db) {
            Db = db;
        }
        public ParadaBus() { }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO paradas_buses(`idRuta`, `idParada`) VALUES (@idRuta, @idParada) ";
            bindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        //public async Task<ParadaBus> FindOne()
        //{
        //    using var cmd = Db.Connection.CreateCommand();
        //    cmd.CommandText = @"SELECT `idRuta`, `idParada` WHERE `idRuta` = @idRuta AND `idParada` = @idParada";
        //    bindParams(cmd);
        //    var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
        //    return result.Count > 0 ? result[0] : null;
        //}
        public async Task<List<ParadaBus>> FindRoute(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT r.`idRuta`, p.`idParada`, p.`latitud`, p.`longitud` FROM `paradas_buses` pb, `rutas` r, `paradas` p WHERE r.`idRuta` = @r.idRuta AND r.`idRuta` = pb.`idRuta` AND p.`idParada`  = pb.`idParada`;";
            bindRuta(cmd, id);
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result : null;
        }
        public async Task<List<ParadaBus>> ReadAllAsync(DbDataReader reader) {
            var paradas = new List<ParadaBus>();
            using (reader) {
                while (await reader.ReadAsync()) {
                    var parada = new ParadaBus(Db)
                    {
                        idRuta = reader.GetInt32(0),
                        idParada = reader.GetInt32(1),
                        latitud = reader.GetDouble(2),
                        longitud = reader.GetDouble(3)
                    };
                    paradas.Add(parada);
                }
                return paradas;
            }
        }
        public void bindParams(DbCommand cmd) {

            // Adds a new MySql Parameter
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@idRuta",
                DbType = DbType.Int32,
                Value = idRuta
            });

            // Adds a new MySql Parameter
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@idParada",
                DbType = DbType.Int32,
                Value = idParada
            });
        }
        public void bindRuta(DbCommand cmd, int id) {
            // Adds a new MySql Parameter
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@r.idRuta",
                DbType = DbType.Int32,
                Value = id
            });
        }
        //public void bindJoinParams(DbCommand cmd, int idRuta, int idParada) {
        //    // Adds a new MySql Parameter
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@r.idRuta",
        //        DbType = DbType.Int32,
        //        Value = idRuta
        //    });

        //    // Adds a new MySql Parameter
        //    cmd.Parameters.Add(new MySqlParameter
        //    {
        //        ParameterName = "@p.idParada",
        //        DbType = DbType.Int32,
        //        Value = idParada
        //    });
        //}
    }
}

