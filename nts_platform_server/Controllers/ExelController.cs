using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace nts_platform_server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExelController : ControllerBase
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
    }
}
