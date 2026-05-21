public static class Entry
{
    public static Adventure a = new();
    public static List<string> adventures = [];
    public static bool reloading = false;
    static string greeting = @$"**white**Welcome to my Basic Adventure Engine!

What do you want to do?

**yellow**1. **white**Start a new adventure
**yellow**2. **white**Load an old adventure
**yellow**3. **white**Manage adventures
**yellow**4. **white**Manage saves
**yellow**5. **white**Develop adventure
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
        while (!a.playing)
        {
            a = new();
            if (reloading) reload();
            else
            {
                Console.Title = "Basic Adventure Engine";

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
                    case 0:
                        Environment.Exit(0);
                        break;
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

                                addAdventure();
                                break;
                            case "2":
                                removeAdventure();
                                break;
                        }
                        break;
                    case 5:
                        develop();
                        break;
                }
            }
        }
    }
    private static void init()
    {
        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Basic Adventure Engine"));
        Directory.SetCurrentDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Basic Adventure Engine"));
        if (Directory.Exists("tmp")) Directory.Delete("tmp", true);
        if (!File.Exists("adventures.txt")) File.WriteAllText("adventures.txt", "");
        foreach (string s in File.ReadAllLines("adventures.txt"))
        {
            adventures.Add(s);
        }
        tidy();
    }

    private static void addAdventure()
    {
        try
        {
            c.colorPrint("**white**Enter the path to the adventure: ");
            string path = Console.ReadLine();
            while (String.IsNullOrWhiteSpace(path))
            {
                c.colorPrint("**white**Enter the path to the adventure: ");
                path = Console.ReadLine();

            }
            path = path.Trim(['"', '\'']);
            string dest = Path.Combine("Adventures", Path.GetFileName(path));
            Directory.CreateDirectory("Adventures");
            File.Copy(path, dest, true);
            Parser.parseAdventure(dest);
            adventures.Add($"{a.adventureInfo.Name}::{dest}");
            File.AppendAllLines("adventures.txt", [$"{a.adventureInfo.Name}::{dest}"]);

            c.colorPrintln($"**green**Adventure \"{a.adventureInfo.Name}\" successfully imported!", true);
            a = new();
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }

    private static void removeAdventure()
    {
        Console.WriteLine("Select an adventure to remove\n");
        int count = 1;
        foreach (String s in Entry.adventures)
        {
            c.colorPrint($"**yellow**{count}. **white**{s.Split("::")[0]}\n");
            count++;
        }

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
        try
        {
            File.Delete(adventures[n - 1].Split("::")[1]);
        }
        catch (FileNotFoundException) { }
        adventures[n - 1] = "";
        tidy();
    }

    private static void tidy()
    {
        List<string> result = [];
        foreach (String s in adventures)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s)) continue;
            result.Add(s);
        }
        File.WriteAllLines("adventures.txt", result);
        adventures = result;
    }

    private static void develop(bool clear = true)
    {
        if(clear) Console.Clear();
        if (!Directory.Exists("indev")) Directory.CreateDirectory("indev");
        if (!File.Exists(Path.Combine("indev", "meta"))) generateTemplate();
        c.colorPrintln($"**dgray**Indev adventure is located at {Path.GetFullPath("indev")}");
        c.colorPrintln("**dgreen**Loading adventure now...\n");
        Parser.parseAdventure("indev", true);
        a.newAdventure(true);
    }

    private static void generateTemplate()
    {
        c.colorPrintln("**cyan**Meta file not found in the indev folder. Generating template adventure...");
        string meta = @"ADVENTURE:Template adventure
AUTHOR:floatingjacob
STARTING:bedroom";

        string bedroom = @"ID:bedroom
TEXT:You are in **cyan**your bedroom**white**. It's quite a sight, actually.\nAmong all the empty Monster Energy cans scattered across your floor, **red**something is glowing radioactive-green.**white**\n%ACTIONS\n\nWhat do you do? 
ACTION:1%Toss a book on the glow
ACTION:2%Call the police
ACTION:3%Tell your neighbor
ACTION:0%Exit adventure

$1:NAV%toss
$2:NAV%call
$3:NAV%tellNeighbor
$0:QUIT";

        string toss = @"ID:toss
TEXT:You grab a book off your desk, knocking three others off in the process, and plop it onto the glowing thing.\nThe book instantly vaporizes, and the air turns white-hot.\n\nWithout getting too graphic, **red**you died**white**.\n\n
ACTIONS:tossEnd";

        string tossEnd = @"ID:tossEnd
GSET:ENDING=**red**There was a guy who died the end**white**
DO:retry";

        string retry = @"ID:retry
PRINT:Press enter to continue...
READ
PRINT:You got the %ENDING ending.
DELAY:1000
PRINT:.
DELAY:1000
PRINT:.
DELAY:1000
PRINT:.
PRINT:\nTry again? (y/n): 
SET:input=%READ
IF:%input==y
CLEAR
NAV:RESET
ENDIF
IF:%input!=y
PRINTL:Bye!
DELAY:2000
QUIT
ENDIF";

        string call = @"ID:call
TEXT:Unfortunately, you never paid your phone bill, meaning that your call couldn't complete.\nThe room slowly melts...\n\n
ACTIONS:callEnd";

        string callEnd = @"ID:callEnd
GSET:ENDING=**green**Coward's way out**white**
DO:retry";

        string tellNeighbor = @"ID:tellNeighbor
TEXT:You run to your **cyan**neighbor's house**white**, pound down his door, and slam into his bedroom, where he is currently asking who the handsome man in the mirror is.\nPanting, you frantically explain to him that something in your bedroom is glowing.\n\nHe doesn't react the way you expected. He just smiles at you, and **red**starts glowing radioactive-green.**white**\nHe melts into the floor, and you suffocte in his fumes.\n\n
ACTIONS:neighborEnd";
        string neighborEnd = @"ID:neighborEnd
GSET:ENDING=**red**Radioactive Neighbor**white**
DO:retry";
        try
        {
            Directory.CreateDirectory(Path.Combine("indev", "actions"));
            Directory.CreateDirectory(Path.Combine("indev", "scenes"));
            File.WriteAllText(Path.Combine("indev", "meta"), meta);
            File.WriteAllText(Path.Combine("indev", "scenes", "bedroom"), bedroom);
            File.WriteAllText(Path.Combine("indev", "scenes", "toss"), toss);
            File.WriteAllText(Path.Combine("indev", "scenes", "tellNeighbor"), tellNeighbor);
            File.WriteAllText(Path.Combine("indev", "scenes", "call"), call);
            File.WriteAllText(Path.Combine("indev", "actions", "retry"), retry);
            File.WriteAllText(Path.Combine("indev", "actions", "callEnd"), callEnd);
            File.WriteAllText(Path.Combine("indev", "actions", "neighborEnd"), neighborEnd);
            File.WriteAllText(Path.Combine("indev", "actions", "tossEnd"), tossEnd);
            c.colorPrintln($"**dgreen**Succesfully generated template adventure! **white**\n");
        }
        catch (Exception ex)
        {
            c.colorPrintln($"**red**Error while generating template adventure: **dgray**{ex}");
        }

    }
    public static void reload()
    {
        reloading = false;
        c.colorPrintln("**red**Reloading indev adventure now...");
        develop(false);
    }
}