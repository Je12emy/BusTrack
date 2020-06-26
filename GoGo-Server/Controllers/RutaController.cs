using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoGo_Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoGo_Server.Controllers
{
    [Route("api/[controller]")]
    public class RutaController : Controller
    {
        public AppDb Db { get; }
        public RutaController(AppDb db) {
            Db = db;
        }
        // POST api/Ruta
        [HttpPost]
        public async Task<IActionResult> CreateParada([FromBody]Ruta body) {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }
        // PUT api/Parada/1
        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateParada(int id, [FromBody]Ruta body)
        {
            await Db.Connection.OpenAsync();
            var query = new Ruta(Db);
            var result = await query.FindOne(id);
            if (result is null)
                return new NotFoundResult();
            result.nombre = body.nombre;
            result.descripcion = body.descripcion;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }
        // GET api/Parada/1
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetRuta(int id) {
            await Db.Connection.OpenAsync();
            var query = new Ruta(Db);
            var result = await query.FindOne(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
        // DELETE api/Parada/1
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteOne(int id) {
            await Db.Connection.OpenAsync();
            var query = new Ruta(Db);
            var result = await query.FindOne(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteOneAsync();
            return new OkResult();
        }

    }
}
