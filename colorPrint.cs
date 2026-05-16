public static class c
{
    // Eventually need to do DRY stuff to cut down on the number of lines
	public static void colorPrint(string text, bool useOldColor = true)
	{
		ConsoleColor old = Console.ForegroundColor;
		string[] stuff = text.Split("**"); // IDK what the fuck to name it
		
		for (int i = 0; i < stuff.Length; i++)
		{
			switch (stuff[i])
			{
				case "blue":
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
				case "dblue":
					Console.ForegroundColor = ConsoleColor.DarkBlue;
					Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
				case "green":
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
				case "dgreen":
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
				case "cyan":
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "dcyan":
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "yellow":
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "dyellow":
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "red":
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "dred":
					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "gray":
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "dgray":
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "magenta":
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "dmagenta":
					Console.ForegroundColor = ConsoleColor.DarkMagenta;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
				case "white":
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write(stuff[i + 1]);
                    if (useOldColor)  Console.ForegroundColor = old;
					break;
            }
		}
	}

	public static void colorPrint(ConsoleColor color, string text)
	{
		ConsoleColor old = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.Write(text);
		Console.ForegroundColor = old;
	}
    public static void colorPrintln(string text, bool useOldColor = true)
    {
        ConsoleColor old = Console.ForegroundColor;
        string[] stuff = text.Split("**"); // IDK what the fuck to name it

        for (int i = 0; i < stuff.Length; i++)
        {
            switch (stuff[i])
            {
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "dblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "dgreen":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "dcyan":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "dyellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "dred":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "gray":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "dgray":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "dmagenta":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
                case "white":
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(stuff[i + 1]);
                    if (useOldColor) Console.ForegroundColor = old;
                    break;
            }
        }
        Console.WriteLine(); // Newline
    }
    public static void colorPrintln(ConsoleColor color, string text)
	{
		ConsoleColor old = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.WriteLine(text);
		Console.ForegroundColor = old;
	}

}
