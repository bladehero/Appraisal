
using Xamarin.Forms;

namespace AppraisalClient
{
    public partial class App : Application
	{
        public static ClientObject ClientObject { get; set; }

		public App ()
		{
			InitializeComponent();

			MainPage = new AppraisalClient.MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
