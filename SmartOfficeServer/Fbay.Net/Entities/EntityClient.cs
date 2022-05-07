using System;
using System.Net.Sockets;
using Fbay.Net.Unique;

namespace Fbay.Net.Entities
{
    public class EntityClient : UniqueClient
    {
        public EntityClient(string address, ushort port) : base(address, port)
        {
        }

        public void LoadEntity<T>(T entity) where T : NetEntity
        {
        }
    }
}
