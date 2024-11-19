using Firebase.Auth;
using Firebase.Auth.Providers;
using System.Text.RegularExpressions;

namespace FirebaseAuthProject.Pages
{
    public partial class SignUpPage : ContentPage
    {
        private readonly FirebaseAuthClient _authClient;

        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            // Initialize FirebaseAuthClient with configuration
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyDxWFcn-FjBg_BCZEh1T_jf_8eehNAaSsE", // Replace with your Firebase API key
                AuthDomain = "loginmaui-28396.firebaseapp.com", // Replace with your Firebase Project ID if required
                Providers = new FirebaseAuthProvider[] { new EmailProvider() }
            };

            _authClient = new FirebaseAuthClient(config);
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            // Reset error messages
            EmailError.IsVisible = false;
            PasswordError.IsVisible = false;
            FirstNameError.IsVisible = false;
            LastNameError.IsVisible = false;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
            {
                FirstNameError.Text = "First name is required.";
                FirstNameError.IsVisible = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(LastNameEntry.Text))
            {
                LastNameError.Text = "Last name is required.";
                LastNameError.IsVisible = true;
                return;
            }

            if (!IsValidEmail(EmailEntry.Text))
            {
                EmailError.Text = "Please enter a valid email address.";
                EmailError.IsVisible = true;
                return;
            }

            if (!IsValidPassword(PasswordEntry.Text))
            {
                PasswordError.Text = "Password must be at least 6 characters.";
                PasswordError.IsVisible = true;
                return;
            }

            // Sign up with Firebase
            try
            {
                var user = await _authClient.CreateUserWithEmailAndPasswordAsync(EmailEntry.Text, PasswordEntry.Text);
                await DisplayAlert("Success", "Your account has been created!", "OK");

                // You may want to store the first and last name separately, e.g., in Firestore or Firebase Realtime Database.
                // For now, we just pass to the next page.
                await Navigation.PushAsync(new SignInPage());
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Message.Contains("EMAIL_EXISTS"))
                {
                    await DisplayAlert("Error", "An account already exists with this email address.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", $"Sign-up failed: {ex.Message}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }

        private void OnGoToSignInClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignInPage());
        }

        private bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }

        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 6;
        }
    }
}
