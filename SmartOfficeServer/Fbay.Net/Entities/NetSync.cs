using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fbay.Net.Entities
{
    public abstract class NetSync
    {
        /// <summary>
        /// Position in server entity
        /// </summary>
        internal int position;

        public abstract void Deserialize(byte[] data);
        public abstract byte[] Serialize();
    }

    public class NetSync<T> : NetSync
    {
        private BinaryFormatter bf;
        public T Value { get; private set; }

        public NetSync()
        {
            bf = new BinaryFormatter();
        }
        public NetSync(T value) : this()
        {
            Value = value;
        }

        public override void Deserialize(byte[] data)
        {
            using MemoryStream ms = new MemoryStream(data);
            Value = (T)bf.Deserialize(ms);
        }

        public override byte[] Serialize()
        {
            using MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, Value);
            return ms.ToArray();
        }
    }
}
