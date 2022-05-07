using System;

namespace Fbay.Net.Unique
{
    public class UniqueClient : FbayClient
    {
        public event Action<UniqueMessage> OnUniqueMessage;

        public UniqueClient(string address, ushort port) : base(address, port)
        {
            OnMessage += () =>
            {
                UniqueMessage msg = Stream.ReadMessage<UniqueMessage>();
                OnUniqueMessage?.Invoke(msg);
            };
        }

        public void SendBaseMessage(Message message) => base.SendMessage(message);
        public new void SendMessage(Message message) => base.SendMessage(new UniqueMessage(message));
        public new void SendMessage(string str) => base.SendMessage(new UniqueMessage(new StrMessage(str)));
        public new void SendMessage(int i) => base.SendMessage(new UniqueMessage(new IntMessage(i)));
        public new void SendMessage(bool b) => base.SendMessage(new UniqueMessage(new BoolMessage(b)));
    }
}
