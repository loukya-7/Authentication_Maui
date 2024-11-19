using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FirebaseAuthProject.Pages
{
    public partial class ResetPasswordPage : ContentPage
    {
        private const string FirebaseApiKey = "AIzaSyDxWFcn-FjBg_BCZEh1T_jf_8eehNAaSsE"; // Replace with your Firebase API key
        private readonly FirebaseAuthClient _authClient;

        public ResetPasswordPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            // Initialize FirebaseAuthClient with configuration
            var config = new FirebaseAuthConfig
            {
                ApiKey = FirebaseApiKey,  // Replace with your Firebase API key
                AuthDomain = "loginmaui-28396.firebaseapp.com", // Replace with your Firebase Project ID
                Providers = new FirebaseAuthProvider[] { new EmailProvider() }
            };

            _authClient = new FirebaseAuthClient(config);
        }

        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            // Reset error messages
            EmailError.IsVisible = false;

            // Validate email input
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || !IsValidEmail(EmailEntry.Text))
            {
                EmailError.Text = "Please enter a valid email address.";
                EmailError.IsVisible = true;
                return;
            }

            // Send password reset email using Firebase's REST API
            try
            {
                await SendPasswordResetEmailAsync(EmailEntry.Text);
                await DisplayAlert("Success", "Password reset link has been sent to your email.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Password reset failed: {ex.Message}", "OK");
            }
        }

        private async Task SendPasswordResetEmailAsync(string email)
        {
            var resetUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={FirebaseApiKey}";

            using var httpClient = new HttpClient();
            var content = new StringContent(
                JsonSerializer.Serialize(new { requestType = "PASSWORD_RESET", email = email }),
                Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(resetUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error: {error}");
            }
        }

        private void OnGoToSignInClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignInPage());
        }

        private bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && new System.Text.RegularExpressions.Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(email);
        }
    }
}
