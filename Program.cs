// Should probably eventually add a "dev adventure folder" so creators don't need to zip WIP adventures every time they want to test it


using System.Runtime.CompilerServices;

public static class Entry
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
    static string manageAdventure = @"**cyan**Manage Adventures

**white**What do you want to do?
**yellow**1. **white**Import an adventure
**yellow**2. **white**Delete an adventure
**yellow**0. **white**Go back
> **yellow**";
    static void Main(string[] args)
    {
        init();
        /*if (args.Length > 0)
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
        else*/
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
                // I should like, actually finish this menu :p
                case 1:
                    a.newAdventure();
                    break;
                case 3:
                    c.colorPrint(manageAdventure, false);
                    string input = Console.ReadLine();
                    while (String.IsNullOrWhiteSpace(input))
                    {
                        c.colorPrint("**white**>**yellow** ", false);
                        input = Console.ReadLine();
                    }
                    switch (input)
                    {
                        case "1":
                            c.colorPrint("**white**Enter the path to the adventure: ");
                            string i = Console.ReadLine();
                            while (String.IsNullOrWhiteSpace(i))
                            {
                                c.colorPrint("**white**Enter the path to the adventure: ");
                                i = Console.ReadLine();
                                
                            }
                            addAdventure(i);
                            break;
                    }
                    break;
            }
        }
    }

    private static void init()
    {
        if (Directory.Exists("tmp")) Directory.Delete("tmp", true);
        if (!File.Exists("adventures.txt")) File.WriteAllText("adventures.txt", "");
        adventures = File.ReadAllLines("adventures.txt");
    }

    private static void addAdventure(string path)
    {
        try
        {
            path = path.Trim(['"', '\'']);
            string dest = Path.Combine("Adventures", Path.GetFileName(path));
            Directory.CreateDirectory("Adventures");
            File.Copy(path, dest, true); 
            Parser.parseAdventure(dest);
            File.AppendAllText("adventures.txt", $"\n{a.adventureInfo.Name}::{dest}");
            c.colorPrintln($"**green**Adventure \"{a.adventureInfo.Name}\" successfully imported!", true);
            a = new();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
}