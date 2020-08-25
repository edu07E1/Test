using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using DALib.BaseCode;
using TripsCore.Code;
using Newtonsoft.Json;


namespace TripsCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[RequestSizeLimit(100_000_000)]
    public class HomeController : Controller
    {
        /// <summary>
        /// Process file upload and generates report
        /// </summary>
        /// <returns>OK: list with report rows. Error: HTTP error code</returns>
        [HttpPost]
        public IEnumerable<DriversReport> Post()
        {
            try
            {
                //For increased security we do validations both in client and server
                //We should have only 1 file with .dat or .txt extension
                if (Request.Form.Files.Count > 0)
                {
                    DataFile Df = new DataFile(Request.Form.Files[0], 80);
                    if (Df.LoadFile(1) == DataFile.LFStatus.OK)     //1: .NET, 2: SQL Server (not implemented)
                    {
                        return Df.GetReport();
                    }
                }
                return null;
            }
            catch (Exception)
            {
                //return StatusCode(500, "Internal server error");    //Error 500
                return null;
            }
        }
    }
}
