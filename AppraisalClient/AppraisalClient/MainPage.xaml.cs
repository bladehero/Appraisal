using System;
using Xamarin.Forms;

namespace AppraisalClient
{
    public partial class MainPage : ContentPage
    {
        bool isRegistered = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            var args = ipEntry.Text.Split(':');
            string server =args[0];
            int port = Convert.ToInt32(args[1]);

            App.ClientObject = new ClientObject(server, port);
            App.ClientObject.OnUpdate += Client_OnUpdate;
        }

        private void Client_OnUpdate(object sender, UpdateArgs e)
        {
            var update = e.Update.ToString();
            if (!isRegistered)
            {
                var names = update.Split('\n');
                var window = new JudgeWindow(names);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushModalAsync(window, true);
                });
                isRegistered = true;
            }
    
            if(update.Contains("Vote"))
            {
                var args = update.Split(';');
                var window = new VotesPage(args[1], args[2], Convert.ToInt32(args[3]), Convert.ToInt32(args[4]));
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushModalAsync(window, true);
                });
            }
        }
    }
}
