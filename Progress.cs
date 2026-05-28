public static class Progress
{
    public static void RestoreProgress()
    {
        if (Entry.a.playing)
        {
            c.colorPrint("**yellow**You are currently in an adventure.\n**dyellow**Would you like to save your progress before restoring the next adventure? (y/n) **white**");
            string input = Console.ReadLine();
            bool inputValid = false;
            while (!inputValid)
            {
                while (String.IsNullOrWhiteSpace(input))
                {
                    c.colorPrint("**dyellow**(y/n) **white**");
                    input = Console.ReadLine();
                }
                if (input.ToUpperInvariant() == "Y")
                {
                    SaveProgress();
                    Thread.Sleep(3000);
                    inputValid = true;
                }
                else if (input.ToUpperInvariant() == "N") inputValid = true;
                else input = null;
            }
        }
        Entry.a = new();
        Console.Clear();
        c.colorPrintln("**blue**Choose a save to restore:\n");
        Dictionary<string, string> stuff = [];
        int counter = 1;
        foreach (String f in Directory.GetFiles("saves"))
        {
            stuff.Add(counter.ToString(), f);
            c.colorPrintln($"**yellow**{counter}. **white**{Path.GetFileName(f)}");
            counter++;
        }
        c.colorPrint("\n**white**> **yellow**");
        string i = Console.ReadLine();
        bool valid = false;
        while (!valid)
        {
            while (String.IsNullOrWhiteSpace(i))
            {
                c.colorPrint("**white**>**yellow** ", false);
                i = Console.ReadLine();
            }
            try
            {
                valid = true;
                LoadProgress(stuff[i]);
            }
            catch (KeyNotFoundException)
            {
                valid = false;
                i = null;
            }
        }
    }

    public static void SaveProgress()
    {
        List<string> save = [];

        save.Add($"ADVENTURE::{Entry.a.adventureInfo.Name}"); // adventure name is stored so the right adventure is loaded when a save is restored
        if (Entry.a.indev) save.Add("INDEV");
        save.Add($"CURRENTSCENE::{Entry.a.CurrentScene}");

        foreach (var action in Entry.a.Actions.Values) // saves local variables
        {
            if (action.Variables != null)
            {
                foreach (var localVariable in action.Variables)
                {
                    save.Add($"LOCALVAR::{action.ID}::{localVariable.Value.Name}::{localVariable.Value.Value ?? "NULL"}");
                }
            }
        }
        if (Entry.a.Globals.Values != null)
        {
            foreach (var global in Entry.a.Globals.Values) // saves global variables
            {
                save.Add($"GLOBALVAR::{global.Name}::{global.Value}");
            }
        }
        string path = Path.Combine("saves", $"{Entry.a.adventureInfo.Name} - {DateTime.Now.ToString().Replace('/', '.').Replace(':', '-')}.baesave");
        File.WriteAllLines(path, save);
        Console.WriteLine($"Progress saved at {Path.GetFullPath(path)}");
    }

    public static void LoadProgress(string SaveFile)
    {
        Console.WriteLine($"Loading save file \"{SaveFile}\" now...");
        Entry.a.playing = true;
        string[] save = File.ReadAllLines(SaveFile);
        if (save[1] == "INDEV")
        {
            Parser.parseAdventure("indev", true);
        }
        else
        {
            foreach (string adventure in Entry.adventures)
            {
                string[] a = adventure.Split("::");
                if (a[0] == save[0].Split("::")[1])
                {
                    Parser.parseAdventure(a[1]);
                    break;
                }
            }
        }
        foreach (string line in save)
        {
            string[] currentLine = line.Split("::");
            switch (currentLine[0])
            {
                case "CURRENTSCENE":
                    Entry.a.CurrentScene = currentLine[1];
                    break;
                case "LOCALVAR":
                    Variable var = new Variable { Name = currentLine[2], Value = currentLine[3] };
                    if (Entry.a.Actions[currentLine[1]].Variables == null) Entry.a.Actions[currentLine[1]].Variables = [];
                    if (Entry.a.Actions[currentLine[1]].Variables.TryAdd(currentLine[2], var) != true) Entry.a.Actions[currentLine[1]].Variables[currentLine[2]] = var;
                    break;
                case "GLOBALVAR":
                    GlobalVariable gvar = new GlobalVariable { Name = currentLine[1], Value = currentLine[2] };
                    if (Entry.a.Globals == null) Entry.a.Globals = [];
                    if (Entry.a.Globals.TryAdd(currentLine[1], gvar) != true) Entry.a.Globals[currentLine[1]] = gvar;
                    break;
            }
        }
        Console.WriteLine("Progress restored! Press enter to continue...");
        Console.ReadLine();
        Entry.a.displayScene(Entry.a.CurrentScene);
        while (Entry.a.playing)
        {
            Entry.a.inputLoop();
        }
    }

}