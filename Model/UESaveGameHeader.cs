namespace GSVWriter.Model
{

    public sealed class UESaveGameHeader : ISerializeable
    {
        // Const
        public const UEVer VERSION = UEVer.UE_54;

        // Serializeable
        const int UE_SAVEGAME_FILE_TYPE_TAG = 0x53415647; // "SAVG"
        const int SaveGameFileVersion = 3;
        readonly FPackageFileVersion PackageFileUEVersion;
        readonly FEngineVersionBase  SavedEngineVersion;
        const int CustomVersionFormat = 3;
        const int FCustomVersionArray = 0; // FCustomVersionArray is actually array of FCustomVersion structs but we ignore it here
        readonly string SaveGameClassName = ""; // /Game/Mods/GSVQueue/SG_QueueItem.SG_QueueItem_C


        public UESaveGameHeader(string saveGameClassName)
        {
            if (string.IsNullOrEmpty(saveGameClassName))
                throw new ArgumentException($"{nameof(SaveGameClassName)} cannot be null!");
            PackageFileUEVersion = new FPackageFileVersion(522, 1012);
            SavedEngineVersion = new FEngineVersionBase(5, 4, 3, 0, "UE5");
            SaveGameClassName = saveGameClassName;
        }

        public void Serialize(BinaryWriter bWriter)
        {
            bWriter.Write(UE_SAVEGAME_FILE_TYPE_TAG);
            bWriter.Write(SaveGameFileVersion);
            PackageFileUEVersion.Serialize(bWriter);
            SavedEngineVersion.Serialize(bWriter);
            bWriter.Write(CustomVersionFormat);
            bWriter.Write(FCustomVersionArray); // array serialization in reality
            bWriter.WriteFString(SaveGameClassName);
            bWriter.Write((byte)0);             // padding
        }

        public int GetSizeInBytes() =>
                    4 + 4 + PackageFileUEVersion.GetSizeInBytes() + SavedEngineVersion.GetSizeInBytes() + 4 + 4 + SaveGameClassName.FLengthFull();
    }

    public enum UEVer
    {
        None = 0,
        UE_54 = 1,
        Unknown = 2
    }

    public readonly struct FPackageFileVersion : ISerializeable
    {
        public readonly int FileVersionUE4 = 522;
		public readonly int FileVersionUE5 = 1012;

        public FPackageFileVersion(int fileVersionUE4, int fileVersionUE5)
        {
            FileVersionUE4 = fileVersionUE4;
            FileVersionUE5 = fileVersionUE5;
        }

        public int GetSizeInBytes() => 8;

        public void Serialize(BinaryWriter bWriter)
        {
            bWriter.Write(FileVersionUE4);
            bWriter.Write(FileVersionUE5);
        }
    }

    public readonly struct FEngineVersionBase : ISerializeable
    {
        public readonly ushort Major = 5;
    	public readonly ushort Minor = 4;
        public readonly ushort Patch = 3;
    	public readonly uint   Changelist = 0;
        public readonly string Branch = "UE5";

        public FEngineVersionBase(ushort major, ushort minor, ushort patch, uint changelist, string branch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Changelist = changelist;
            Branch = branch;
        }

        public int GetSizeInBytes() => 2 + 2 + 2 + 4 + Branch.FLengthFull();

        public void Serialize(BinaryWriter bWriter)
        {
            bWriter.Write(Major);
            bWriter.Write(Minor);
            bWriter.Write(Patch);
            bWriter.Write(Changelist);
            bWriter.WriteFString(Branch);
        }
    }
}
