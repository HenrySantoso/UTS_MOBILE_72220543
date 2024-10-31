using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javademy.Data;
using Microsoft.Maui.Controls;

namespace Javademy.Pages
{
    public partial class CourseReadPage : ContentPage
    {
        public CourseReadPage()
        {
            InitializeComponent();
            LoadCoursesAsync();
        }

        private async void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            await LoadCoursesAsync();
        }

        private async Task LoadCoursesAsync()
        {
            try
            {
                var courses = await CoursesManager.GetAllCourses();
                CourseCollectionView.ItemsSource = courses.ToList();
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., show an alert or log the error)
                await DisplayAlert("Error", "Failed to load courses: " + ex.Message, "OK");
            }
        }
    }
}
