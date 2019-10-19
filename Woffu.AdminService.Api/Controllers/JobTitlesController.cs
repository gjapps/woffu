using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Woffu.AdminService.Api.Interfaces;
using Woffu.AdminService.Models.JobTitlesService;

namespace Woffu.AdminService.Api.Controllers
{
    /// <summary>
    /// Job title controller
    /// </summary>
    [ApiController]
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
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<JobTitle>> GetJobTitle(int jobTitleId)
        {
            this.jobTitlesServiceClient.Authorization = HttpContext.Request.Headers[Constants.AUTHORIZATION].ToString();
            var jobTitle = await jobTitlesServiceClient.GetJobTitle(jobTitleId);
            if (jobTitle is null)
            {
                return NotFound();
            }
            return Ok(jobTitle);
        }

        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<JobTitle>> CreateJobTitle([FromBody] JobTitle jobTitle)
        {
            this.jobTitlesServiceClient.Authorization = HttpContext.Request.Headers[Constants.AUTHORIZATION].ToString();
            var createdJobTitle= await jobTitlesServiceClient.CreateJobTitle(jobTitle);
            return Created("",createdJobTitle);
        }

        [HttpPut("{jobTitleId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<JobTitle>> UpdateJobTitle(int jobTitleId, [FromBody] JobTitle jobTitle)
        {
            this.jobTitlesServiceClient.Authorization = HttpContext.Request.Headers[Constants.AUTHORIZATION].ToString();
            var updatedJobTitle = await jobTitlesServiceClient.UpdateJobTitle(jobTitleId, jobTitle);
            return new OkObjectResult(updatedJobTitle);
        }

        [HttpDelete("{jobTitleId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteJobTitle(int jobTitleId)
        {
            this.jobTitlesServiceClient.Authorization = HttpContext.Request.Headers[Constants.AUTHORIZATION].ToString();
            await jobTitlesServiceClient.DeleteJobTitle(jobTitleId);
            return Ok();
        }
    }
}
