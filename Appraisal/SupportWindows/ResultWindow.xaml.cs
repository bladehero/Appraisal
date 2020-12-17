using Appraisal.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Appraisal.SupportWindows
{
    public partial class ResultWindow : Window
    {
        IList<Vote> votes;
        Competition competition;
        public ResultWindow(IList<Vote> votes, Competition competition)
        {
            this.votes = votes;
            this.competition = competition;
            InitializeComponent();
            CreatePlaceDataGrid();
            CreateNominationDataGrid();

            var teams = competition.Teams;
            List<ResultMark> resultMarks = new List<ResultMark>();
            foreach (var team in teams)
            {
                var _votes = votes.Where(v => v.Team == team).ToList();
                var resultMark = new ResultMark(team, _votes);
                resultMarks.Add(resultMark);
            }
            resultMarks.Sort(ResultMark.CompareResultMark);
            for (int i = 0; i < resultMarks.Count; i++)
            {
                resultMarks[i].Place = i + 1;
            }
            placeDataGrid.ItemsSource = resultMarks;

            var resultNominations = new List<ResultNomination>();
            foreach (var nomination in competition.Nominations)
            {
                var resultNomination = new ResultNomination(nomination, votes);
                resultNominations.Add(resultNomination);
            }
            nominationsDataGrid.ItemsSource = resultNominations;
        }

        private void CreatePlaceDataGrid()
        {
            placeDataGrid.AutoGenerateColumns = false;
            placeDataGrid.IsReadOnly = true;
            var placeColumn = new DataGridTextColumn() { Binding = new System.Windows.Data.Binding("Place"), Header = "№" };
            var teamColumn = new DataGridTextColumn() { Binding = new System.Windows.Data.Binding("Team"), Header = "Команда" };
            var markColumn = new DataGridTextColumn() { Binding = new System.Windows.Data.Binding("AverageMark"), Header = "Балл за выстулпение" };
            placeDataGrid.Columns.Add(placeColumn);
            placeDataGrid.Columns.Add(teamColumn);
            placeDataGrid.Columns.Add(markColumn);
        }
        private void CreateNominationDataGrid()
        {
            nominationsDataGrid.AutoGenerateColumns = false;
            nominationsDataGrid.IsReadOnly = true;
            var nominationColumn = new DataGridTextColumn() { Binding = new System.Windows.Data.Binding("Nomination"), Header = "Номинация" };
            var teamColumn = new DataGridTextColumn() { Binding = new System.Windows.Data.Binding("Team"), Header = "Команда" };
            nominationsDataGrid.Columns.Add(nominationColumn);
            nominationsDataGrid.Columns.Add(teamColumn);
        }

        private class ResultNomination
        {
            public Team Team { get; set; }
            public Nomination Nomination { get; set; }

            public ResultNomination(Nomination nomination, IList<Vote> votes)
            {
                Nomination = nomination;
                int max = votes.Where(v => v.Nomination == nomination).Max(v => v.Value);
                Team = votes.Where(v => v.Nomination == nomination && v.Value == max).Select(v => v.Team).FirstOrDefault();
            }
        }
        private class ResultMark
        {
            public int Place { get; set; }
            public Team Team { get; set; }
            public double AverageMark { get; set; }

            public ResultMark(Team team, IList<Vote> votes)
            {
                Team = team;
                AverageMark = Math.Round((double)votes.Sum(v => v.Value) / votes.Count, 2);
            }
            public static int CompareResultMark(ResultMark x, ResultMark y)
            {
                if (x.AverageMark > y.AverageMark)
                {
                    return -1;
                }
                else if (x.AverageMark < y.AverageMark)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}