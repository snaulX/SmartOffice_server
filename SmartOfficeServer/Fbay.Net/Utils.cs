using System;
using System.Reflection;
using System.IO;
using System.Net.Sockets;

namespace Fbay.Net
{
    public static class ExtensionUtils
    {
        public static bool AttributeExists<T>(this MemberInfo member) where T : Attribute =>
            member.GetCustomAttribute<T>() != null;

        /// <summary>
        /// Read byte array from stream with this structure <code>{int length, byte[length] data}</code> where data is array to return
        /// </summary>
        /// <returns>Byte array which was readed from stream or if stream hasn't data return empty byte array</returns>
        public static byte[] ReadBytes(this NetworkStream stream)
        {
            if (stream.DataAvailable)
            {
                byte[] lenData = new byte[sizeof(int)];
                stream.Read(lenData);
                int len = lenData.ToInt();
                byte[] allData = new byte[lenData.Length + len];
                stream.Read(allData, lenData.Length, len);
                byte[] data = new byte[len];
                Buffer.BlockCopy(allData, lenData.Length, data, 0, len);
                return data;
            }
            else
            {
                Debug.Warning("Stream hasn't data. Returning empty byte array");
                return Array.Empty<byte>();
            }
        }

        public static void WriteMessage<T>(this Stream stream, T message) where T : Message
            => stream.Write(ByteUtils.ArrayWithLength(message.Data));

        public static void ReadMessage<T>(this NetworkStream stream, T message) where T : Message
            => message.Read(stream.ReadBytes());
        public static T ReadMessage<T>(this NetworkStream stream) where T : Message, new()
        {
            T msg = new T();
            msg.Read(stream.ReadBytes());
            return msg;
        }

        #region ToBytes methods
        public static byte[] ToBytes(this int i) => BitConverter.GetBytes(i);
        public static byte[] ToBytes(this uint i) => BitConverter.GetBytes(i);
        #endregion

        #region From byte[] to type methods
        public static int ToInt(this byte[] ba) => BitConverter.ToInt32(ba);
        public static uint ToUInt(this byte[] ba) => BitConverter.ToUInt32(ba);
        public static Guid ToGuid(this byte[] ba) => new Guid(ba);
        #endregion
    }

    public static class ByteUtils
    { 
        /// <summary>
        /// Join byte arrays into one and return it
        /// </summary>
        public static byte[] Join(params byte[][] newData)
        {
            byte[] data;
            int len = 0;
            foreach (byte[] d in newData)
                len += d.Length;
            data = new byte[len];
            len = 0;
            foreach (byte[] d in newData)
            {
                Buffer.BlockCopy(d, 0, data, len, d.Length);
                len += d.Length;
            }
            return data;
        }

        /// <summary>
        /// Combine byte array and his length to one byte array and return it
        /// </summary>
        public static byte[] ArrayWithLength(byte[] ba) => Join(ba.Length.ToBytes(), ba);
    }
}
