using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppraisalClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JudgeWindow : ContentPage
    {
        public JudgeWindow(string[] judges)
        {
            InitializeComponent();
            judgesPicker.ItemsSource = judges;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.ClientObject.Execute("Judge:" + judgesPicker.SelectedItem.ToString());

            var window = new WaitWindow("Пожалуйста, подождите...");
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PushModalAsync(window, true);
            });
        }
    }
}