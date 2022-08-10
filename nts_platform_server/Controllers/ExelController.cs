using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using nts_platform_server.Models;
using nts_platform_server.Auth.JWT;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Net.Http;
using System.Net;

namespace nts_platform_server.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    public class ExelController : Controller
    {

        private readonly ILogger<ExelController> _logger;

        public ExelController(ILogger<ExelController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Exel([FromForm] IFormFile file)
        {
            

            if(file != null)
            {
                Exel exel = new Exel(file);

                var list1 = exel.GetRowHPM(2);
                exel.CreateExel();

                var list2 = exel.GetRowHPM(3);
                exel.CreateExel();

                var list3 = exel.GetRowHPM(4);
                exel.CreateExel();
                return Ok();
            }
                
            return NotFound();
        }


   

        [Route("projects/user/week/exel")]
        [Authorize]
        [HttpPost("projects/user/week/exel")]
        public IActionResult DownloadPRojectUserWeekExel([FromBody] DownloadProjectUserWeekExelModel exelModel)
        {
            string nameFile = exelModel.UserEmail + "_" + exelModel.ProjectCode + "_" + exelModel.YearWeek + "_" + exelModel.NumberWeek;
            nameFile += ".xlsx";
            Exel exel = new Exel(nameFile);
            return File(exel.bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nameFile);
        }
    }
}
