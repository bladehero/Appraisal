using Appraisal.Logic;
using Appraisal.Server;
using Appraisal.SupportWindows;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;

namespace Appraisal
{
    public partial class MainWindow : Window
    {
        public static Competition Competition { get; set; }
        public static Competition Selected { get; set; }

        public MainWindow()
        {
            Competition = new Competition();
            InitializeComponent();
            teamListBox.ItemsSource = Competition.Teams;
            judgeListBox.ItemsSource = Competition.Judges;
            nominationListBox.ItemsSource = Competition.Nominations;
            AppSettings.MainWindow = this;
            CreateDataGrid();
            dataGrid.ItemsSource = AppSettings.Votes;

            SetLocalAddress();
        }

        #region FirstTab
        private void AddTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddTeamWindow();
            var res = addWindow.ShowDialog();
            if (res == true)
            {
                Competition.Teams.Add(addWindow.Team);
            }
        }
        private void DeleteTeamButton_Click(object sender, RoutedEventArgs e)
        {
            while (teamListBox.SelectedItems.Count > 0)
            {
                Competition.Teams.Remove(teamListBox.SelectedItem as Team);
            }
        }
        private void AddJudgeButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddJudgeWindow();
            var res = addWindow.ShowDialog();
            if (res == true)
            {
                Competition.Judges.Add(addWindow.Judge);
            }
        }
        private void DeleteJudgeButton_Click(object sender, RoutedEventArgs e)
        {
            while (judgeListBox.SelectedItems.Count > 0)
            {
                Competition.Judges.Remove(judgeListBox.SelectedItem as Judge);
            }
        }
        private void AddNominationButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddNominationWindow();
            var res = addWindow.ShowDialog();
            if (res == true)
            {
                Competition.Nominations.Add(addWindow.Nomination);
            }
        }
        private void DeleteNominationButton_Click(object sender, RoutedEventArgs e)
        {
            while (nominationListBox.SelectedItems.Count > 0)
            {
                Competition.Nominations.Remove(nominationListBox.SelectedItem as Nomination);
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            string folder = null;

            var res = fbd.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                folder = fbd.SelectedPath;
            }
            else
            {
                return;
            }

            Competition.Name = nameCompetitionTextBox.Text;
            Competition.Save($@"{folder}{nameCompetitionTextBox.Text}.json");
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog()
            {
                Multiselect = false
            };

            var res = ofd.ShowDialog();
            if (res == true)
            {
                try
                {
                    Competition = Competition.Open(ofd.FileName);
                    Clear();
                    teamListBox.ItemsSource = Competition.Teams;
                    judgeListBox.ItemsSource = Competition.Judges;
                    nominationListBox.ItemsSource = Competition.Nominations;
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Clear()
        {
            teamListBox.ItemsSource = null;
            judgeListBox.ItemsSource = null;
            nominationListBox.ItemsSource = null;
        }
        #endregion

        #region SecondTab
        private void SetLocalAddress()
        {
            try
            {
                ipTextBox.Text = AppSettings.GetLocalIPAddress();
            }
            catch (Exception)
            {
                ipTextBox.Text = "Ошибка подключения!";
            }
        }
        private void RefreshIp_Click(object sender, RoutedEventArgs e)
        {
            SetLocalAddress();
        }
        private void CurrentCompetition_Click(object sender, RoutedEventArgs e)
        {
            Selected = Competition;
            if (Competition != null)
                selectedCompetitionTextBox.Text = Competition.Name;
            else
                selectedCompetitionTextBox.Text = "Ошибка!";
        }
        private void OpenCompetition_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog()
            {
                Multiselect = false
            };

            var res = ofd.ShowDialog();
            if (res == true)
            {
                try
                {
                    Selected = Competition.Open(ofd.FileName);
                    if (Selected != null)
                        selectedCompetitionTextBox.Text = Selected.Name;
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                    selectedCompetitionTextBox.Text = "Ошибка!";
                }
            }
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            startGrid.IsEnabled = false;
            AppSettings.WaitForConnections(Selected.Judges.Count, AppSettings.GetLocalIPAddress());            
        }
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            int minValue = Competition.Nominations.Where(n => n.ToString().Trim() == nominationComboBox.Text.Trim()).FirstOrDefault().MinMark;
            int maxValue = Competition.Nominations.Where(n => n.ToString().Trim() == nominationComboBox.Text.Trim()).FirstOrDefault().MaxMark;
            foreach (var listener in AppSettings.Listeners)
            {
                string message = $"Vote;{nominationComboBox.Text};{groupComboBox.Text};{minValue};{maxValue}";
                var data = Encoding.Unicode.GetBytes(message);
                listener.TcpClient.GetStream().Write(data, 0, data.Length);
            }
        }
        public void StartVotes()
        {
            Dispatcher.Invoke(() =>
            {
                startGrid.Visibility = Visibility.Collapsed;
                processGrid.Visibility = Visibility.Visible;
                firstTab.IsEnabled = false;
                nominationComboBox.ItemsSource = Competition.Nominations;
                groupComboBox.ItemsSource = Competition.Teams;
            });
        }
        private void CreateDataGrid()
        {
            dataGrid.AutoGenerateColumns = false;
            dataGrid.IsReadOnly = true;
            var nominationColumn = new DataGridTextColumn() { Binding = new System.Windows.Data.Binding("Nomination"), Header = "Номинация" };
            var groupColumn = new DataGridTextColumn() { Binding = new System.Windows.Data.Binding("Team"), Header = "Команда" };
            var judgeColumn = new DataGridTextColumn() { Binding = new System.Windows.Data.Binding("Judge"), Header = "Судья" };
            var markColumn = new DataGridTextColumn() { Binding = new System.Windows.Data.Binding("Value"), Header = "Балл" };
            dataGrid.Columns.Add(nominationColumn);
            dataGrid.Columns.Add(groupColumn);
            dataGrid.Columns.Add(judgeColumn);
            dataGrid.Columns.Add(markColumn);
        }
        #endregion

        private void Results_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new ResultWindow(AppSettings.Votes, Competition).Show();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Не все номинации оценены!");
            }
        }
    }
}
