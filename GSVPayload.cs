using GSVWriter.Model;

namespace GSVWriter
{
    public sealed class GSVPayload
    {
        // Semi-const
        public readonly UESaveGameHeader header;
        const string PropName = "Payload";
        const string PropVariant = "ArrayProperty";
        const int PropVariantTag = 1;
        const string PropType = "StrProperty";
        const int PropTypeTag = 0;

        // Props
        int FixedSize => PropName.FLengthFull() + PropVariant.FLengthFull() + 4 + PropType.FLengthFull() + 4;
        public string FilePath { get; set; }


        public GSVPayload(string saveGameClassName, string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.Path.IsPathFullyQualified(filePath))
                throw new ArgumentException($"File path cannot be empty or invalid!");
            this.header = new UESaveGameHeader(saveGameClassName);
            FilePath = filePath;
        }

        public void SerializePayload(IReadOnlyCollection<string> data)
        {
            if (data == null || data.Count == 0)
                throw new InvalidDataException("Serializeable data cannot be emnpty!");

            int payloadSize = 4;
            foreach (var str in data)
                payloadSize += str.FLengthFull();

            using (FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    header.Serialize(writer);
                    writer.WriteFString(PropName);
                    writer.WriteFString(PropVariant);
                    writer.Write(PropVariantTag);
                    writer.WriteFString(PropType);
                    writer.Write(PropTypeTag);
                    //--
                    writer.Write(payloadSize);
                    writer.Write((byte)0); // padding?
                    writer.Write(data.Count);
                    foreach (var str in data)
                        writer.WriteFString(str);
                    //--
                    writer.WriteFString("None"); // indicates end of properties
                    writer.Write((int)0);
                }
            }
        }
    }
}
