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
using nts_platform_server.Services;

namespace nts_platform_server.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    public class ExelController : Controller
    {

        private readonly ILogger<ExelController> _logger;

        private readonly IProjectService _projectService;

        public ExelController(ILogger<ExelController> logger, IProjectService projectService)
        {
            _projectService = projectService;


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
        public async Task<IActionResult> DownloadProjectUserWeekExelAsync([FromBody] DownloadProjectUserWeekExelModel exelModel_UserProjectWeek)
        {
            //Проверка на принятие данных
            if(exelModel_UserProjectWeek == null)
            {
                return BadRequest(new { message = "Data model is empty!" });
            }

            //Найти данный проект и пользователя в базе
            var response = await _projectService.FindUserProjectWeek(exelModel_UserProjectWeek);

            if(response == null)
            {
                return BadRequest(new { message = "Don't find \"project user week\" in database!" });
            }


            //int year = exelModel_UserProjectWeek.YearWeek % 100;

           // string nameFile = "cw"+year+ exelModel_UserProjectWeek.NumberWeek + "_"+ response.Project.Code + "_" + response.User.FirstName + "_" + response.User.SecondName + "_Hour_report";
            //nameFile += ".xlsx";

            string template = "template_hours1.xlsx";
            Exel exel = new Exel();
            var bytes = exel.createExelHour(template, response.Weeks.FirstOrDefault(),response,false);

            return File(bytes.bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", bytes.fileName);

        }

        [Route("projects/user/week/all/zip")]
        [Authorize]
        [HttpPost("projects/user/week/all/zip")]
        public async Task<IActionResult> DownloadProjectUserAllWeekExelAsync([FromBody] DownloadProjectUserWeekExelModel exelModel_UserProjectWeek)
        {
            //Проверка на принятие данных
            if (exelModel_UserProjectWeek == null)
            {
                return BadRequest(new { message = "Data model is empty!" });
            }

            //Найти данный проект и пользователя в базе
            var response = await _projectService.FindUserProjectWeek(exelModel_UserProjectWeek);

            if (response == null)
            {
                return BadRequest(new { message = "Don't find \"project user week\" in database!" });
            }


            int year = exelModel_UserProjectWeek.YearWeek % 100;

            string nameFile = response.Project.Code + "_" + response.User.FirstName + "_" + response.User.SecondName + "_ALL_Hour_report";
            nameFile += ".zip";

            string template = "template_hours1.xlsx";
            Exel exel = new Exel();


            foreach (var week in response.Weeks)
            {
                exel.createExelHour(template,week,response,true);
            }

            var bytes = exel.CreateZipArchive();

            return File(bytes, "application/zip", nameFile);
        }

    }
}
