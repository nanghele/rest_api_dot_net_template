using Microsoft.AspNetCore.Mvc.Testing;
using MyAPI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestIntegration;
using Xunit;

namespace MyAPI.Tests
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<MyAPI.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<MyAPI.Startup> _factory;

        public IntegrationTests(
            CustomWebApplicationFactory<MyAPI.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("api/todo")]
        public async Task GetItemsEndpointsReturnSuccessAndCorrectContentType(string url)
        {

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            var stringResponse = await response.Content.ReadAsStringAsync();
            var toDoList = JsonConvert.DeserializeObject<IEnumerable<TodoItem>>(stringResponse);
            Assert.Contains(toDoList, p => p.Name == "Item2" && p.IsComplete == false);
        }

        [Fact]
        public async Task InsertANewItemAndCheckResponseAndHeader()
        {
            TodoItem newItem = new TodoItem { Name = "Item5", IsComplete = false};
            // Act
            var response = await _client.PostAsJsonAsync<TodoItem>("api/todo", newItem);
            // Assert
            

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            response.Headers.Location.AbsolutePath.Equals("/api/ToDo/5");
            await CheckItemExistsAsync(newItem, 5);
        }

        [Fact]
        public async Task InsertANewItemWithIdReturnBadRequest()
        {
            TodoItem newItem = new TodoItem { Name = "Item5", IsComplete = false, Id = 2 };
            // Act
            var response = await _client.PostAsJsonAsync<TodoItem>("api/todo", newItem);
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        private async Task CheckItemExistsAsync(TodoItem item, long id)
        {
            // Act
            var response = await _client.GetAsync("api/todo/" + id);
            // Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var returnedItem = JsonConvert.DeserializeObject<TodoItem>(stringResponse);
            Assert.Equal(returnedItem.Name, item.Name);
            Assert.Equal(returnedItem.IsComplete, item.IsComplete);
        }

        [Fact]
        public async Task Delete_DeleteAnItemAndverifyResponse()
        {          
            // Act
            var response = await _client.DeleteAsync("api/todo/1");
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            async Task act() => await CheckItemExistsAsync(new TodoItem { Name = "Item1", IsComplete = false }, 1);
           await Assert.ThrowsAsync<HttpRequestException>(act);
        }


        [Fact]
        public async Task Put_UpdateNewItemAndCheckResponseAndHeader()
        {
            TodoItem newItem = new TodoItem { Name = "Item3bis", IsComplete = false, Id=3 };
            // Act
            var response = await _client.PutAsJsonAsync<TodoItem>("api/todo/3", newItem);
            // Assert

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            await CheckItemExistsAsync(newItem, 3);
        }
    }
}