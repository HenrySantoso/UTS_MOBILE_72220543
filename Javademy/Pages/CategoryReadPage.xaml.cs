using Javademy.Data;

namespace Javademy.Pages;

public partial class CategoryReadPage : ContentPage
{
    public CategoryReadPage()
    {
        InitializeComponent();
        LoadCategories(); // Optionally load categories when the page initializes
    }

    private async Task LoadCategories()
    {
        var categories = await CategoriesManager.GetAllCategories();
        CategoryCollectionView.ItemsSource = categories;
    }

    private async void OnRefreshButtonClicked(object sender, EventArgs e)
    {
        await LoadCategories();
    }
}
