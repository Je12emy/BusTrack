using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoGo_Server.Models;

namespace GoGo_Server.Controllers
{
    [Route("api/[controller]")]
    public class ParadaController : ControllerBase
    {
        public AppDb Db { get; }
        public ParadaController(AppDb db) {
            Db = db;
        }
        // POST api/Parada
        [HttpPost]
        public async Task<IActionResult> CreateParada([FromBody]Parada body) {
            // Open a database connection
            await Db.Connection.OpenAsync();
            // Pass in the database instance
            body.Db = Db;
            // Call the Insert method async
            await body.InsertAsync();
            // Return 200 Ok with the created object
            return new OkObjectResult(body);
            // Look into populating the relationship table from here
        }
        // PUT api/Parada/1
        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateParada(int id, [FromBody]Parada body) {
            // Open a db connection
            await Db.Connection.OpenAsync();
            // Create a new parada
            var query = new Parada(Db);
            // Find the parada with the passed id and asign it to result
            var result = await query.FindOne(id);
            if (result is null)
                return new NotFoundResult();
            // Update the properties
            result.nombre = body.nombre;
            result.descripcion = body.descripcion;
            result.latitud = body.latitud;
            result.longitud = body.longitud;
            // Update the parada
            await result.UpdateAsync();
            // Return 200 ok with the updated object
            return new ObjectResult(result);
        }
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetParada(int id)
        {
            // Open database connection
            await Db.Connection.OpenAsync();
            // Create a new parada
            var query = new Parada(Db);
            // Find the parada and asign it
            var result = await query.FindOne(id);
            if (result is null)
                return new NotFoundResult();
            // Return 200 ok and the found object
            return new OkObjectResult(result);
        }
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteParada(int id) {
            // Open database connection
            await Db.Connection.OpenAsync();
            // Create a new parada
            var query = new Parada(Db);
            // Find one parada and asign it to a new value
            var result = await query.FindOne(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteOneAsync();
            return new OkResult();
        }
    }
}
