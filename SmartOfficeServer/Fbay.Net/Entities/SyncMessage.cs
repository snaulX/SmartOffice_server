namespace Fbay.Net.Entities
{
    internal class SyncMessage : Message
    {
        internal int entityId, position;
        internal byte[] varData;

        public SyncMessage()
        {
            Write(0, 0, System.Array.Empty<byte>());
        }

        public SyncMessage(int entityId, int position, byte[] varData)
        {
            Write(entityId, position, varData);
        }

        public override void Read()
        {
            entityId = ReadInt();
            position = ReadInt();
            varData = ReadByteArray();
        }

        public void Write(int entityId, int position, byte[] varData)
        {
            this.entityId = entityId;
            this.position = position;
            this.varData = varData;
            Write();
        }

        public override void Write()
        {
            WriteInt(entityId);
            WriteInt(position);
            WriteByteArray(varData);
        }
    }
}
