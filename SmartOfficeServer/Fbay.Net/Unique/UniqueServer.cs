using System;
using System.Net;

namespace Fbay.Net.Unique
{
    public class UniqueServer : FbayServer
    {
        public event Action<UniqueMessage, ConnectionManager> OnUniqueMessage;

        public UniqueServer() : base() { }

        public UniqueServer(IPAddress address, ushort port) : base(address, port)
        {
            OnMessage += (cm) =>
            {
                UniqueMessage msg = cm.Stream.ReadMessage<UniqueMessage>();
                OnUniqueMessage?.Invoke(msg, cm);
            };
        }

        public void SendBaseMessage(Message message) => base.SendMessage(message);
        public new void SendMessage(Message message) => base.SendMessage(new UniqueMessage(message));
        public new void SendMessage(string str) => base.SendMessage(new UniqueMessage(new StrMessage(str)));
        public new void SendMessage(int i) => base.SendMessage(new UniqueMessage(new IntMessage(i)));
        public new void SendMessage(bool b) => base.SendMessage(new UniqueMessage(new BoolMessage(b)));
    }
}
