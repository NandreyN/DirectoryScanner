using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProxyService.Classes;
using ProxyService.Interfaces;

namespace Analyzer.Controllers
{
    [Route("api/[controller]")]
    public class ScannerController : Controller
    {
        private readonly HttpClient _httpClient;
        public ScannerController(HttpClient client)
        {
            _httpClient = client;
        }

        public class ScannerEntryData : ProxyService.Controllers.ProxyController.EntryProxyData
        {
            public string Token { get; set; }
        }

        [HttpPost("Scan")]
        public JsonResult Scan([FromBody]ScannerEntryData request)
        {
            if (request == null)
                return Json(new { IsSuccess = false, ErrorMessage = "Invalid request parameters"});
            if (!request.List.Any())
                return Json(new { IsSuccess = false, ErrorMessage = "Nothing to scan" });

            IFolderScanner scanner = new LocalFolderScanner();
            IEnumerable<IFile> files =  scanner.GetFiles(new LocalFolder(request.List.First()));
            

            return Json(null) ;
        }
    }
}
