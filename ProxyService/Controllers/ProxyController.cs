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
using System.Data.SqlClient;
using System.Data;
using System.Net;
using Newtonsoft.Json;
using Hangfire;
using FluentScheduler;

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
            public override string ToString()
            {
                return JsonConvert.SerializeObject(List);
            }
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
            (string token, bool isSuccess) = await _tokenProvider.RegisterTokenAsync(value.ToString());
            if (!isSuccess)
                throw new Exception("Unsuccessful token creation");

            IEnumerable<string> requestedFolders = value.List;
            string sqlCreateString = $"CREATE TABLE [{token}] ([Folder] TEXT NOT NULL, CONSTRAINT[PK_Folder] PRIMARY KEY([Folder]))";
            bool creationResult = await SqliteUtilities.ExecuteCommandAsync(_connectionString, sqlCreateString);
            if (!creationResult)
                return StatusCode(500);

            /*SqliteCommand comm = new SqliteCommand("SELECT * FROM sqlite_master WHERE type = 'table'", connection);
            SqliteDataReader reader = comm.ExecuteReader();

            // Step through each row
            while (reader.Read())
            {
                string name = reader[1].ToString();
            }*/
            LocalFolderScanner scanner = new LocalFolderScanner(((List<string>)requestedFolders).ConvertAll(x => new LocalFolder(x)));
            scanner.CreateFolderStructure();
            bool writeResult = await scanner.WriteToTableAsync(_connectionString, token);

            IRecoveryManager recoveryManager = new SqliteRecoveryManager(_taskContext);
            // Launch background job here

            var dist = new BackgroundDistributor();

            return writeResult && recoveryManager.SetProperty(PropertySelector.FolderStructureCreated, token, true) ?
                Ok() : StatusCode(500);
        }

        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
