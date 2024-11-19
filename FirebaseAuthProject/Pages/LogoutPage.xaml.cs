using Firebase.Auth;
using Microsoft.Maui.Controls;

namespace FirebaseAuthProject.Pages
{
    public partial class LogoutPage : ContentPage
    {
        private readonly FirebaseAuthClient _authClient;

        public LogoutPage(FirebaseAuthClient authClient)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _authClient = authClient;
            UpdateUserInterface();
        }

        private void UpdateUserInterface()
        {
            try
            {
                var currentUser = _authClient.User;
                if (currentUser != null)
                {
                    WelcomeLabel.Text = $"Welcome, {currentUser.Info.FirstName}!";
                    LogoutButton.IsVisible = true;
                }
                else
                {
                    WelcomeLabel.Text = "Welcome to .NET MAUI!";
                    LogoutButton.IsVisible = false;
                    // Use Task.Run to avoid blocking the UI thread
                    Task.Run(async () => await NavigateToLogin());
                }
            }
            catch (Exception)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Error", "Unable to get user information", "OK");
                });
            }
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            try
            {
                LogoutButton.IsEnabled = false;

                // Sign out using the correct method
                _authClient.SignOut();

                WelcomeLabel.Text = "You have been logged out successfully.";
                await NavigateToLogin();
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Unable to sign out. Please try again.", "OK");
            }
            finally
            {
                LogoutButton.IsEnabled = true;
            }
        }

        private async Task NavigateToLogin()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PushAsync(new SignInPage());
                if (Navigation.NavigationStack.Count > 1)
                {
                    Navigation.RemovePage(this);
                }
            });
        }
    }
}