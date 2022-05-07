using System;
using System.Net.Sockets;
using System.Text;

namespace Fbay.Net.Entities
{
    public class NetEntity
    {
        internal int id;
        internal EntityServer server;

        public void Sync(NetSync netVar)
        {
            server.SendMessage(new SyncMessage(id, netVar.position, netVar.Serialize()));
        }
    }
}
