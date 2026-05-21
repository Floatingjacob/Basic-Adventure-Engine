// TODO: make items actually do stuff, implement saving and loading, etc.
public partial class Adventure
{
    public bool indev = false;
    public Dictionary<string, Scene> Scenes = new();
    public Dictionary<string, Action> Actions = new();
    public Dictionary<string, Item> Items = new();
    private Dictionary<string, GlobalVariable> Globals = new();
    public AdventureInfo adventureInfo = new() { Name = "", StartingScene = "" };
    private string CurrentScene;
    private string Caller = "";
    public bool playing = false;
    private Stack<bool> ifStack = new(); // I don't really understand how this works, but its supposed to make nested if stamements not explode

    /*
        save format should be something like

        adventure ID
        save name
        current room
        stats, variables etc.

    */

    public void saveProgress(string progress)
    {
        // Hello darkness my old fr--
    }

    public void loadProgress(string[] progress)
    {
        playing = true;
        c.colorPrintln(ConsoleColor.DarkBlue, $"Loading save \"{progress[1]}\"");

    }

    public void newAdventure(bool indev = false)
    {
        playing = true;
        if (!indev)
        {
            Console.WriteLine("Which adventure do you want to start?");
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

            Parser.parseAdventure(Entry.adventures[n - 1].Split("::")[1]);
        }
        Console.WriteLine("Press enter to begin...");
        Console.ReadLine();
        Console.Title = $"Basic Adventure Engine - {adventureInfo.Name}";
        Console.Clear();
        displayScene(adventureInfo.StartingScene);
        while (playing)
        {
            inputLoop();
        }

    }

    private void displayScene(String SceneID)
    {
        Console.Clear();
        CurrentScene = SceneID;
        string sceneText = Scenes[SceneID].Text;
        sceneText = sceneText.Replace("%ITEMS", getItems(SceneID));
        sceneText = sceneText.Replace("%ACTIONS", getActions(SceneID));
        sceneText = sceneText.Replace("%ADVENTURE", adventureInfo.Name);

        if (Scenes[SceneID].PreActionsFile != null) doCommand($"DO%{Scenes[SceneID].PreActionsFile}");
        c.colorPrint(slap(sceneText));
        if (Scenes[SceneID].ActionsFile != null) doCommand($"DO%{Scenes[SceneID].ActionsFile}");
    }

    private void inputLoop()
    {
        string input = "";
        while (String.IsNullOrWhiteSpace(input))
        {
            input = Console.ReadLine();
        }
        try
        {
            if (input == "!@#" && indev)
            {
                Entry.reloading = true;
                playing = false;
                return;
            }
            else doCommand(Scenes[CurrentScene].Shortcuts[input.Trim()]);
        }
        catch (Exception ex) {
            switch (ex.GetBaseException())
            {
                case KeyNotFoundException:
                    Console.WriteLine($"Input \"{input}\" not recognized in scene \"{CurrentScene}\" (current scene)");
                    break;
                default:
                    Console.WriteLine(ex);
                    break;
            }
            
        }
    }
    private string getInput()
    {
        string input = "";
        while(String.IsNullOrWhiteSpace(input) || String.IsNullOrEmpty(input))
        {
            input = Console.ReadLine();
        }
        return input;
    }
    private bool evalIf(string conditions)
    {
        string[] parts = [];
        int type = -1;
        // This *could* be nicer, but whatever
        if (conditions.Contains("=="))
        {
            parts = conditions.Split("==");
            type = 0;
        }
        else if (conditions.Contains("!="))
        {
            parts = conditions.Split("!=");
            type = 1;
        }
        switch (type)
        {
            case 0:
                if (parts[0] == parts[1])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 1:
                if (parts[0] != parts[1])
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false; // This path should never be reached, but just in case...
    }

    private string slap(string input)
    {
        string result = input;
        
        if (result == null) return "";
        if (result.Contains("\\n")) result = result.Replace("\\n", "\n");
        if (result.Contains('%'))
        {
            if (Actions[Caller].Variables != null)
            {
                foreach (var v in Actions[Caller].Variables.OrderByDescending(pair => pair.Key.Length))
                {
                    if (result.Contains($"%{v.Value.Name}"))
                    {
                        result = result.Replace($"%{v.Value.Name}", v.Value.Value.Trim());
                    }
                }
            }
            if (Globals != null)
            {
                foreach (var g in Globals.OrderByDescending(pair => pair.Key.Length))
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

    private string getItems(string SceneID)
    {
        string items = "";
        if (Scenes[SceneID].Items == null) return "";
        foreach (string item in Scenes[SceneID].Items)
        {
            items += $"\n{Items[item].Name}";
        }
        return items;
    }

    private string getActions(string SceneID)
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