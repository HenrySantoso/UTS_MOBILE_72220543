using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Javademy.Data;
using System.Net.Http.Json;

namespace Javademy.Pages
{
    public partial class CategoryReadPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public CategoryReadPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                var categories = await _httpClient.GetFromJsonAsync<List<Category>>("https://actualbackendapp.azurewebsites.net/api/v1/Categories");
                CategoryCollectionView.ItemsSource = categories;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is int categoryId)
            {
                bool confirmDelete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this category?", "Yes", "No");
                if (confirmDelete)
                {
                    try
                    {
                        var response = await _httpClient.DeleteAsync($"https://actualbackendapp.azurewebsites.net/api/v1/Categories/{categoryId}");
                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Success", "Category deleted successfully!", "OK");
                            LoadCategories(); // Refresh the list
                        }
                        else
                        {
                            await DisplayAlert("Error", "Failed to delete category.", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Failed to delete category: {ex.Message}", "OK");
                    }
                }
            }
        }

        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            // Get the category ID from the button's CommandParameter
            var button = (Button)sender;
            int categoryId = (int)button.CommandParameter;

            // Navigate to CategoryEditPage and pass the category ID
            await Navigation.PushAsync(new CategoryEditPage(categoryId));
        }

        private void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            LoadCategories();
        }
    }
}
