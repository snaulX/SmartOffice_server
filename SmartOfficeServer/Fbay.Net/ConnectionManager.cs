using System;
using System.Net.Sockets;
using System.Threading;

namespace Fbay.Net
{
    /// <summary>
    /// Manager of client-server connection on server-side
    /// </summary>
    public sealed class ConnectionManager
    {
        public NetworkStream Stream { get; private set; }
        public TcpClient Client { get; private set; }
        private Thread receiver;

        public event Action<ConnectionManager> OnMessage;

        public ConnectionManager(TcpClient client)
        {
            this.Client = client;
            Stream = this.Client.GetStream();
            receiver = new Thread(Receive);
        }

        public void Start() => receiver.Start();

        internal void Receive()
        {
            while (true)
            {
                try
                {
                    if (Stream.DataAvailable)
                    {
                        OnMessage?.Invoke(this);
                    }
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
            }
        }

        public void SendMessage(Message message)
        {
            Stream.WriteMessage(message);
        }

        public void Stop() => Stream.Close();
    }
}
