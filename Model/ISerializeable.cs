using System;
using System.IO;

namespace GSVWriter.Model
{
    internal interface ISerializeable
    {
        public void Serialize(BinaryWriter bWriter);
        public int GetSizeInBytes();
    }
}
