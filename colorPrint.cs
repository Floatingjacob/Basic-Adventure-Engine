public static class c
{
	public static void colorPrint(string text, bool useOldColor = true)
	{
		ConsoleColor old = Console.ForegroundColor;
		string[] stuff = text.Split("**"); // IDK what the fuck to name it

        foreach (string s in stuff) 
		{

            switch (s.Trim().ToLowerInvariant())
            {
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    continue;
                case "dblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    continue;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    continue;
                case "dgreen":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    continue;
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    continue;
                case "dcyan":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    continue;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    continue;
                case "dyellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    continue;
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    continue;
                case "dred":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    continue;
                case "gray":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                case "dgray":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    continue;
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    continue;
                case "dmagenta":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    continue;
                case "white":
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
            }
            Console.Write(s);
        }
        if (useOldColor) Console.ForegroundColor = old;
    }

    public static void colorPrintln(string text, bool useOldColor = true)
    {
        colorPrint($"{text}\n", useOldColor);
    }

    public static void colorPrint(ConsoleColor color, string text)
	{
		ConsoleColor old = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.Write(text);
		Console.ForegroundColor = old;
	}
    
    public static void colorPrintln(ConsoleColor color, string text)
	{
        colorPrint(color, $"{text}\n");
	}

}