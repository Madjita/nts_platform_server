using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nts_platform_server.Auth.JWT;
using nts_platform_server.Entities;
using nts_platform_server.Models;
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
        [HttpPost("allChecks/add")]
        public async Task<IActionResult> addNewCheck(
            [FromForm] IFormFile File_checkBankPhotoByte,
            [FromForm] IFormFile File_ticketPhotoByte,
            [FromForm] IFormFile File_borderTicketPhotoByte,
            [FromForm] IFormFile File_billPhotoByte,
            [FromForm] string jsonString)
        {

            ReportCheckNewModel reportCheckNewModel = JsonConvert.DeserializeObject<ReportCheckNewModel>(jsonString);

            if(File_checkBankPhotoByte == null && File_ticketPhotoByte == null && File_borderTicketPhotoByte == null && File_billPhotoByte == null)
            {
                return BadRequest(new { message = "All files are empty." });
            }



            var response = await _reportCheck.AddNewCheck(File_checkBankPhotoByte, File_ticketPhotoByte, File_borderTicketPhotoByte, File_billPhotoByte, reportCheckNewModel);

            if (response == null)
            {
                return BadRequest(new { message = "Table Check is empty !" });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPut("allChecks/add")]
        public async Task<IActionResult> EditCheck(
            [FromForm] IFormFile File_checkBankPhotoByte,
            [FromForm] IFormFile File_ticketPhotoByte,
            [FromForm] IFormFile File_borderTicketPhotoByte,
            [FromForm] IFormFile File_billPhotoByte,
            [FromForm] string jsonString)
        {

            ReportCheckEditModel reportCheckEditModel = JsonConvert.DeserializeObject<ReportCheckEditModel>(jsonString);

           /* if (File_checkBankPhotoByte == null && File_ticketPhotoByte == null && File_borderTicketPhotoByte == null && File_billPhotoByte == null)
            {
                return BadRequest(new { message = "All files are empty." });
            }*/

            var response = await _reportCheck.EditCheck(File_checkBankPhotoByte, File_ticketPhotoByte, File_borderTicketPhotoByte, File_billPhotoByte, reportCheckEditModel);

            if (response == null)
            {
                return BadRequest(new { message = "Table Check is empty !" });
            }

            return Ok(response);
        }



        [Authorize]
        [HttpDelete("checks")] // ReportCheck/checks
        public async Task<IActionResult> DeleteCheckAsync([FromBody] ReportCheckNewModel reportCheckNewModel)
        {
            var responseDelete = await _reportCheck.DeleteCheckAsync(reportCheckNewModel);

            if (responseDelete)
            {
                //var response = await _reportCheck.GetAllAsync();

                return Ok(responseDelete);
            }

            return BadRequest(new { message = "Don't find delete item check!" });
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

        [Authorize]
        [HttpGet("BusinessTrips")] // ReportCheck/BusinessTrips
        public async Task<IActionResult> GetBusinessTrip()
        {
            var response = await _reportCheck.GetAllBusinessTripAsync();

            return Ok(response);
        }

        //[Authorize]
        [HttpPost("BusinessTrips/selected")]
        public async Task<IActionResult> GetBusinessTripSelectedChecks([FromBody] BusinessTripModel businessTripModel)
        {
            var response = await _reportCheck.GetSelectedChecksAsync(businessTripModel);


            return Ok(response);
        }


        [Authorize]
        [HttpPost("BusinessTrips/finish")] // ReportCheck/BusinessTrips
        public async Task<IActionResult> FinishBusinessTrip([FromBody] BusinessTripModel businessTripModel)
        {
            var response = await _reportCheck.FinishBusinessTripAsync(businessTripModel);

            return Ok(response);
        }


        [Authorize]
        [HttpPost("BusinessTrips")] // ReportCheck/BusinessTrips
        public async Task<IActionResult> AddBusinessTripAsync([FromBody] BusinessTripModel businessTripModel)
        {
             var response = await _reportCheck.AddBusinessTripAsync(businessTripModel);

             response = await _reportCheck.GetAllBusinessTripAsync();

            return Ok(response);
        }

        [Authorize]
        [HttpPut("BusinessTrips")]
        public async Task<IActionResult> EditBusinessTripAsync([FromBody] BusinessTripEditModel businessTripModel)
        {
            var response = await _reportCheck.EditBusinessTripAsync(businessTripModel);

            response = await _reportCheck.GetAllBusinessTripAsync();

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("BusinessTrips")] // ReportCheck/BusinessTrips
        public async Task<IActionResult> DeleteBusinessTripAsync([FromBody] BusinessTripModel businessTripModel)
        {
            var responseDelete = await _reportCheck.DeleteBusinessTripAsync(businessTripModel);

            if(responseDelete)
            {
               // var response = await _reportCheck.GetAllBusinessTripAsync();

                return Ok(responseDelete);
            }

            return BadRequest(new { message = "Don't find delete item!" });
        }


    }
}

