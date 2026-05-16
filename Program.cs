public static class entry
{
    public static Adventure a = new();
    public static string[] adventures = [];
    static string greeting = @$"**white**Welcome to my Basic Adventure Engine!

What do you want to do?

**yellow**1. **white**Start a new adventure
**yellow**2. **white**Load an old adventure
**yellow**3. **white**Manage adventures
**yellow**4. **white**Manage saves
**yellow**0. **white**Exit
";
    static void Main(string[] args)
    {
        init();
        if (args.Length > 0)
        {
            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-l": // load save file

                        a.loadProgress(File.ReadAllLines(args[i + 1]));
                        break;
                }
            }
        }
        else
        {

                c.colorPrint(greeting);
                int n = -1;
                while (n == -1)
                {
                    try
                    {
                        c.colorPrint("**white**>**yellow** ", false);
                        n = int.Parse(Console.ReadLine().Trim());
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    catch (FormatException) { }
                }
                switch (n)
                {
                    case 1:
                        a.newAdventure();
                        break;
                }
            }

        
    }

    internal static void init()
    {   if (Directory.Exists("tmp")) Directory.Delete("tmp", true);
        if (!File.Exists("adventures.txt")) File.WriteAllText("adventures.txt", "");
        adventures = File.ReadAllLines("adventures.txt");
    }

}