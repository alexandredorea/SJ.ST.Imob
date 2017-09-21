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
    public class ClasseController : Controller
    {
        private readonly IServiceBus<Classe> serviceBus;

        public ClasseController(IServiceBus<Classe> serviceBus, IRedisDataAgent redisDataAgent, IRepository<Classe> repository)
        {
            this.serviceBus = serviceBus;
            this.serviceBus.AddRedisDataAgent(redisDataAgent)
                           .AddRepository(repository);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Classe> Get()
        {
            return serviceBus.GetData();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Classe Get(string id)
        {
            return serviceBus.GetData(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Classe value)
        {
            serviceBus.PostData(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]Classe value)
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
