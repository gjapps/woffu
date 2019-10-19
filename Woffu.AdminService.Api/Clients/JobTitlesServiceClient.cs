using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Woffu.AdminService.Api.Interfaces;
using Woffu.AdminService.Models.JobTitlesServiceDto;

namespace Woffu.AdminService.Api.Clients
{
    public class JobTitlesServiceClient : IJobTitlesServiceClient
    {
        HttpClient httpClient;

        public JobTitlesServiceClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        string authorization;
        public string Authorization {
            get { return authorization; }
            set {
               authorization= value.Replace($"{Constants.BASIC} ", string.Empty);
               var bytes= Encoding.ASCII.GetBytes(authorization);
               authorization =  Convert.ToBase64String(bytes);
            }
        }

        public Task CreateJobTitle(JobTitleBase jobtitle)
        {
            throw new NotImplementedException();
        }

        public Task<JobTitle> DeleteJobTitle(int id)
        {
            throw new NotImplementedException();
        }

        public Task<JobTitle> GetJobTitle(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<JobTitle>> GetJobTitles()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            
            request.Headers.Authorization = new AuthenticationHeaderValue(Constants.BASIC,authorization);
            request.Method = HttpMethod.Get; 
            
            var response=await httpClient.SendAsync(request);

            VerifySupportedStatus(response.StatusCode);

            IEnumerable<JobTitle> ret = null;
            if (response.Content != null)
            {
                ret = JsonConvert.DeserializeObject<IEnumerable<JobTitle>>(await response.Content.ReadAsStringAsync());
            }
            
            return ret;
        }

        public Task UpdateJobTitle(JobTitle jobtitle)
        {
            throw new NotImplementedException();
        }

        bool VerifySupportedStatus(HttpStatusCode code)
        {
            if (code != HttpStatusCode.OK && code != HttpStatusCode.NotFound) {
                throw new HttpRequestException (code.ToString());
            }
            
            return true;
        }
    }
}
