using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nts_platform_server.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nts_platform_server.Controllers
{
    [Route("[controller]")]
    public class WeekController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly IProjectService _projectService;

        private readonly IDocHourseService _docHourService;

        public WeekController(IUserService userService, ICompanyService companyService, IProjectService projectService, IDocHourseService docHourService)
        {
            _userService = userService;
            _companyService = companyService;
            _projectService = projectService;
            _docHourService = docHourService;
        }


        [Authorize]
        [HttpGet("week")]
        public IActionResult GetAllWeeks()
        {
            var response = _docHourService.GetAll();

            if (response == null)
            {
                return BadRequest(new { message = "Project don't Find!" });
            }

            return Ok(response);
        }
    }
}

