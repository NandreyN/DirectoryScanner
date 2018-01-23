using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections;
using ProxyService.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ProxyService.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ProxyController : Controller
    {
        private Queue<PoolItem> _poolAddresses;
        private PoolItemContext _poolContext;

        public sealed class EntryProxyData
        {
            public IEnumerable<string> List { get; set; }
        }


        public ProxyController(IConfiguration config, PoolItemContext context)
        {
            _poolContext = context;
            _poolAddresses = new Queue<PoolItem>();
            foreach (var item in _poolContext.PoolItems)
                _poolAddresses.Enqueue(item);
        }

        [HttpPost("Entry")]
        public IActionResult Entry([FromBody]EntryProxyData value)
        {
            ///<summary>
            ///An entry point of the request life-cycle.
            ///There should be logic of splitting requested folders between services in the pool
            ///Generate token for each request and register it in Task DataBase
            ///</summary>

            IEnumerable<string> requestedFolders = value.List;
            return Ok();
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
