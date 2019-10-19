using Autofac.Extras.Moq;
using Moq;
using Newtonsoft.Json;
using System;
using Woffu.AdminService.Api;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Woffu.AdminService.Api.Clients;
using Woffu.AdminService.Tests.Utils;
using Xunit;
using Woffu.AdminService.Models.JobTitlesServiceDto;
using System.Collections.Generic;

namespace Woffu.AdminService.Tests.UnitTests
{
    public class JobTitlesServicesClientTests
    {
        public static IEnumerable<object[]> JobTitleData =>
        new List<object[]>
        {
                    new object[] {HttpStatusCode.Created, Data.CreateJobTitle(1)},
                    new object[] {HttpStatusCode.NotFound,null },
                    new object[] {HttpStatusCode.InternalServerError,null },
                    new object[] {HttpStatusCode.Unauthorized,null }
        };

        [Theory()]
        [MemberData(nameof(JobTitleData))]
        public async Task UserLogged_CreateJob0Title_JobTitleCreatedResponseProcessed(HttpStatusCode expectedCode, JobTitle expectedResult) {

            using (var mockProvider = AutoMock.GetLoose())
            {
                //Arrange
                var client=ConfigureClient(expectedCode, expectedResult, mockProvider);

                //Act
                JobTitle result=null;
                try
                {
                    result = await client.CreateJobTitle(expectedResult);
                }
                catch(HttpRequestException e) {
                    Assert.Equal(expectedCode.ToString(),e.Message);
                }

                //Assert
                ValidateAuthorization(client, "key");

                if (expectedResult != null)
                {
                    Assert.Equal(JsonConvert.SerializeObject( expectedResult), JsonConvert.SerializeObject(result));
                }
            }
        }

        [Theory()]
        [MemberData(nameof(JobTitleData))]
        public async Task UserLogged_UpdateJobTitle_JobTitleUpdatedResponseProcessed(HttpStatusCode expectedCode, JobTitle expectedResult)
        {
            using (var mockProvider = AutoMock.GetLoose())
            {
                //Arrange
                var client = ConfigureClient(expectedCode, expectedResult, mockProvider);

                //Act
                JobTitle result = null;
                try
                {
                    result = await client.UpdateJobTitle(1,expectedResult);
                }
                catch (HttpRequestException e)
                {
                    Assert.Equal(expectedCode.ToString(), e.Message);
                }

                //Assert
                ValidateAuthorization(client, "key");

                if (expectedResult != null)
                {
                    Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
                }
            }
        }


        public static IEnumerable<object[]> JobTitlesData =>
        new List<object[]>
        {
                            new object[] {HttpStatusCode.OK, new List<JobTitle> { Data.CreateJobTitle(1), Data.CreateJobTitle(2) } },
                            new object[] {HttpStatusCode.NotFound,null},
                            new object[] {HttpStatusCode.InternalServerError,null },
                            new object[] {HttpStatusCode.InternalServerError,null }
        };

        [Theory()]
        [MemberData(nameof(JobTitlesData))]
        public async Task UserLogged_GetJobTitles_JobTitlesResponseProcessed(HttpStatusCode expectedCode, IEnumerable<JobTitle> expectedResult)
        {
            using (var mockProvider = AutoMock.GetLoose())
            {
                ///Arrange
                var client = ConfigureClient(expectedCode, expectedResult, mockProvider);

                //Act
                IEnumerable<JobTitle> result = null;
                try
                {
                    result = await client.GetJobTitles();
                }
                catch (HttpRequestException e)
                {
                    Assert.Equal(expectedCode.ToString(), e.Message);
                }

                //Assert
                ValidateAuthorization(client,"key");

                if (expectedResult != null)
                {
                    Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
                }
            }
        }

        [Theory()]
        [MemberData(nameof(JobTitleData))]
        public async Task UserLogged_GetJobTitle_JobTitleResponseProcessed(HttpStatusCode expectedCode, JobTitle expectedResult)
        {
            using (var mockProvider = AutoMock.GetLoose())
            {
                ///Arrange
                var client = ConfigureClient(expectedCode, expectedResult, mockProvider);

                //Act
                JobTitle result = null;
                try
                {
                    result = await client.GetJobTitle(1);
                }
                catch (HttpRequestException e)
                {
                    Assert.Equal(expectedCode.ToString(), e.Message);
                }

                //Assert
                ValidateAuthorization(client, "key");

                if (expectedResult != null)
                {
                    Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
                }
            }
        }

        [Theory()]
        [MemberData(nameof(JobTitleData))]
        public async Task UserLogged_DeleteJobTitle_JobTitleDeletedResponseProcessed(HttpStatusCode expectedCode, JobTitle expectedResult)
        {
            using (var mockProvider = AutoMock.GetLoose())
            {
                ///Arrange
                var client = ConfigureClient(expectedCode, expectedResult, mockProvider);

                //Act
                try
                {
                    await client.DeleteJobTitle(1);
                }
                catch (HttpRequestException e)
                {
                    Assert.Equal(expectedCode.ToString(), e.Message);
                }

                //Assert
                ValidateAuthorization(client, "key");
            }
        }

        JobTitlesServiceClient ConfigureClient(HttpStatusCode expectedCode, object expectedResult, AutoMock mockProvider)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = expectedCode;
            if (expectedResult != null)
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(expectedResult));
            }
            var messageHandler = new TestMessageHandler(response);

            var httpClient = new HttpClient(messageHandler);
            mockProvider.Provide(httpClient);
            var client = mockProvider.Create<JobTitlesServiceClient>();

            httpClient.BaseAddress = new Uri("http://www.test.com/");
            client.Authorization = $"{Constants.BASIC } key";

            return client;
        }

        void ValidateAuthorization(JobTitlesServiceClient client,string auth) {
            var bytes = Encoding.ASCII.GetBytes(auth);
            var authorization = Convert.ToBase64String(bytes);
            Assert.Equal(authorization, client.Authorization);
        }
    }
}
