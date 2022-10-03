using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nts_platform_server.Services;

namespace nts_platform_server.Controllers
{
    [Route("[controller]")]
    public class ReportCheckController : Controller
    {
        private readonly IReportCheckService _reportCheck;

        public ReportCheckController(IReportCheckService reportCheck)
        {
            _reportCheck = reportCheck;
        }


        [Authorize]
        [HttpGet("allChecks")]
        public async Task<IActionResult> GetAllChecks()
        {
            var response = await _reportCheck.GetAllAsync();

            if (response == null)
            {
                return BadRequest(new { message = "Table Check is empty !" });
            }

            return Ok(response);
        }


        [Authorize]
        [HttpGet("PlaneChecks")]
        public async Task<IActionResult> GetChecks_Plane()
        {
            var response = await _reportCheck.GetCheck_PlaneAsync();

            if (response == null)
            {
                return BadRequest(new { message = "Plane Checks are empty !" });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("TrainChecks")]
        public async Task<IActionResult> GetChecks_Train()
        {
            var response = await _reportCheck.GetCheck_TrainAsync();

            if (response == null)
            {
                return BadRequest(new { message = "Train Checks are empty!" });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("HostelChecks")]
        public async Task<IActionResult> GetChecks_Hostel()
        {
            var response = await _reportCheck.GetCheck_HostelAsync();

            if (response == null)
            {
                return BadRequest(new { message = "Hostel Checks are empty!" });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("ShopChecks")]
        public async Task<IActionResult> GetChecks_Shop()
        {
            var response = await _reportCheck.GetCheck_ShopAsync();

            if (response == null)
            {
                return BadRequest(new { message = "Shop Checks are empty!" });
            }

            return Ok(response);
        }



        //Code for get all BusinessTrip

       // [Authorize]
        [HttpGet("BusinessTrips")] // ReportCheck/BusinessTrips
        public async Task<IActionResult> GetBusinessTrip()
        {
            var response = await _reportCheck.GetAllBusinessTripAsync();

            return Ok(response);
        }

    }
}

