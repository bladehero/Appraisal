using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppraisalClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaitWindow : ContentPage
    {
        public WaitWindow(string text)
        {
            InitializeComponent();
            waitLabel.Text = text;
        }
    }
}