using System;
using System.Collections.Generic;
using System.Linq; // Import for LINQ
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Javademy.Data;

namespace Javademy.Pages
{
    public partial class CourseReadPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private List<Course> allCourses; // Store all courses

        public CourseReadPage()
        {
            InitializeComponent();
            LoadCourses();
        }

        private async void LoadCourses()
        {
            try
            {
                //allCourses = CoursesManager.GetAllCourses();
                allCourses = await _httpClient.GetFromJsonAsync<List<Course>>("https://actualbackendapp.azurewebsites.net/api/Courses");
                CourseCollectionView.ItemsSource = allCourses; // Display all courses initially
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
            }
        }

        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            // Get the category ID from the button's CommandParameter
            var button = (Button)sender;
            int courseId = (int)button.CommandParameter;

            // Navigate to CategoryEditPage and pass the category ID
            await Navigation.PushAsync(new CourseEditPage(courseId));
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is int courseId)
            {
                bool confirmDelete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this course?", "Yes", "No");
                if (confirmDelete)
                {
                    try
                    {
                        var response = await _httpClient.DeleteAsync($"https://actualbackendapp.azurewebsites.net/api/Courses/{courseId}");
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Success", "Course deleted successfully!", "OK");
                            LoadCourses(); // Refresh the list
                        }
                        else
                        {
                            await DisplayAlert("Error", "Failed to delete course.", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Failed to delete course: {ex.Message}", "OK");
                    }
                }
            }
        }

        private void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            LoadCourses();
        }

        //private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        //    // Filter courses based on search input
        //    var filteredCourses = string.IsNullOrWhiteSpace(searchText)
        //        ? allCourses // Show all if search text is empty
        //        : allCourses.Where(c => c.Name.ToLower().Contains(searchText)).ToList();

        //    // Update the CollectionView with filtered results
        //    CourseCollectionView.ItemsSource = filteredCourses;
        //}

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue?.Trim();

            // If search text is empty, reset to show all courses
            if (string.IsNullOrWhiteSpace(searchText))
            {
                CourseCollectionView.ItemsSource = allCourses; // Assuming `allCourses` contains the full course list
                return;
            }

            try
            {
                // Call GetCourseByName to fetch the course matching the search text
                var course = await CoursesManager.GetCourseByName(searchText);
                if (course != null)
                {
                    // Update the CollectionView with the single course result
                    CourseCollectionView.ItemsSource = new List<Course> { course };
                }
                else
                {
                    // If no course is found, clear the CollectionView
                    CourseCollectionView.ItemsSource = new List<Course>();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Could not retrieve course: {ex.Message}", "OK");
            }
        }
    }
}
