using Appraisal.Logic;
using System.Windows;

namespace Appraisal.SupportWindows
{
    public partial class AddTeamWindow : Window
    {
        public Team Team { get; set; }

        public AddTeamWindow()
        {
            Team = new Team();
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Team.Name = nameTextBox.Text;
            Team.Alias = aliasTextBox.Text;
            DialogResult = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
