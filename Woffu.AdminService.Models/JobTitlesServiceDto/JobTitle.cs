using System;

namespace Woffu.AdminService.Models.JobTitlesService
{
    /// <summary>
    /// Job titles model
    /// </summary>
    public class JobTitle
    {
        /// <summary>
        /// Job title id
        /// </summary>
        public int JobTitleId { get; set; }

        /// <summary>
        /// Job title name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Company id
        /// </summary>
        public int CompanyId { get; set; }
    }
}
