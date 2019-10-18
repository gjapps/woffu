using System;
using System.Collections.Generic;
using System.Text;

namespace Woffu.AdminService.Models.WebClientDto
{
    /// <summary>
    /// Job titles model
    /// </summary>
    public class JobTitle
    {
        /// <summary>
        /// Job title id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Job title name
        /// </summary>
        public string Name { get; set; }
    }
}
