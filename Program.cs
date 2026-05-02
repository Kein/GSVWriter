using System;

namespace GSVWriter
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            var filepath = $"{AppContext.BaseDirectory}/payload.sav";
            var GSV = new GSVPayload("/Game/Mods/GSVQueue/SG_QueueItem.SG_QueueItem_C", filepath);
            GSV.SerializePayload(args);
        }
    }
}
