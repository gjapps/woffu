using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Woffu.AdminService.Api.Interfaces;
using Woffu.AdminService.Models.WebClientDto;

namespace Woffu.AdminService.Api.Controllers
{
    /// <summary>
    /// Job title controller
    /// </summary>
    [ApiVersion(Constants.CURRENT_VERSION)]
    [Authorize]
    [Route(Constants.API_BASE_URL+ "{version:apiVersion}/" + Constants.JOB_TITLE_RESOURCE)]
    public class JobTitlesController : Controller
    {
        readonly IJobTitlesServiceClient jobTitlesServiceClient;

        public JobTitlesController(IJobTitlesServiceClient jobTitlesServiceClient) {
            this.jobTitlesServiceClient = jobTitlesServiceClient;
        }

        [HttpGet()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<JobTitle>>> GetJobTitles()
        {
            this.jobTitlesServiceClient.Authorization = HttpContext.Request.Headers[Constants.AUTHORIZATION].ToString();
            var jobTitles = await jobTitlesServiceClient.GetJobTitles();
            
            if (jobTitles is null) {
               return NotFound();
            }
            return Ok(jobTitles);
        }

        [HttpGet("{jobTitleId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<JobTitle>> GetJobTitle(int jobTitleId)
        {
            return Ok();
        }

        [HttpPost("{jobTitleId}")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateJobTitle(int jobTitleId)
        {
            return Ok();
        }

        [HttpPut("{jobTitleId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateJobTitle(int jobTitleId)
        {
            return Ok();
        }

        [HttpDelete("{jobTitleId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteJobTitle(int jobTitleId)
        {
            return Ok();
        }
    }
}
