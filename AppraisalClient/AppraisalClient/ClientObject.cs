using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppraisalClient
{
    public class ClientObject
    {
        private Timer timer;

        public TcpClient TcpClient { get; set; }

        public event EventHandler<UpdateArgs> OnUpdate;

        public ClientObject()
        {
            TcpClient = new TcpClient();
            var timerCallback = new TimerCallback(Tick);
            timer = new Timer(timerCallback);
        }

        public ClientObject(string server, int port) : this()
        {
            Connect(server, port);
        }

        private void Tick(object sender)
        {
            if (TcpClient is null) return;

            Task.Run(new Action(() =>
            {
                var stream = TcpClient.GetStream();
                var data = new byte[1024];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);

                var update = builder.ToString();
                OnUpdate?.Invoke(this, new UpdateArgs(update));
            }));
        }

        public void Execute(string request)
        {
            var data = Encoding.Unicode.GetBytes(request);
            TcpClient.GetStream().Write(data, 0, data.Length);
        }

        public void Connect(string server, int port)
        {
            int _start = 28040;
            TcpClient.Connect(server, _start+port);
            timer.Change(0, 5000);
        }
    }

    public class UpdateArgs : EventArgs
    {
        public object Update { get; }

        public UpdateArgs(object update)
        {
            Update = update;
        }
    }
}
