using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Woffu.AdminService.Models.JobTitlesServiceDto;

namespace Woffu.AdminService.Api.Interfaces
{
    /// <summary>
    /// Web client used to communicate with Job Titles Service
    /// </summary>
    public interface IJobTitlesServiceClient
    {
        /// <summary>
        /// Authorization header
        /// </summary>
        public string Authorization { get; set; }

        /// <summary>
        /// Get job title list
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<JobTitle>> GetJobTitles();

        /// <summary>
        /// Get job title
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<JobTitle> GetJobTitle(int id);

        /// <summary>
        /// Create job title
        /// </summary>
        /// <param name="jobtitle"></param>
        /// <returns></returns>
        Task CreateJobTitle(JobTitleBase jobtitle);

        /// <summary>
        /// Update job title
        /// </summary>
        /// <param name="jobtitle"></param>
        /// <returns></returns>
        Task UpdateJobTitle(JobTitle jobtitle);

        /// <summary>
        /// Delete job title
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<JobTitle> DeleteJobTitle(int id);
    }
}
