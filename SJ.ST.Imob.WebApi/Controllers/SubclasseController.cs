using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SJ.ST.Imob.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SJ.ST.Imob.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SubclasseController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<Subclasse> Get()
        {
            return new List<Subclasse>();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Subclasse Get(int id)
        {
            return new Subclasse();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Subclasse value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Subclasse value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
