using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Javademy.Data;

namespace Javademy.Pages
{
    public partial class CategoryEditPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private int _currentCategoryId;

        public CategoryEditPage()
        {
            InitializeComponent();
        }

        public CategoryEditPage(int categoryId)
        {
            InitializeComponent();
            _currentCategoryId = categoryId; // Set the category ID
            LoadCategoryDetails(_currentCategoryId); // Load category details
        }

        private async Task LoadCategoryDetails(int categoryId)
        {
            try
            {
                var category = await _httpClient.GetFromJsonAsync<Category>($"https://actualbackendapp.azurewebsites.net/api/v1/Categories/{categoryId}");
                if (category != null)
                {
                    // Set the CategoryIdLabel text
                    CategoryIdLabel.Text = $"Category ID: {category.CategoryId}";
                    CategoryNameEntry.Text = category.Name;
                    CategoryDescriptionEditor.Text = category.Description;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load category details: {ex.Message}", "OK");
            }
        }

        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var updatedCategory = new Category
                {
                    CategoryId = _currentCategoryId,
                    Name = CategoryNameEntry.Text,
                    Description = CategoryDescriptionEditor.Text
                };

                var response = await _httpClient.PutAsJsonAsync($"https://actualbackendapp.azurewebsites.net/api/v1/Categories/{_currentCategoryId}", updatedCategory);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Category updated successfully!", "OK");
                    await Navigation.PopAsync(); // Go back to CategoryReadPage
                }
                else
                {
                    await DisplayAlert("Error", "Failed to update category.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to update category: {ex.Message}", "OK");
            }
        }
        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            // Reset the fields to the selected category details
            LoadCategoryDetails(_currentCategoryId);
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the CategoryReadPage
            await Shell.Current.GoToAsync("..");
        }
    }
}
