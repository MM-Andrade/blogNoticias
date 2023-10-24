using BlogPetNews.API.Domain.Enums;
using BlogPetNews.API.Domain.UseCases.LoginUser;
using BlogPetNews.API.Infra.Utils;
using BlogPetNews.API.Service.ViewModels.News;
using BlogPetNews.Integration.Tests.Util;
using BlogPetNews.Tests.Common.Factory;
using BlogPetNews.Tests.Common.News;
using BlogPetNews.Tests.Common.Users;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BlogPetNews.Integration.Tests.Get
{
    public class GetNewsTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {

        private readonly CustomWebApplicationFactory<Program> _application;
        private HttpClient _httpClient;
        private readonly ICryptography _cryptography;

        public GetNewsTest(CustomWebApplicationFactory<Program> application)
        {
            _application = application;
            _httpClient = application.CreateClient();
            _cryptography = new Cryptography();
            _application.AddServiceFake(IntegrationTestHelpers.UserServiceFake());
        }
        [Fact]
        public async Task Get_News_ShouldReturnAllNews()
        {
                    
        }

        [Fact]
        public async Task Get_News_ShouldReturnNewsById()
        {
            //Arrange

            _httpClient = await LoginUser(true);
            
             var news = await CreateNews(_httpClient);

            //Act
            var response = await _httpClient.GetAsync($"/news/{news.Id}");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ReadNewsDto>(content);

            //Asserts
            Assert.NotNull(result!.Title);
            Assert.Equal(news.Id, result!.Id);
            IntegrationTestHelpers.AssertStatusCodeOk(response);
        }




        private async Task<HttpClient> LoginUser(bool userIsAdmin)
        {
            //Arrange
            var user = UserTestFixture.UserFaker.Generate();
            var tempPassword = user.Password;
            user.Password = _cryptography.Encodes(user.Password);

            if (userIsAdmin)
                user.Role = RolesUser.Admin;
            else
                user.Role = RolesUser.User;

            await IntegrationTestsMockData.Createuser(_application, user);
            var httpResponse = await _httpClient.PostAsync($"/login?email={user.Email}&password={tempPassword}", null);

            var content = await httpResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<LoginUserCommandResponse>(content);

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + result!.Token);

            return _httpClient;
        }



        private async Task<ReadNewsDto> CreateNews(HttpClient client)
        {
            var news = NewsTestFixture.CreateNewsDtoFaker.Generate();
            var response = await _httpClient.PostAsJsonAsync("/news", news);
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ReadNewsDto>(content);
            
            return result!;

        }
    }
}