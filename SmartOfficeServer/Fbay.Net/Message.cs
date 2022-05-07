using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

namespace Fbay.Net
{
    public abstract class Message
    {
        private byte[] _data;
        protected int seek = 0;
        // welcome to null-safety world where has ?? operator
        /// <summary>
        /// Data of message in byte array
        /// </summary>
        /// <remarks>Use it carefully when in source code of your message</remarks>
        public byte[] Data { get => _data ?? Array.Empty<byte>(); protected set => _data = value; }

        #region Write methods
        /// <summary>
        /// Add string in <see cref="Encoding.UTF8"/> to <see cref="Data"/>
        /// </summary>
        public void WriteUTF8(string str) => Write(ByteUtils.ArrayWithLength(Encoding.UTF8.GetBytes(str)));
        /// <summary>
        /// Add int to <see cref="Data"/>
        /// </summary>
        public void WriteInt(int i) => Write(i.ToBytes());
        /// <summary>
        /// Add uint to <see cref="Data"/>
        /// </summary>
        public void WriteUInt(uint i) => Write(i.ToBytes());
        /// <summary>
        /// Add byte array to <see cref="Data"/> with this structure <code>{ int length, byte[length] <paramref name="data"/> }</code>
        /// </summary>
        public void WriteByteArray(byte[] data) => Write(ByteUtils.ArrayWithLength(data));
        /// <summary>
        /// Add <see cref="Guid"/> to <see cref="Data"/>
        /// </summary>
        public void WriteGuid(Guid guid) => Write(guid.ToByteArray());
        /// <summary>
        /// Add <paramref name="data"/> to <see cref="Data"/>
        /// </summary>
        private void Write(byte[] data) => Data = ByteUtils.Join(Data, data);
        /// <summary>
        /// Packs message to <see cref="Data"/>
        /// </summary>
        public abstract void Write();
        #endregion

        #region Read methods
        /// <summary>
        /// Read string in <see cref="Encoding.UTF8"/> from <see cref="Data"/>
        /// </summary>
        public string ReadUTF8() => Encoding.UTF8.GetString(ReadByteArray());
        /// <summary>
        /// Read int from <see cref="Data"/>
        /// </summary>
        public int ReadInt() => Read(sizeof(int)).ToInt();
        /// <summary>
        /// Read uint from <see cref="Data"/>
        /// </summary>
        public uint ReadUInt() => Read(sizeof(uint)).ToUInt();
        /// <summary>
        /// Read byte array from <see cref="Data"/> with this structure 
        /// <code>{ int length, byte[length] data }</code>
        /// where data is array to return
        /// </summary>
        public byte[] ReadByteArray()
        {
            int len = ReadInt();
            return Read(len);
        }
        /// <summary>
        /// Read <see cref="Guid"/> from <see cref="Data"/>
        /// </summary>
        public Guid ReadGuid() => Read(16).ToGuid();
        /// <summary>
        /// Reads byte[] from <see cref="Data"/> with given <paramref name="length"/>
        /// </summary>
        /// <param name="length">Length of byte[] to read</param>
        public byte[] Read(int length)
        {
            byte[] data = new byte[length];
            Buffer.BlockCopy(Data, seek, data, 0, length);
            seek += length;
            return data;
        }
        /// <summary>
        /// Write <see cref="Data"/> from <paramref name="data"/>, set seek to zero and call <see cref="Read()"/>
        /// </summary>
        /// <param name="data">Data to read</param>
        internal void Read(byte[] data)
        {
            Data = data;
            seek = 0;
            Read();
        }
        /// <summary>
        /// Read <see cref="Data"/> and pack it to current message
        /// </summary>
        public abstract void Read();
        #endregion
    }

    /// <summary>
    /// Empty realization of <see cref="Message"/>
    /// </summary>
    public class DataMessage : Message
    {
        public DataMessage() { Write(); }
        public DataMessage(byte[] data) { Read(data); }

        public override void Read() { }

        public override void Write() { Data = Array.Empty<byte>(); }

        public void ConvertTo<T>(T msg) where T : Message
        {
            msg.Read(Data);
        }

        public static implicit operator byte[](DataMessage msg) => msg.Data;
        public static explicit operator DataMessage(byte[] data) => new DataMessage(data);
    }

    public sealed class Message<T> : Message
    {
        private BinaryFormatter bf;

        public T Value { get; private set; }

        public Message() : base()
        {
            bf = new BinaryFormatter();
        }
        public Message(T value) : this()
        {
            Write(value);
        }
        public Message(byte[] data)
        {
            Read(data);
        }

        public override void Read()
        {
            Value = (T)bf.Deserialize(new MemoryStream(Data));
        }

        public void Write(T value)
        { 
            Value = value;
            Write();
        }

        public override void Write()
        {
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, Value);
            Data = ms.ToArray();
        }

        public override string ToString() => Value.ToString();
    }

    public sealed class StrMessage : Message
    {
        public string Value { get; private set; }

        public StrMessage() : this(string.Empty)
        { }
        public StrMessage(string message)
        {
            Write(message);
        }
        public StrMessage(byte[] data)
        {
            Read(data);
        }

        public void Write(string msg)
        {
            Value = msg;
            Write();
        }

        public override void Read()
        {
            Value = Encoding.UTF8.GetString(Data);
        }

        public override void Write()
        {
            Data = Encoding.UTF8.GetBytes(Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(StrMessage msg) => msg.Value;
        public static implicit operator byte[](StrMessage msg) => msg.Data;
        public static explicit operator StrMessage(string str) => new StrMessage(str);
        public static explicit operator StrMessage(byte[] data) => new StrMessage(data);
    }

    public sealed class BoolMessage : Message
    {
        public bool Value { get; private set; }

        public BoolMessage() : this(false)
        { }
        public BoolMessage(bool value)
        {
            Write(value);
        }
        public BoolMessage(byte[] data)
        {
            Read(data);
        }

        public void Write(bool value)
        {
            Value = value;
            Write();
        }

        public override void Read()
        {
            Value = BitConverter.ToBoolean(Data);
        }

        public override void Write()
        {
            Data = BitConverter.GetBytes(Value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator bool(BoolMessage msg) => msg.Value;
        public static implicit operator byte[](BoolMessage msg) => msg.Data;
        public static explicit operator BoolMessage(bool b) => new BoolMessage(b);
        public static explicit operator BoolMessage(byte[] data) => new BoolMessage(data);
    }

    public sealed class IntMessage : Message
    {
        public int Value { get; private set; }

        public IntMessage() : this(0)
        { }
        public IntMessage(int value)
        {
            Write(value);
        }
        public IntMessage(byte[] data)
        {
            Read(data);
        }

        public void Write(int value)
        {
            Value = value;
            Write();
        }

        public override void Read()
        {
            Value = Data.ToInt();
        }

        public override void Write()
        {
            Data = Value.ToBytes();
        }

        public override string ToString() => Value.ToString();

        public static implicit operator int(IntMessage msg) => msg.Value;
        public static implicit operator byte[](IntMessage msg) => msg.Data;
        public static explicit operator IntMessage(int i) => new IntMessage(i);
        public static explicit operator IntMessage(byte[] data) => new IntMessage(data);
    }
}
