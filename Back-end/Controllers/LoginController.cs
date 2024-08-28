using Back_end.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MCFContext _context;
        public LoginController(MCFContext context)
        {
            _context = context;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<MsUser>> Get()
        {
            var login = await _context.MsUsers.ToListAsync();
            return login;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<JsonResult?> Post(MsUser value)
        {
            // Finding match | cari persis

            var data = await _context.MsUsers.ToListAsync();

            var match = data.FindAll(x => x.UserName == value.UserName && x.Password == value.Password);

            if (match.Count > 0)
            {
                var dataRaw = System.Text.Json.JsonSerializer.Serialize(data[0]);
                return System.Text.Json.JsonSerializer.Deserialize<JsonResult>(dataRaw);
            }
            else
            {
                return null;
            }

            
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
