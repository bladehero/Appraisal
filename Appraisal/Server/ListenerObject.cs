using Appraisal.Logic;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Appraisal.Server
{
    public class ListenerObject
    {
        private Timer timer;

        public Judge Judge { get; set; }
        public TcpClient TcpClient { get; private set; }
        public TcpListener TcpListener { get; private set; }
        public bool isRegistered = false;

        public event EventHandler<UpdateArgs> OnUpdate;

        public ListenerObject(int port, string address)
        {
            IPAddress ip = IPAddress.Parse(address);
            TcpListener = new TcpListener(ip, port);
            timer = new Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (TcpClient is null) return;

                NetworkStream stream = TcpClient.GetStream();
                if (!isRegistered)
                {
                    Register(stream);
                    isRegistered = true;
                    return;
                }

                byte[] data = new byte[1024];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
                var update = builder.ToString();
                OnUpdate?.Invoke(this, new UpdateArgs(update, this));
            });
        }

        public void Register(Stream stream)
        {
            string update = String.Empty;
            for (int i = 0; i < MainWindow.Competition.Judges.Count; i++)
            {
                update += MainWindow.Competition.Judges[i].ToString();
                if (i < MainWindow.Competition.Judges.Count - 1)
                {
                    update += '\n';
                }
            }
            var data = Encoding.Unicode.GetBytes(update);
            stream.Write(data, 0, data.Length);
        }

        public void WaitForClient()
        {
            TcpListener.Start();
            TcpClient = TcpListener.AcceptTcpClient();
        }
        public void Start()
        {
            timer.Start();
        }
        public void Send(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            TcpClient.GetStream().Write(data, 0, data.Length);
        }
    }

    public class UpdateArgs : EventArgs
    {
        public object Update { get; }
        public ListenerObject Listener { get; }

        public UpdateArgs(object update, ListenerObject listener)
        {
            Update = update;
            Listener = listener;
        }
    }
}
