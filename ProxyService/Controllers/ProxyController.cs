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
using System.Threading;
using Newtonsoft.Json;
using Hangfire;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProxyService.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class ProxyController : Controller
    {
        private readonly Queue<PoolItem> _poolAddresses;
        private readonly PoolItemContext _poolContext;
        private readonly TaskItemContext _taskContext;
        private readonly ITokenProvider _tokenProvider;
        private readonly FolderRecordContext _folderContext;
        private readonly IHostedService _backgroundDistributor;

        public class EntryProxyData
        {
            public enum FolderType
            {
                Local,
                GoogleDrive,
                Dropbox
            }

            public IEnumerable<string> List { get; set; }
            public FolderType Type { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(List);
            }
        }

        public ProxyController(IServiceProvider provider, IConfiguration config, PoolItemContext context, TaskItemContext taskContext, FolderRecordContext folderContext)
        {
            //_backgroundDistributor = provider.CreateScope().ServiceProvider.GetRequiredService<BackgroundDistributor>();
            _poolContext = context;
            _taskContext = taskContext;
            _tokenProvider = new JWTTokenProvider(_taskContext, config);
            _folderContext = folderContext;
            _poolAddresses = new Queue<PoolItem>();
            foreach (var item in _poolContext.PoolItems)
                _poolAddresses.Enqueue(item);
        }

        ///<summary>
        ///An entry point of the request life-cycle.
        ///There should be logic of splitting requested folders between services in the pool
        ///Generates token for each request and registers it in Task DataBase
        ///</summary>
        [HttpPost("Entry")]
        public async Task<IActionResult> EntryAsync([FromBody]EntryProxyData value)
        {
            (string token, bool isSuccess) = await _tokenProvider.RegisterTokenAsync(value.ToString());
            if (!isSuccess)
                throw new Exception("Unsuccessful token creation");

            IEnumerable<string> requestedFolders = value.List;

            LocalFolderScanner scanner = new LocalFolderScanner(((List<string>)requestedFolders).ConvertAll(x => new LocalFolder(x)));
            scanner.CreateFolderStructure();
            bool writeResult = await scanner.WriteToTableAsync(_folderContext, token);
            if (!writeResult)
                return StatusCode(500, new { message = "Unhandled exception during processing. Try again later." });

            IRecoveryManager recoveryManager = new SqliteRecoveryManager(_taskContext);
            //JobManager.AddJob(new BackgroundDistributor(_poolContext, _folderContext), s => s.ToRunNow());
            //await _backgroundDistributor.StartAsync(new CancellationToken(canceled: false));

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
