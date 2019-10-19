using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Woffu.AdminService.Api.Interfaces;
using Woffu.AdminService.Models.JobTitlesServiceDto;

namespace Woffu.AdminService.Api.Clients
{
    /// <summary>
    ///  Client used to communicate with JobTitlesService
    /// </summary>
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
        public async Task<IEnumerable<JobTitle>> GetJobTitles()
        {
            HttpRequestMessage request = new HttpRequestMessage();

            request.Headers.Authorization = new AuthenticationHeaderValue(Constants.BASIC, authorization);
            request.Method = HttpMethod.Get;

            var response = await httpClient.SendAsync(request);

            ValidateResponseStatus(response.StatusCode);

            IEnumerable<JobTitle> ret = null;
            if (response.Content != null)
            {
                ret = JsonConvert.DeserializeObject<IEnumerable<JobTitle>>(await response.Content.ReadAsStringAsync());
            }

            return ret;
        }

        public async Task<JobTitle> GetJobTitle(int jobTitleId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,jobTitleId.ToString());

            request.Headers.Authorization = new AuthenticationHeaderValue(Constants.BASIC, authorization);

            var response = await httpClient.SendAsync(request);

            ValidateResponseStatus(response.StatusCode);

            JobTitle ret = null;
            if (response.Content != null)
            {
                ret = JsonConvert.DeserializeObject<JobTitle>(await response.Content.ReadAsStringAsync());
            }

            return ret;
        }

        public async Task<JobTitle> CreateJobTitle(JobTitle jobTitle)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.Headers.Authorization = new AuthenticationHeaderValue(Constants.BASIC, authorization);
            request.Content = new StringContent(JsonConvert.SerializeObject(jobTitle), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            ValidateSuccessStatus(response);

            JobTitle ret = null;
            ret = JsonConvert.DeserializeObject<JobTitle>(await response.Content.ReadAsStringAsync());
            return ret;
        }

        public async Task<JobTitle> UpdateJobTitle(int jobTitleId, JobTitle jobTitle)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put,jobTitle.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue(Constants.BASIC, authorization);
            request.Content = new StringContent(JsonConvert.SerializeObject(jobTitle), Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(request);
            ValidateSuccessStatus(response);

            JobTitle ret = null;
            ret = JsonConvert.DeserializeObject<JobTitle>(await response.Content.ReadAsStringAsync());
            return ret;
        }

        public async Task DeleteJobTitle(int jobTitleId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, jobTitleId.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue(Constants.BASIC, authorization);

            var response = await httpClient.SendAsync(request);
            ValidateSuccessStatus(response);
        }

        void ValidateResponseStatus(HttpStatusCode code)
        {
            if  (code != HttpStatusCode.OK & code != HttpStatusCode.Created && code != HttpStatusCode.NotFound) {
                throw new HttpRequestException (code.ToString());
            }
        }

        void ValidateSuccessStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.StatusCode.ToString());
            }
        }
    }
}
