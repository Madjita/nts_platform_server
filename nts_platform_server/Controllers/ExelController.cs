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
using nts_platform_server.Algorithms;
using System.Text;

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


            Translit translit = new Translit();

            string template = "template_hours1.xlsx";
            Exel exel = new Exel();
            var bytes = exel.createExelHour(template, response.Weeks.FirstOrDefault(),response,false);

            return File(bytes.bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", translit.TranslitFileName(bytes.fileName.ToLower()));

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


            StringBuilder nameFile = new StringBuilder(50);

            nameFile.Append(response.Project.Code);
            nameFile.Append("_");
            nameFile.Append(response.User.FirstName);
            nameFile.Append("_");
            nameFile.Append(response.User.SecondName);
            nameFile.Append("_ALL_Hour_report");
            nameFile.Append(".zip");


            //int year = exelModel_UserProjectWeek.YearWeek % 100;

           // string nameFile = response.Project.Code.ToString() + "_" + response.User.FirstName.ToString() + "_" + response.User.SecondName.ToString() + "_ALL_Hour_report";
           // nameFile += ".zip";

            string template = "template_hours1.xlsx";
            Exel exel = new Exel();


            foreach (var week in response.Weeks)
            {
                exel.createExelHour(template,week,response,true);
            }

            var bytes = exel.CreateZipArchive();

            Translit translit = new Translit();

            return File(bytes, "application/zip;charset=utf-8", translit.TranslitFileName(nameFile.ToString().ToLower()));
        }

    }
}
