using Javademy.Data;
using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Javademy.Pages
{
    public partial class CourseEditPage : ContentPage
    {
        private int CourseId { get; set; }
        private Course CurrentCourse { get; set; }

        public CourseEditPage(int courseId)
        {
            InitializeComponent();
            CourseId = courseId;
            LoadCourseDetailsAsync();
        }

        private async void LoadCourseDetailsAsync()
        {
            try
            {
                var courses = await CoursesManager.GetAllCourses();
                CurrentCourse = courses.FirstOrDefault(c => c.CourseId == CourseId);

                if (CurrentCourse != null)
                {
                    CourseIdLabel.Text = $"Course ID: {CurrentCourse.CourseId}";
                    CourseImageNameEntry.Text = CurrentCourse.ImageName;
                    CourseDescriptionEditor.Text = CurrentCourse.Description;
                    CourseDurationEntry.Text = CurrentCourse.Duration?.ToString();
                    CourseCategoryIdEntry.Text = CurrentCourse.CategoryId?.ToString();
                }
                else
                {
                    await DisplayAlert("Error", "Course not found!", "OK");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load course details: {ex.Message}", "OK");
            }
        }

        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            try
            {
                CurrentCourse.ImageName = CourseImageNameEntry.Text;
                CurrentCourse.Description = CourseDescriptionEditor.Text;
                CurrentCourse.Duration = int.TryParse(CourseDurationEntry.Text, out var duration) ? duration : null;
                CurrentCourse.CategoryId = int.TryParse(CourseCategoryIdEntry.Text, out var categoryId) ? categoryId : null;

                await CoursesManager.UpdateCourse(CurrentCourse);
                await DisplayAlert("Success", "Course updated successfully!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to update course: {ex.Message}", "OK");
            }
        }

        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            if (CurrentCourse != null)
            {
                CourseImageNameEntry.Text = CurrentCourse.ImageName;
                CourseDescriptionEditor.Text = CurrentCourse.Description;
                CourseDurationEntry.Text = CurrentCourse.Duration?.ToString();
                CourseCategoryIdEntry.Text = CurrentCourse.CategoryId?.ToString();
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the CourseReadPage
            await Shell.Current.GoToAsync("..");
        }
    }
}
