using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Javademy.Data
{
    public static class CoursesManager
    {
        static readonly string BaseAddress = "https://actualbackendapp.azurewebsites.net"; // Update with the actual base address
        static readonly string Url = $"{BaseAddress}/api/Courses"; // Update with the actual endpoint for courses
        private static string authorizationKey;

        static HttpClient client;

        private static async Task<HttpClient> GetClient()
        {
            if (client != null)
                return client;

            client = new HttpClient();

            // Get authorization key if needed
            if (string.IsNullOrEmpty(authorizationKey))
            {
                authorizationKey = await client.GetStringAsync($"{BaseAddress}/api/v1/login"); // Update with actual login endpoint
                authorizationKey = JsonSerializer.Deserialize<string>(authorizationKey);
            }

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.ConnectionClose = true; // Close connection after response

            return client;
        }

        // CRUD operations for Courses

        public static async Task<List<Course>> GetAllCourses()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return new List<Course>();

            var client = await GetClient();
            return await client.GetFromJsonAsync<List<Course>>("https://actualbackendapp.azurewebsites.net/api/Courses");
        }

        public static async Task<Course> AddCourse(string name, string imageName, int? duration, string description, int? categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Course name is required.");

            var course = new Course
            {
                Name = name,
                ImageName = imageName,
                Duration = duration,
                Description = description,
                CategoryId = categoryId
            };

            var msg = new HttpRequestMessage(HttpMethod.Post, Url);
            msg.Content = JsonContent.Create(course);

            var client = await GetClient();
            var response = await client.SendAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                var returnedJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Course>(returnedJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error saving course: {response.StatusCode} - {errorContent}");
            }
        }


        public static async Task UpdateCourse(Course course)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;

            HttpRequestMessage msg = new(HttpMethod.Put, $"{Url}/{course.CourseId}");
            msg.Content = JsonContent.Create<Course>(course);
            var client = await GetClient();
            var response = await client.SendAsync(msg);
            response.EnsureSuccessStatusCode();
        }

        public static async Task DeleteCourse(int courseId)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return;

            HttpRequestMessage msg = new(HttpMethod.Delete, $"{Url}/{courseId}");
            var client = await GetClient();
            var response = await client.SendAsync(msg);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<Course> GetCourseByName(string name)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return null;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Course name is required.");

            var client = await GetClient();
            var response = await client.GetAsync($"{Url}/search/{name}");

            if (response.IsSuccessStatusCode)
            {
                var returnedJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Course>(returnedJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error retrieving course: {response.StatusCode} - {errorContent}");
            }
        }
    }
}
