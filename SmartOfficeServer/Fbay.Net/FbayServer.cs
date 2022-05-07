using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Reflection;

namespace Fbay.Net
{
    /// <summary>
    /// Manager of client-server connections on server-side
    /// </summary>
    public class FbayServer
    {
        private TcpListener listener;
        private Thread listen;
        private List<ConnectionManager> clients;
        /// <summary>
        /// Event for assigment to <see cref="ConnectionManager"/>
        /// </summary>
        private event Action<ConnectionManager> onMessage;

        public event Action<ConnectionManager> OnMessage
        {
            add
            {
                foreach (ConnectionManager cl in clients)
                    cl.OnMessage += value;
                onMessage += value;
            }
            remove
            {
                foreach (ConnectionManager cl in clients)
                    cl.OnMessage -= value;
                onMessage -= value;
            }
        }
        public event Action<ConnectionManager> OnConnected;

        public IPAddress Address { get; protected set; }
        public ushort Port { get; protected set; }

        /// <summary>
        /// Base initilization of server
        /// </summary>
        protected FbayServer()
        {
            clients = new List<ConnectionManager>();
            listen = new Thread(Listen);
        }
        public FbayServer(IPAddress address, ushort port) : this()
        {
            Address = address;
            Port = port;
            listener = new TcpListener(address, port);
        }
        public FbayServer(string address, ushort port) : this(IPAddress.Parse(address), port) { }

        /// <summary>
        /// Start listening pending connections
        /// </summary>
        public virtual void Start()
        {
            listener.Start();
            listen.Start();
        }

        private void Listen()
        {
            while (true)
            {
                try
                {
                    if (listener.Pending())
                    {
                        ConnectionManager client = new ConnectionManager(listener.AcceptTcpClient());
                        OnConnected?.Invoke(client);
                        client.OnMessage += onMessage;
                        client.Start();
                        clients.Add(client);
                    }
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Send message to all clients
        /// </summary>
        public void SendMessage(Message message)
        {
            foreach (ConnectionManager cl in clients)
                cl.SendMessage(message);
        }
        /// <summary>
        /// Pack string to <see cref="StrMessage"/> and send it to all clients
        /// </summary>
        public void SendMessage(string str) => SendMessage((Message)new StrMessage(str));
        /// <summary>
        /// Pack bool to <see cref="BoolMessage"/> and send it to all clients
        /// </summary>
        public void SendMessage(bool b) => SendMessage((Message)new BoolMessage(b));
        /// <summary>
        /// Pack int to <see cref="IntMessage"/> and send it to all clients
        /// </summary>
        public void SendMessage(int i) => SendMessage((Message)new IntMessage(i));

        /// <summary>
        /// Stop listening pending connections and close all connections
        /// </summary>
        public virtual void Stop()
        {
            foreach (ConnectionManager cl in clients)
                cl.Stop();
            listener.Stop();
        }
    }
}
