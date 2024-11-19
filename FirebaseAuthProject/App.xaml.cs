using FirebaseAuthProject.Pages;
namespace FirebaseAuthProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        // Override CreateWindow to set the main page of the app
        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Set SignUpPage as the root page of the app
            return new Window(new NavigationPage(new SignUpPage()));
        }
    }
}
