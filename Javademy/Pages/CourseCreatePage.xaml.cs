using Javademy.Data; // Ensure you have the correct namespace
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Javademy.Pages
{
    public partial class CourseCreatePage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public CourseCreatePage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                // Replace with your actual API URL
                var categories = await _httpClient.GetFromJsonAsync<List<Category>>("https://actualbackendapp.azurewebsites.net/api/v1/Categories");

                // Check if categories are retrieved
                if (categories != null)
                {
                    CourseCategoryPicker.ItemsSource = categories.Select(c => c.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show an error message)
                await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            string courseName = CourseNameEntry.Text;
            string courseDescription = CourseDescriptionEditor.Text;
            string courseDuration = CourseDurationEntry.Text;
            string courseCategory = CourseCategoryPicker.SelectedItem?.ToString(); // Get the selected category

            // Add your logic to save the course using the provided values

            // Optionally show a success message
            await DisplayAlert("Success", "Course created successfully!", "OK");
        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            // Clear all entries and reset the picker
            CourseNameEntry.Text = string.Empty;
            CourseDescriptionEditor.Text = string.Empty;
            CourseDurationEntry.Text = string.Empty;
            CourseCategoryPicker.SelectedItem = null;
        }
    }
}
