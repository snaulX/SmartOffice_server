using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Collections.Generic;
using Fbay.Net.Unique;

namespace Fbay.Net.Entities
{
    public class EntityServer : UniqueServer
    {
        private Dictionary<int, List<byte[]>> entities = new Dictionary<int, List<byte[]>>();

        public EntityServer() : base()
        {
        }

        public EntityServer(IPAddress address, ushort port) : base(address, port)
        {
            OnUniqueMessage += (msg, cm) => 
            { 
                if (msg.ConvertIfEquals(out SyncMessage syncMessage))
                {
                    entities[syncMessage.entityId][syncMessage.position] = syncMessage.varData;
                }
            };
        }

        public void LoadEntity<T>(T entity) where T : NetEntity
        {
            int hashCode = entity.GetHashCode();
            entity.id = hashCode;
            entity.server = this;
            int pos = 0;
            foreach (FieldInfo f in typeof(T).GetFields())
            {
                if (f.FieldType.IsSubclassOf(typeof(NetSync)))
                    ((NetSync)f.GetValue(entity)).position = ++pos;
            }
        }
    }
}
