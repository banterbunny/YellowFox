using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace YellowFox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_LoadedAsync(object sender, RoutedEventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDvTZmLrg8PgrXzo0Gpu7MMMr4EI2JCLlg"));
            FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync("generic.man@banterbun.com", "generic.man");

            var firebase = new FirebaseClient(
              "https://stuff-7ea31.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
              });

            await firebase.Child("members").Child("t-rex").PutAsync(new Member());


            var members = await firebase.Child("members").OnceAsync<Member>();

            foreach (var member in members)
            {
                Debug.WriteLine($"{member.Object.firstname} {member.Object.lastname}");
            }
        }
    }

    class Member
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
}
