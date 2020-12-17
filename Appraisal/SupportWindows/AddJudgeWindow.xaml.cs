using Appraisal.Logic;
using System.Windows;

namespace Appraisal.SupportWindows
{
    public partial class AddJudgeWindow : Window
    {
        public Judge Judge { get; set; }

        public AddJudgeWindow()
        {
            Judge = new Judge();
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Judge.FirstName = firstNameTextBox.Text;
            Judge.LastName = lastNameTextBox.Text;
            Judge.MiddleName = middleNameTextBox.Text;
            DialogResult = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
