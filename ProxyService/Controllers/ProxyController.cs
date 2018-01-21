﻿using System;
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
        private IList<PoolItem> _busyServices;
        private PoolItemContext _poolContext;
        private readonly ILogger _logger;

        public ProxyController(IConfiguration config, PoolItemContext context, ILogger log)
        {
            _poolContext = context;
            _logger = log;

            _poolAddresses = new Queue<PoolItem>();
            _busyServices = new List<PoolItem>();

            foreach (var item in _poolContext.PoolItems)
            {
                if (item.IsBusy)
                    _busyServices.Add(item);
                else
                    _poolAddresses.Enqueue(item);
            }
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
            ///<summary>
            ///An entry point of the request life-cycle.
            ///There should be logic of splitting requested folders between services in the pool
            ///Generate token for each request and register it in Task DataBase
            ///</summary>
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
