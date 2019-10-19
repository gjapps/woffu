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
        /// Company id
        /// </summary>
        public int CompanyId { get; set; }
     
        /// <summary>
        /// JotTitleKey
        /// </summary>
        public string JobTitleKey { get; set; }
        
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

       /// <summary>
       /// Color
       /// </summary>
        public string Color { get; set; }
    }
}
