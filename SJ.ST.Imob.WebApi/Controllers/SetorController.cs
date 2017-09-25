using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SJ.ST.Imob.Core;
using EasyNetQ;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SJ.ST.Imob.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SetorController : Controller
    {
        private readonly IServiceBus<Setor> serviceBus;
        

        public SetorController(IServiceBus<Setor> serviceBus, IRedisDataAgent redisDataAgent)
        {
            this.serviceBus = serviceBus;
            this.serviceBus.AddRedisDataAgent(redisDataAgent);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Setor> Get()
        {
            return serviceBus.GetData(this.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString())); // new Setor[] { new Setor() };
        }

        // GET api/values/k23i34u3yh4p51e
        [HttpGet("{id}")]
        public Setor Get(string id)
        {
            return serviceBus.GetData(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Setor value)
        {
            serviceBus.PostData(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]Setor value)
        {
            serviceBus.PutData(id, value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            serviceBus.DeleteData(id);
        }
    }
}
