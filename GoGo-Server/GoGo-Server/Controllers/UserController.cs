using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GoGo_Server.Models;

namespace GoGo_Server.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public AppDb Db { get; }
        public UserController(AppDb db) {
            Db = db;
        }
        // POST api/User
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]User body) {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }
        // PUT api/User/1
        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]User body) {
            await Db.Connection.OpenAsync();
            var query = new User(Db);
            var result = await query.FindOne(id);
            if (result == null)
                return new NotFoundResult();
            result.FirstName = body.FirstName;
            result.MiddleName = body.MiddleName;
            result.SeccondName = body.SeccondName;
            result.Age = body.Age;
            result.Password = body.Password;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }


    }
}
