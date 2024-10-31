using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Javademy.Data
{
    public static class CategoriesManager
    {
        static readonly string BaseAddress = "https://actualbackendapp.azurewebsites.net";
        static readonly string Url = $"{BaseAddress}/api/v1/Categories";
        private static string authorizationKey;

        static HttpClient client;

        private static async Task<HttpClient> GetClient()
        {
            if (client != null)
                return client;

            client = new HttpClient();

            if (string.IsNullOrEmpty(authorizationKey))
            {
                authorizationKey = await client.GetStringAsync($"{Url}login");
                authorizationKey = JsonSerializer.Deserialize<string>(authorizationKey);
            }

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.ConnectionClose = true; // Close connection after re

            return client;
        }

        // PARTS CRUD operations are here (as in your code)

        // CATEGORY CRUD operations

        public static async Task<IEnumerable<Category>> GetAllCategories()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return new List<Category>();

            var client = await GetClient();
            string result = await client.GetStringAsync($"{Url}");

            return JsonSerializer.Deserialize<List<Category>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        public static async Task<Category> AddCategory(string name, string description)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            var category = new Category
            {
                Name = name,
                Description = description // Assuming you have a Description property
            };

            var msg = new HttpRequestMessage(HttpMethod.Post, Url); // Ensure Url points to the correct endpoint
            msg.Content = JsonContent.Create(category);

            var client = await GetClient();
            var response = await client.SendAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                var returnedJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Category>(returnedJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            }
            else
            {
                // Handle error response
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error saving category: {response.StatusCode} - {errorContent}");
            }
        }


        public static async Task UpdateCategory(Category category)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;

            HttpRequestMessage msg = new(HttpMethod.Put, $"{Url}/{category.CategoryId}");
            msg.Content = JsonContent.Create<Category>(category);
            var client = await GetClient();
            var response = await client.SendAsync(msg);
            response.EnsureSuccessStatusCode();
        }

        public static async Task DeleteCategory(int categoryId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;

            HttpRequestMessage msg = new(HttpMethod.Delete, $"{Url}/{categoryId}");
            var client = await GetClient();
            var response = await client.SendAsync(msg);
            response.EnsureSuccessStatusCode();
        }
    }
}
