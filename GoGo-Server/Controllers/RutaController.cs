using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoGo_Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoGo_Server.Controllers
{
    [Route("api/[controller]")]
    public class RutaController : ControllerBase
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
        // PUT api/Ruta/1
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
        // Get api/Ruta
        [HttpGet]
        public async Task<IActionResult> GetRutas() {
            await Db.Connection.OpenAsync();
            var query = new Ruta(Db);
            var result = await query.FindAllRutas();
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
        // GET api/Ruta
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetRuta(int id) {
            await Db.Connection.OpenAsync();
            var query = new Ruta(Db);
            var result = await query.FindOne(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
        // DELETE api/Ruta/1
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
