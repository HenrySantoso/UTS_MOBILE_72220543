using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Javademy.Data;

namespace Javademy.Pages
{
    public partial class CourseReadPage : ContentPage
    {
        public CourseReadPage()
        {
            InitializeComponent();
            LoadCourses();
        }

        private async Task LoadCourses() // Change return type to Task
        {
            var courses = await CoursesManager.GetAllCourses();
            CourseCollectionView.ItemsSource = courses;
        }

        private async void OnRefreshButtonClicked(object sender, EventArgs e) // Keep it as void
        {
            await LoadCourses(); // Refresh the course list
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e) // Keep it as void
        {
            var button = (Button)sender;
            int courseId = (int)button.CommandParameter;

            bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this course?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    await CoursesManager.DeleteCourse(courseId);
                    await DisplayAlert("Success", "Course deleted successfully!", "OK");
                    await LoadCourses(); // Refresh the list after deletion
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete course: {ex.Message}", "OK");
                }
            }
        }
    }
}
