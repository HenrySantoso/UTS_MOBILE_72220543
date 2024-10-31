using Javademy.Data;
using Microsoft.Maui.Controls;

namespace Javademy.Pages
{
    public partial class CategoryCreatePage : ContentPage
    {
        public CategoryCreatePage()
        {
            InitializeComponent();
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            string categoryName = CategoryNameEntry.Text; 
            string categoryDescription = CategoryDescriptionEditor.Text; 

            try
            {
                var newCategory = await CategoriesManager.AddCategory(categoryName, categoryDescription);
                if (newCategory != null)
                {
                    // Optionally, display success message or refresh data
                    await DisplayAlert("Success", "Category created successfully!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }


        private void OnResetClicked(object sender, EventArgs e)
        {
            CategoryNameEntry.Text = string.Empty;
            CategoryDescriptionEditor.Text = string.Empty;
        }
    }
}
