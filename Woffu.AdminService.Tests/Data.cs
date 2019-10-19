using Woffu.AdminService.Models.JobTitlesService;

namespace Woffu.AdminService.Tests
{
    public static class Data
    {
        public static JobTitle CreateJobTitle(int id)
        {
            return new JobTitle
            {
                Color = "color",
                CompanyId = id,
                JobTitleId = id,
                JobTitleKey = "jotTitleKey" + id,
                Name = "name" + id
            };
        }
    }
}
