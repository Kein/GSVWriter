namespace GSVWriter
{
    public class Program
    {
        static void Main(string[] args)
        {
            var arg = string.Join(' ', args);
            // Create reuseable instance of GSV writer with fixed class name (it wont change) and File name
            // file name can be changed with FilePath propety at runtime.
            var GSV = new GSVPayload("/Game/Mods/GSVQueue/SG_QueueItem.SG_QueueItem_C", @"N:\payload.sav");
            // Serialize 2 commands
            GSV.SerializePayload(new string[] { "kick -id=Pooh -reason=fat", "exec УТФ8тЕКСТ" });
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
