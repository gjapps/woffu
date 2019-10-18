using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Woffu.AdminService.Models.WebClientDto;

namespace Woffu.AdminService.Api.Controllers
{
    /// <summary>
    /// Job title controller
    /// </summary>
    [Route(Constants.API_BASE_URL + Constants.JOB_TITLE_RESOURCE)]
    public class JobTitlesController : Controller
    {
        
        [HttpGet()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<JobTitle>>> GetJobTitles()
        {
            var ret = new List<JobTitle>();
            return  Ok(ret);
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
