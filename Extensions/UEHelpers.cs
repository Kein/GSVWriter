using System.Text;

public static class UEHelpers
{
    public static bool isWide(this string str)
    {
	    return str.Any((char c) => c > '\u007f');
    }

	public static int FLengthFull(this string str)
    {
		return 4 + str.FLengthWithNull();    
    }

    public static int FLengthWithNull(this string str)
    {
	    if (string.IsNullOrEmpty(str))
		    return 0;
	    if (!str.isWide())
		    return str.Length + 1;
	    return str.Length * 2 + 2;
    }

    public static int FLength(this string str)
	{
		if (string.IsNullOrEmpty(str))
			return 0;
		if (!str.isWide())
			return str.Length;
		return str.Length * 2;
	}

    public static void WriteFString(this BinaryWriter writer, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			writer.Write(0);
			return;
		}
		string text = value + "\0";
		bool isWide = value.isWide();
		int num = text.Length;
		byte[] array = isWide ? Encoding.Unicode.GetBytes(text) : Encoding.ASCII.GetBytes(text);
		num = isWide ? -num : num;
		writer.Write(num);
		writer.Write(array);
	}
}

