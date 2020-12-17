using System;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Xaml;

namespace AppraisalClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VotesPage : ContentPage
    {
        Android.Widget.NumberPicker numberPicker;
        public VotesPage(string nomination, string group, int minValue, int maxValue)
        {
            InitializeComponent();
            nominationLabel.Text = nomination;
            groupLabel.Text = group;
            numberPicker = new Android.Widget.NumberPicker(Android.App.Application.Context)
            {
                MinValue = minValue,
                MaxValue = maxValue,
                Value = maxValue
            };
            gridNumberPicker.Children.Add(numberPicker);
        }

        private void Send_Click(object sender, EventArgs e)
        {
            var window = new WaitWindow("Ожидание вердикта других судей...");
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushModalAsync(window, true);
            });
            var data = Encoding.Unicode.GetBytes($"Vote;{nominationLabel.Text};{groupLabel.Text};{numberPicker.Value}");
            App.ClientObject.TcpClient.GetStream().Write(data, 0, data.Length);
        }
    }
}
