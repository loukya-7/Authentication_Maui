using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.Extensions.Logging;

namespace FirebaseAuthProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyDxWFcn-FjBg_BCZEh1T_jf_8eehNAaSsE",
                AuthDomain = "loginmaui-28396.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
    {
               new EmailProvider()
    }
            }));

            return builder.Build();
        }
    }
}
