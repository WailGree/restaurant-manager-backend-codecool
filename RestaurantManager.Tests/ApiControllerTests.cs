﻿using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EFDataAccess.DataAccess;
using EFDataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using RestaurantManager.Web;
using Xunit;

namespace RestaurantManager.Tests
{
    public class ApiControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client { get; }
        private string _token;

        public ApiControllerTests()
        {
            var fixture = new WebApplicationFactory<Startup>();
            _client = fixture.CreateClient();
        }

        [Test]
        public async Task Test1_Registration_ValidCredential_ShouldReturnOk()
        {
            // Arrange
            string url = "api/registration";
            RegistrationCredentials registrationCred =
                new RegistrationCredentials("username", "password", "email@email.com");
            string output = JsonConvert.SerializeObject(registrationCred);
            var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(output,
                    Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(req);

            // Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }


        [Test]
        public async Task Test2_Login_ValidCredential_ShouldReturnToken()
        {
            // Arrange
            string url = "api/login";
            LoginCredential loginCredential =
                new LoginCredential("username", "password");
            string output = JsonConvert.SerializeObject(loginCredential);
            var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(output,
                    Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(req);

            _token = response.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.NotNull(_token);
        }

        [Test]
        public async Task Test3_Logout_ValidCredential_ShouldReturnOk()
        {
            // Arrange
            string url = "api/logout";
            AuthenticationCredential authenticationCredential =
                new AuthenticationCredential("username", _token);
            string output = JsonConvert.SerializeObject(authenticationCredential);
            var req = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(output,
                    Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(req);

            // Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public async Task Test4_Delete_ValidCredential_ShouldReturnOk()
        {
            // Arrange
            string url = "api/delete";
            LoginCredential loginCredential =
                new LoginCredential("username", "password");
            string output = JsonConvert.SerializeObject(loginCredential);
            var req = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = new StringContent(output,
                    Encoding.UTF8, "application/json")
            };

            // Act
            var response = await _client.SendAsync(req);

            // Assert
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
    }
}