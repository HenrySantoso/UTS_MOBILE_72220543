using System;
using Microsoft.Maui.Controls;

namespace Javademy.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            // TODO: Add authentication logic here

            // Navigate to the Home page using the defined route
            await Shell.Current.GoToAsync("//Home");
        }

    }
}
