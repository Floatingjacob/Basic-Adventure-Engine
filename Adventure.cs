public class Adventure
{
    public Dictionary<string, Scene> Scenes = new();
    public Dictionary<string, Action> Actions = new();
    public Dictionary<string, Item> Items = new();
    public Dictionary<string, GlobalVariable> Globals = new();
    public AdventureInfo adventureInfo = new() { Name = "", StartingScene = "" };
    public string CurrentScene;
    public string Caller = "";
    public bool playing = false;
    /*
        save format should be something like

        adventure name
        save name
        current room
        stats, variables etc.

    */

    public void saveProgress(string progress)
    {

    }

    public void loadProgress(string[] progress)
    {
        playing = true;
        c.colorPrintln(ConsoleColor.DarkBlue, $"Loading save \"{progress[1]}\"");

    }

    public void newAdventure()
    {
        playing = true;
        Console.WriteLine("Which adventure do you want to start?");
        int count = 1;
        foreach (String s in entry.adventures)
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

        parser.parseAdventure(entry.adventures[n - 1].Split("::")[1]);
        displayScene(adventureInfo.StartingScene);
        while (playing)
        {
            inputLoop();
        }

    }

    public void displayScene(String SceneID)
    {
        CurrentScene = SceneID;
        string sceneText = Scenes[SceneID].Text;
        sceneText = sceneText.Replace("%ITEMS", getItems(SceneID));
        sceneText = sceneText.Replace("%ACTIONS", getActions(SceneID));
        sceneText = sceneText.Replace("%ADVENTURE", adventureInfo.Name);

        Console.WriteLine(slap(sceneText));
        if (Scenes[SceneID].ActionsFile != null) doCommand($"DO%{Scenes[SceneID].ActionsFile}");


    }

    public void inputLoop()
    {
        string input = "";
        while (String.IsNullOrWhiteSpace(input))
        {
            input = Console.ReadLine();
        }
        try
        {
            doCommand(Scenes[CurrentScene].Shortcuts[input.Trim()]);
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }

    public void doCommand(string command, string caller = null)
    {
        string[] a = command.Split([':', '%'], 2);

        for (var i = 0; i < a.Length; i++) // Janky ahh commenting system
        {

            if (a[i].Contains('#'))
            {
                a[i] = a[i].Split('#', 2)[0];
            }
        }

        if (caller != null) Caller = caller;
        switch (a[0].ToUpper())
        {
            case "ID":
                Caller = a[1];
                break;
            case "CLEAR":
                Console.Clear();
                break;
            case "NAV":
                displayScene(a[1]);
                break;
            case "PRINT":
                Console.Write(slap(a[1]));
                break;
            case "PRINTL":
                Console.WriteLine(slap(a[1]));
                break;
            case "CPRINT":
                c.colorPrint(slap(a[1]), false);
                break;
            case "CPRINTL":
                c.colorPrintln(slap(a[1]));
                break;
            case "QUIT":
                playing = false;
                return;
            case "READ":
                Console.ReadLine();
                break;
            case "SET":
                try
                {
                    // should probably slap the args just in case
                    string[] arg = a[1].Split('=');
                    if (arg[1][0] == '%')
                    {
                        arg[1] = arg[1].Replace("%READ", Console.ReadLine());

                        // Add more stuff eventually
                    }
                    Variable var = new Variable { Name = arg[0], Value = arg[1] };
                    if (Actions[Caller].Variables == null) Actions[Caller].Variables = [];
                    Actions[Caller].Variables.TryAdd(arg[0], var);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                break;
            case "GSET":
                try
                {
                    string[] arg = a[1].Split('=');
                    if (arg[1][0] == '%')
                    {
                        arg[1] = arg[1].Replace("%READ", Console.ReadLine());

                        // Add more stuff eventually
                    }
                    GlobalVariable var = new GlobalVariable { Name = arg[0], Value = arg[1] };
                    if (Globals == null) Globals = [];
                    Globals.TryAdd(arg[0], var);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                break;
            case "DO":
                foreach (string action in Actions[a[1]].Actions) doCommand(action, Caller);
                break;
            case "DELAY":
                Thread.Sleep(int.Parse(a[1].Trim()));
                break;
                // Need to add support for if statements and logical operators (<, >, ==, !=, etc.)

        }

    }

    public string slap(string input)
    {
        string result = input;
        if (result == null) return "";
        if (result.Contains("\\n")) result = result.Replace("\\n", "\n");
        if (result.Contains('%'))
        {
            if (Actions[Caller].Variables != null)
            {
                foreach (var v in Actions[Caller].Variables)
                {

                    if (result.Contains($"%{v.Value.Name}"))
                    {
                        result = result.Replace($"%{v.Value.Name}", v.Value.Value.Trim());
                    }
                }
            }
            if (Globals != null)
            {
                foreach (var g in Globals)
                {

                    if (result.Contains($"%{g.Value.Name}"))
                    {
                        result = result.Replace($"%{g.Value.Name}", g.Value.Value.Trim());
                    }
                }
            }
        }

        return result;
    }

    public string getItems(string SceneID)
    {
        string items = "";
        if (Scenes[SceneID].Items == null) return "";
        foreach (string item in Scenes[SceneID].Items)
        {
            items += $"\n{Items[item].Name}";
        }
        return items;
    }

    public string getActions(string SceneID)
    {
        string actions = "";
        if (Scenes[SceneID].Actions == null) return "";
        foreach (string action in Scenes[SceneID].Actions)
        {
            string[] thingy = action.Split('%'); // Like *you* could think of a better name for it...
            actions += $"\n{thingy[0]}. {thingy[1]}";
        }
        return actions;
    }
}

public class Action
{
    public required string ID { get; set; }
    public required List<string> Actions { get; set; }
    public Dictionary<string, string>? Shortcuts { get; set; }
    public Dictionary<string, Variable>? Variables { get; set; }
}

public class Variable
{
    public required string Name { get; set; }
    public required string Value { get; set; }
    // Should eventually add types like boolean, int, str, etc.
}

public class GlobalVariable
{
    public required string Name { get; set; }
    public required string Value { get; set; }
}

public class Item
{
    public required string ID { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Use { get; set; } // Text to display when the item is used
    public required List<Attribute> Attributes { get; set; }
    public int? Health { get; set; } // How much health the item restores, if applicable
    public string? Attack { get; set; } // How much damage the item inflicts, if applicable

    public enum Attribute
    {
        Weapon,
        Consumable,
        Health, // Health-restoring item
        Quest, // Quest item
        Magic,
        One_Use // If an item has this attribute, it should be destroyed after a single use
    }

}

public class Scene
{
    public required string ID { get; set; }
    public required string Text { get; set; }
    public required List<string> Actions { get; set; }
    public required Dictionary<string, string> Shortcuts { get; set; }
    public string? ActionsFile { get; set; } // Action file to run after displaying the scene text
    public string[]? Items { get; set; }
}



public class AdventureInfo
{
    public required string Name { get; set; }
    public string? Author { get; set; }
    public required string StartingScene { get; set; }
}


