using Firebase.Auth;
using Firebase.Auth.Providers;
using System;
using System.Text.RegularExpressions;

namespace FirebaseAuthProject.Pages
{
    public partial class SignInPage : ContentPage
    {
        private readonly FirebaseAuthClient _authClient;

        public SignInPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyDxWFcn-FjBg_BCZEh1T_jf_8eehNAaSsE",
                AuthDomain = "loginmaui-28396.firebaseapp.com",
                Providers = new FirebaseAuthProvider[] { new EmailProvider() }
            };

            _authClient = new FirebaseAuthClient(config);
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            try
            {
                // Reset error states
                EmailError.Text = string.Empty;
                PasswordError.Text = string.Empty;
                EmailError.IsVisible = false;
                PasswordError.IsVisible = false;

                // Validate email
                if (string.IsNullOrWhiteSpace(EmailEntry.Text))
                {
                    EmailError.Text = "Please enter your email";
                    EmailError.IsVisible = true;
                    return;
                }

                if (!IsValidEmail(EmailEntry.Text))
                {
                    EmailError.Text = "Please enter a valid email";
                    EmailError.IsVisible = true;
                    return;
                }

                // Validate password
                if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
                {
                    PasswordError.Text = "Please enter your password";
                    PasswordError.IsVisible = true;
                    return;
                }

                // Show loading state
                SignInButton.IsEnabled = false;
                LoadingIndicator.IsVisible = true;

                // Attempt sign in
                var user = await _authClient.SignInWithEmailAndPasswordAsync(EmailEntry.Text, PasswordEntry.Text);

                // Success - navigate to main page
                await Navigation.PushAsync(new LogoutPage(_authClient));

            }
            catch (FirebaseAuthException ex)
            {
                HandleAuthError(ex);
            }
            finally
            {
                // Reset UI state
                SignInButton.IsEnabled = true;
                LoadingIndicator.IsVisible = false;
            }
        }

        private void HandleAuthError(FirebaseAuthException ex)
        {
            if (ex.Message.Contains("INVALID_EMAIL") || ex.Message.Contains("USER_NOT_FOUND"))
            {
                EmailError.Text = "No account found with this email";
                EmailError.IsVisible = true;
            }
            else if (ex.Message.Contains("WRONG_PASSWORD") || ex.Message.Contains("INVALID_PASSWORD"))
            {
                PasswordError.Text = "Incorrect password";
                PasswordError.IsVisible = true;
            }
            else if (ex.Message.Contains("TOO_MANY_ATTEMPTS"))
            {
                EmailError.Text = "Too many attempts. Please try again later";
                EmailError.IsVisible = true;
            }
            else if (ex.Message.Contains("NETWORK_ERROR") || ex.Message.Contains("TIMEOUT"))
            {
                DisplayAlert("Connection Error", "Please check your internet connection and try again", "OK");
            }
            else
            {
                DisplayAlert("Error", "Incorrect Email or Password", "OK");
            }
        }

        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResetPasswordPage());
        }

        private void OnGoToSignUpClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) &&
                   Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }
    }
}