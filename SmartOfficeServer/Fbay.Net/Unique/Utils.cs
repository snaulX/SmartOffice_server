using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Fbay.Net.Unique
{
    public static class UniqueUtils
    {
        public static void WriteUniqueMessage<T>(this Stream stream, T message) where T : Message, new()
            => stream.Write(ByteUtils.ArrayWithLength(new UniqueMessage(message).Data));

        /// <summary>
        /// Pack message to <see cref="UniqueMessage"/> and send it to server
        /// </summary>
        public static void SendUniqueMessage(this FbayClient client, Message message)
            => client.SendMessage(new UniqueMessage(message));

        /// <summary>
        /// Pack message to <see cref="UniqueMessage"/> and send it to all clients
        /// </summary>
        public static void SendUniqueMessage(this FbayServer server, Message message)
            => server.SendMessage(new UniqueMessage(message));
    }
}
