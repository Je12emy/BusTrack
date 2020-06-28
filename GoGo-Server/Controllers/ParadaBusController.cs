using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoGo_Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoGo_Server.Controllers
{
    [Route("api/[controller]")]
    public class ParadaBusController : ControllerBase
    {
        public AppDb Db { get; }
        public ParadaBusController(AppDb db) {
            Db = db;
        }
        // POST api/ParadaBus
        [HttpPost]
        public async Task<IActionResult> CreateParadaBus([FromBody]ParadaBus body) {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult("idRuta: " + body.idRuta + '\r' + "idParada: "+body.idParada);
        }
        // GET api/ParadaBus/1
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetParadasRuta(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new ParadaBus(Db);
            var result = await query.FindRoute(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
    }
}
