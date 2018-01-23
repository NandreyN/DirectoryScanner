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
using Microsoft.Data.Sqlite;
using System.Transactions;
using ProxyService.Interfaces;

namespace ProxyService.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ProxyController : Controller
    {
        private Queue<PoolItem> _poolAddresses;
        private PoolItemContext _poolContext;
        private TaskItemContext _taskContext;
        private ITokenProvider _tokenProvider;
        private readonly string _connectionString;

        public sealed class EntryProxyData
        {
            public IEnumerable<string> List { get; set; }
        }


        public ProxyController(IConfiguration config, PoolItemContext context, TaskItemContext taskContext)
        {
            _poolContext = context;
            _taskContext = taskContext;
            _tokenProvider = new JWTTokenProvider(_taskContext, config);
            _connectionString = config.GetConnectionString("Default");
            _poolAddresses = new Queue<PoolItem>();
            foreach (var item in _poolContext.PoolItems)
                _poolAddresses.Enqueue(item);
        }

        [HttpPost("Entry")]
        public async Task<IActionResult> EntryAsync([FromBody]EntryProxyData value)
        {
            ///<summary>
            ///An entry point of the request life-cycle.
            ///There should be logic of splitting requested folders between services in the pool
            ///Generate token for each request and register it in Task DataBase
            ///</summary>
            (string token, bool isSuccess) = await _tokenProvider.RegisterTokenAsync();
            if (!isSuccess)
                throw new Exception("Unsuccessful token creation");

            IEnumerable<string> requestedFolders = value.List;
            string sqlCreateString = $"CREATE TABLE [{token}] ([Token] TEXT NOT NULL, CONSTRAINT[PK_Token] PRIMARY KEY([Token]))";
            using (var connection = new SqliteConnection(_connectionString))
            { 
                await connection.OpenAsync();
                SqliteCommand command = new SqliteCommand(sqlCreateString, connection);
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
            return Ok();
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
