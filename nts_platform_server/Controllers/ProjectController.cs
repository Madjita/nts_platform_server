using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using nts_platform_server.Auth.JWT;
using nts_platform_server.Entities;
using nts_platform_server.Models;
using nts_platform_server.Services;

namespace nts_platform_server.Controllers
{
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly IProjectService _projectService;

        private readonly IDocHourseService _docHourService;

        public ProjectController(IUserService userService, ICompanyService companyService, IProjectService projectService, IDocHourseService docHourService)
        {
            _userService = userService;
            _companyService = companyService;
            _projectService = projectService;
            _docHourService = docHourService;
        }

        [Authorize]
        [HttpGet("projects")]
        public IActionResult GetAllProjects()
        {
            var response = _projectService.GetAll();

            if (response == null)
            {
                return BadRequest(new { message = "Project don't Find!" });
            }

            return Ok(response);
        }


        [Authorize]
        [HttpPost("projects")]
        public async Task<IActionResult> PostAddProjectAsync(ProjectModel newProject)
        {
            if (newProject != null)
            {
                if (newProject.NameProject == null)
                {
                    return BadRequest(new { message = "Project allrady added!" });
                }
            }

            var response = await _projectService.AddAsync(newProject);

            if (response == null)
            {
                return BadRequest(new { message = "Project allrady added!" });
            }


            return Ok(response);
        }

        [Authorize]
        [HttpPost("projects/find")]
        public async Task<IActionResult> PostFindProjectAsync(ProjectModel newProject)
        {
            if (newProject == null)
            {

                return BadRequest(new { message = "Project model is empty!" });
            }

            var response = await _projectService.Find(newProject.Code);

            if (response == null)
            {
                return BadRequest(new { message = "Project allrady added!" });
            }


            return Ok(response);
        }

        [Authorize]
        [HttpDelete("projects")]
        public async Task<IActionResult> DeleteProjectAsync(ProjectModel newProject)
        {
            var response = await _projectService.RemoveCodeAsync(newProject.Code);

            if (response == null)
            {
                return BadRequest(new { message = "Company don't deleted!" });
            }

            return Ok(response);
        }


        [Authorize]
        [HttpPut("projects")]
        public async Task<IActionResult> EditProjectAsync([FromBody] ProjectEditModel newEditProject)
        {
            var response = await _projectService.EditCodeAsync(newEditProject);

            if (response == null)
            {
                return BadRequest(new { message = "Company don't edit!" });
            }

            return Ok(response);
        }

        [Route("projects/user")]
        [Authorize]
        [HttpPost("projects/user")]
        public async Task<IActionResult> PostAddUserCompanyAsync([FromBody] UserProjectModelList newUserProjectList)
        {
            if (newUserProjectList != null)
            {
                if (newUserProjectList.UserProjects.Count < 1)
                {
                    return BadRequest(new { message = "Company allrady added!" });
                }
            }

            var response = await _projectService.AddUserProjectAsync(newUserProjectList);

            if (response == null)
            {
                return BadRequest(new { message = "Company allrady added!" });
            }
            

            return Ok(_projectService.GetAll());
        }

        [Route("projects/usersInProject")]
        [Authorize]
        [HttpPost("projects/usersInProject")]
        public async Task<IActionResult> PostFindUsersInProjectCompanyAsync(UserProjectModel newUserProjectList)
        {
            if (newUserProjectList == null)
            {
                return BadRequest(new { message = "Company allrady added!" });
            }

            var response = await _userService.FindUsersInProjectAsync(newUserProjectList.Project);

            if (response == null)
            {
                return BadRequest(new { message = "Company allrady added!" });
            }


            return Ok(response);
        }



        [Route("projects/user/hours")]
        [Authorize]
        [HttpPost("projects/user/hours")]
        public async Task<IActionResult> PostAddUserHoursAsync([FromBody] UserProjectModelHours newUserProject)
        {
            if (newUserProject == null)
            {
                return BadRequest(new { message = "New user project is empty!" });
            }

            var response = await _projectService.AddUserProjectHoursAsync(newUserProject);

            if (response == null)
            {
                return BadRequest(new { message = "Project hours allrady added!" });
            }


            return Ok(response);
        }

    }
}
