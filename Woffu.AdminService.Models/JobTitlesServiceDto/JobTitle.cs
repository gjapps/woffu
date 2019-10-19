using System;
using Woffu.AdminService.Models.JobTitlesServiceDto;

namespace Woffu.AdminService.Models.JobTitlesServiceDto
{
    /// <summary>
    /// Job titles model
    /// </summary>
    public class JobTitle: JobTitleBase
    {
        /// <summary>
        /// Job title id
        /// </summary>
        public int JobTitleId { get; set; }

        /// <summary>
        /// Company id
        /// </summary>
        public int CompanyId { get; set; }
    }
}
