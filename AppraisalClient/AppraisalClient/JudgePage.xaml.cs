using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppraisalClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JudgePage : ContentPage
    {
        public JudgePage(Judge[] judge)
        {
            InitializeComponent();
            judgesListView.ItemsSource = judge.Where(j => !j.IsBusy);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}