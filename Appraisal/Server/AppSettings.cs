using Appraisal.Logic;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Appraisal.Server
{
    static class AppSettings
    {
        public static List<Vote> Votes;
        public static MainWindow MainWindow { get; set; }
        public static int StartPort { get; }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Нет сетевых адаптеров с IPv4-адресом в системе!");
        }
        public static List<ListenerObject> Listeners { get; set; }

        static AppSettings()
        {
            Votes = new List<Vote>();
            StartPort = 28040;
            Listeners = new List<ListenerObject>();
        }

        public static async void WaitForConnections(int count, string address)
        {
            for (int i = 0; i < count; i++)
            {
                Listeners.Add(new ListenerObject(StartPort + i, address));
                Listeners[i].Start();
                Listeners[i].OnUpdate += AppSettings_OnUpdate;
                Task.WaitAll();
                await Task.Run(() =>
                {
                    int index = i;
                    Listeners[index].WaitForClient();
                });
            }
        }

        private static void AppSettings_OnUpdate(object sender, UpdateArgs e)
        {
            string update = e.Update.ToString();
            if (update.Contains("Judge:"))
            {
                string judge = update.Replace("Judge:", "");
                SetBusyJudge(judge, e.Listener);
                if (IsAllConnected())
                {
                    MainWindow.StartVotes();
                }
            }

            if (update.Contains("Vote"))
            {
                var args = update.Split(';');
                string nomination = args[1].Trim();
                string group = args[2].Trim();

                int value = Convert.ToInt32(args[3]);
                var judge = e.Listener.Judge;
                Nomination _nomination = null;
                Team _team = null;
                foreach (var n in MainWindow.Competition.Nominations)
                {
                    if (n.ToString() == nomination)
                    {
                        _nomination = n;
                        break;
                    }
                }
                foreach (var t in MainWindow.Competition.Teams)
                {
                    if (t.ToString() == group)
                    {
                        _team = t;
                        break;
                    }
                }

                var vote = new Vote
                {
                    Judge = judge,
                    Nomination = _nomination,
                    Team = _team,
                    Value = value
                };
                Votes.Add(vote);
                MainWindow.Dispatcher.Invoke(() => MainWindow.dataGrid.Items.Refresh());
            }
        }

        private static void SetBusyJudge(string name, ListenerObject listenerObject)
        {
            name = name.Trim();
            var judges = MainWindow.Competition.Judges;
            foreach (var judge in judges)
            {
                if (judge.ToString().Trim() == name)
                {
                    judge.IsBusy = true;
                    listenerObject.Judge = judge;
                    break;
                }
            }
        }
        private static bool IsAllConnected()
        {
            bool isEmpty = false;
            var judges = MainWindow.Competition.Judges;
            foreach (var judge in judges)
            {
                if (!judge.IsBusy)
                {
                    isEmpty = true;
                    break;
                }
            }
            return !isEmpty;
        }
    }
}
