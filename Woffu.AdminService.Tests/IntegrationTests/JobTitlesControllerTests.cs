using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Woffu.AdminService.Api.Controllers;
using Woffu.AdminService.Api.Interfaces;
using Woffu.AdminService.Models.JobTitlesService;
using Woffu.AdminService.Tests.Utils;
using Woffu.AdminService.Api;
using Xunit;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace Woffu.AdminService.Tests.UnitTests
{ 
    public class JobTitlesControllerTests
    {
        AutoMock mockProvider;

        public TestServer server { get; private set; }
        public HttpClient client { get; private set; }
        public JobTitlesControllerTests() {
            mockProvider = AutoMock.GetLoose();
            server = new TestServer(new WebHostBuilder().UseStartup<StartupTest>().ConfigureServices((services) => {
                var clientMock = mockProvider.Mock<IJobTitlesServiceClient>();
                services.AddSingleton(clientMock.Object);
            }));

            client = server.CreateClient();
        }

        public static IEnumerable<object[]> GetJobTitlesTestData =>
            new List<object[]>
            {
            new object[] {HttpStatusCode.OK,new List<JobTitle> { Data.CreateJobTitle(1), Data.CreateJobTitle(2), Data.CreateJobTitle(3) } },
            new object[] {HttpStatusCode.NotFound,null } 
        };

        [Theory]
        [MemberData(nameof(GetJobTitlesTestData))]
        public async Task UserLogged_GetJobTitles_JobTitlesReceived(HttpStatusCode expectedStatus, IEnumerable<JobTitle> clientResult)
        {
            //Arrange
            mockProvider.Mock<IJobTitlesServiceClient>().Setup(c => c.GetJobTitles()).ReturnsAsync(clientResult);
         
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Constants.API_BASE_URL}{Constants.CURRENT_VERSION}/{Constants.JOB_TITLE_RESOURCE}");
            request.Headers.Add(Constants.AUTHORIZATION, $"{Constants.BASIC} user:pass");

            //Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(expectedStatus, response.StatusCode);
         }

        public static IEnumerable<object[]> GetJobTitleTestData =>
            new List<object[]>
            {
                    new object[] {1,HttpStatusCode.OK, Data.CreateJobTitle(1)},
                    new object[] {2,HttpStatusCode.NotFound,null }
        };

        [Theory]
        [MemberData(nameof(GetJobTitleTestData))]
        public async Task UserLogged_GetJobTitle_JobTitleReceived(int titleId,HttpStatusCode expectedStatus, JobTitle clientResult)
        {            
            //Arrange
            mockProvider.Mock<IJobTitlesServiceClient>().Setup(c => c.GetJobTitle(titleId)).ReturnsAsync(clientResult);
          
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Constants.API_BASE_URL}{Constants.CURRENT_VERSION}/{Constants.JOB_TITLE_RESOURCE}{titleId}");
            request.Headers.Add(Constants.AUTHORIZATION, $"{Constants.BASIC} user:pass");
            
            //Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact()]
        public async Task UserLogged_CreateUnexistantJobTitle_JobTitleCreated()
        {
            //Arrange
            var jobTitle = Data.CreateJobTitle(1);
            mockProvider.Mock<IJobTitlesServiceClient>().Setup(c => c.CreateJobTitle(jobTitle)).ReturnsAsync(jobTitle);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{Constants.API_BASE_URL}{Constants.CURRENT_VERSION}/{Constants.JOB_TITLE_RESOURCE}");
            request.Headers.Add(Constants.AUTHORIZATION, $"{Constants.BASIC} user:pass");
            request.Content = new StringContent(JsonConvert.SerializeObject(jobTitle), Encoding.UTF8, "application/json");

            //Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact()]
        public async Task UserLogged_UpdateExistentJobTitle_JobTitleUpdated()
        {   
            //Arrange
            var jobTitle = Data.CreateJobTitle(1);
            mockProvider.Mock<IJobTitlesServiceClient>().Setup(c => c.UpdateJobTitle(jobTitle.JobTitleId, It.IsAny<JobTitle>())).ReturnsAsync(jobTitle);

            var request = new HttpRequestMessage(HttpMethod.Put, $"{Constants.API_BASE_URL}{Constants.CURRENT_VERSION}/{Constants.JOB_TITLE_RESOURCE}{jobTitle.JobTitleId}");
            request.Headers.Add(Constants.AUTHORIZATION, $"{Constants.BASIC} user:pass");
            request.Content = new StringContent(JsonConvert.SerializeObject(jobTitle), Encoding.UTF8, "application/json");

            //Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact()]
        public async Task UserLogged_DeleteExistentJobTitle_JobTitleDeleted()
        {  
            //Arrange
            var jobTitle = Data.CreateJobTitle(1);
            mockProvider.Mock<IJobTitlesServiceClient>().Setup(c => c.DeleteJobTitle(1)).Returns(Task.CompletedTask);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Constants.API_BASE_URL}{Constants.CURRENT_VERSION}/{Constants.JOB_TITLE_RESOURCE}{jobTitle.JobTitleId}");
            request.Headers.Add(Constants.AUTHORIZATION, $"{Constants.BASIC} user:pass");
            request.Content = new StringContent(JsonConvert.SerializeObject(jobTitle), Encoding.UTF8, "application/json");

            //Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
