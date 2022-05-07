using System;

namespace Fbay.Net.Unique
{
    public sealed class UniqueMessage : Message
    {
        internal static Set<Type> msgTypes = new Set<Type>();
        public static void Add(params Type[] msgTypes) => UniqueMessage.msgTypes.AddRange(msgTypes);

        private Guid msgTypeId;
        public DataMessage message;

        public UniqueMessage()
        {
            msgTypeId = Guid.Empty;
            message = new DataMessage();
            Write();
        }
        public UniqueMessage(Message msg)
        {
            Type t = msg.GetType();
            if (!msgTypes.Contains(t))
                Add(t);
            msgTypeId = msg.GetType().GUID;
            message = new DataMessage(msg.Data);
            Write();
        }

        public override void Write()
        {
            WriteGuid(msgTypeId);
            WriteByteArray(message);
        }

        public override void Read()
        {
            msgTypeId = ReadGuid();
            message = (DataMessage)ReadByteArray();
        }

        public Type GetMsgType()
        {
            foreach (Type t in msgTypes)
            {
                if (t.GUID == msgTypeId)
                    return t;
            }
            return null;
        }

        /// <summary>
        /// Convert data from this message to message with type <typeparamref name="T"/> if types are equals
        /// </summary>
        /// <returns>Types of messages are equals</returns>
        public bool ConvertIfEquals<T>(out T msg) where T : Message, new()
        {
            msg = new T();
            if (GetMsgType() == typeof(T))
            {
                message.ConvertTo(msg);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Convert data from this message to message with type <typeparamref name="T"/>
        /// </summary>
        public T ConvertTo<T>() where T : Message, new()
        {
            if (GetMsgType() == typeof(T))
            {
                T msg = new T();
                message.ConvertTo(msg);
                return msg;
            }
            else
                return new T();
        }
    }
}
