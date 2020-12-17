using Appraisal.Logic;
using System.Windows;

namespace Appraisal.SupportWindows
{
    public partial class AddNominationWindow : Window
    {
        public Nomination Nomination { get; set; }

        public AddNominationWindow()
        {
            Nomination = new Nomination();
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Nomination.Name = nameTextBox.Text;
            Nomination.MinMark = (int)minSingleUpDown.Value;
            Nomination.MaxMark = (int)maxSingleUpDown.Value;
            DialogResult = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
