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
    public class StatusController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<Status> Get()
        {
            return new List<Status>();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Status Get(int id)
        {
            return new Status();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Status value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Status value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
