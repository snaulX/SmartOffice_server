using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Fbay.Net
{
    /// <summary>
    /// Manager of client-server connection on client-side
    /// </summary>
    public class FbayClient
    {
        public TcpClient Client { get; protected set; }
        public NetworkStream Stream { get; protected set; }
        private Thread receive;

        /// <summary>
        /// Event when client gets message from server
        /// </summary>
        public event Action OnMessage;

        /// <summary>
        /// IP address of connected server
        /// </summary>
        public string Address { get; protected set; }
        /// <summary>
        /// Port of connected server
        /// </summary>
        public ushort Port { get; protected set; }
        
        public FbayClient(string address, ushort port) 
        {
            Address = address;
            Port = port;
            Client = new TcpClient(address, port);
            Stream = Client.GetStream();
            receive = new Thread(Receive);
        }

        /// <summary>
        /// Start receiving messages from server
        /// </summary>
        public virtual void Start()
        {
            receive.Start();
        }

        private void Receive()
        {
            while (true)
            {
                try
                {
                    if (Stream.DataAvailable)
                    {
                        OnMessage?.Invoke();
                    }
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Send message to server
        /// </summary>
        public void SendMessage(Message message) => Stream.WriteMessage(message);
        /// <summary>
        /// Pack string to <see cref="StrMessage"/> and send it to server
        /// </summary>
        public void SendMessage(string str) => SendMessage((Message)new StrMessage(str));
        /// <summary>
        /// Pack bool to <see cref="BoolMessage"/> and send it to server
        /// </summary>
        public void SendMessage(bool b) => SendMessage((Message)new BoolMessage(b));
        /// <summary>
        /// Pack int to <see cref="IntMessage"/> and send it to server
        /// </summary>
        public void SendMessage(int i) => SendMessage((Message)new IntMessage(i));


        /// <summary>
        /// Close connection
        /// </summary>
        public virtual void Close()
        {
            Client.Close();
        }
    }
}
