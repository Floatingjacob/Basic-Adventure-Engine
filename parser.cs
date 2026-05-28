using System.IO.Compression;

public static class Parser
{
    // Should eventually make this convert "adventure source" to some sort of h2 database for speed 
    public static void parseAdventure(string adventureFile, bool indev = false)
    {
        string path = "indev";
        Entry.a.indev = indev;
        if (!indev)
        {
            if (Directory.Exists("tmp")) Directory.Delete("tmp", true);
            path = "tmp";
            ZipFile.ExtractToDirectory(adventureFile, "tmp", true);
        }
            foreach (String l in File.ReadAllLines(Path.Join(path, "meta")))
            {
                string[] line = l.Split(':', 2);
                switch (line[0])
                {
                    case "AUTHOR":
                        Console.WriteLine($"This adventure is authored by \"{line[1]}\"");
                        Entry.a.adventureInfo.Author = line[1];
                        break;
                    case "ADVENTURE":
                        Console.WriteLine($"This adventure is called \"{line[1]}\"");
                        Entry.a.adventureInfo.Name = line[1];
                        break;
                    case "STARTING":
                        Entry.a.adventureInfo.StartingScene = line[1];
                        break;
                }
            }
        
        foreach (String d in Directory.GetDirectories(path))
        {
            string[] dir = d.Split(Path.DirectorySeparatorChar);

            // I know that there's a more efficent way to do this, but I don't know what that way is lol
            if (dir[dir.Length - 1] == "actions")
            {
                foreach (String f in Directory.EnumerateFiles(d))
                {
                    int lineNum = 0;
                    Action action = new() { Actions = [], ID = "UNDEFINED"};
                    string[] lines = File.ReadAllLines(f);
                    foreach (string line in lines)
                    {
                        lineNum++;
                        if (line == "") continue;
                        string stripped = line.Split('#')[0]; // Moved the "commenting system" to over here
                        string[] something = stripped.Split(':', 2);

                        switch (something[0].ToUpperInvariant())
                        {
                            case "ID":
                                action.ID = something[1];
                                action.Actions.Add(new ActionLine() { Action = stripped, LineNumber = lineNum});
                                break;
                            default:
                                if (String.IsNullOrWhiteSpace(something[0])) break;
                                action.Actions.Add(new ActionLine() { Action = stripped, LineNumber = lineNum});
                                break;
                        }
                    }
                    Entry.a.Actions.Add(action.ID, action);
                }
            }

            if (dir[dir.Length - 1] == "items")
            {

                foreach (String f in Directory.EnumerateFiles(d))
                {
                    Item item = new() { ID = "UNDEFINED", Attributes = [], Name = "UNDEFINED" };
                    string[] lines = File.ReadAllLines(f);
                    foreach (string line in lines)
                    {
                        string[] something = line.Split(':', 2);

                        switch (something[0].ToUpperInvariant())
                        {
                            case "ID":
                                item.ID = something[1];
                                break;
                            case "NAME":
                                item.Name = something[1];
                                break;
                            case "ATK":
                                // Need to implement range parsing (ex. ATK:1..3 would mean attack could be a random number between 1 and 3, inclusive)
                                item.Attack = something[1];
                                break;
                            case "DESC":
                                item.Description = something[1];
                                break;
                            case "ATTR":
                                List<string> attributes = something[1].Split('$').ToList();
                                foreach (string a in attributes)
                                {
                                    switch (a.ToUpperInvariant())
                                    {
                                        case "WEAPON":
                                            item.Attributes.Add(Item.Attribute.Weapon);
                                            break;
                                        case "CONSUMABLE":
                                            item.Attributes.Add(Item.Attribute.Consumable);
                                            break;
                                        case "HEALTH":
                                            item.Attributes.Add(Item.Attribute.Health);
                                            break;
                                        case "QUEST":
                                            item.Attributes.Add(Item.Attribute.Quest);
                                            break;
                                        case "MAGIC":
                                            item.Attributes.Add(Item.Attribute.Magic);
                                            break;
                                    }
                                }
                                break;
                            case "USE":
                                item.Use = something[1];
                                break;
                            case "HEALTH":
                                item.Health = int.Parse(something[1]); // I trust that the adventure creators aren't stupid enough to type the number as a word
                                break;
                        }
                    }
                    Entry.a.Items.Add(item.ID, item);
                }
            }
            if (dir[dir.Length - 1] == "scenes")
            {

                foreach (String f in Directory.EnumerateFiles(d))
                {
                    Scene scene = new() { Actions = [], ID = "UNDEFINED", Text = "UNDEFINED", Shortcuts = [] };
                    string[] lines = File.ReadAllLines(f);
                    foreach (string line in lines)
                    {
                        string[] something = line.Split([':'], 2);

                        switch (something[0].ToUpperInvariant())
                        {
                            case "ID":
                                scene.ID = something[1];
                                break;
                            case "ACTION":
                                scene.Actions.Add(something[1]);
                                break;
                            case "TEXT":
                                scene.Text = something[1];
                                break;
                            case "ITEMS":
                                scene.Items = something[1].Split('%');
                                break;
                            case "ACTIONS":
                                scene.ActionsFile = something[1];
                                break;
                            case "PREACTIONS":
                                scene.PreActionsFile = something[1];
                                break;
                        }
                        if (line.StartsWith('$'))
                        {
                            scene.Shortcuts.Add(something[0].TrimStart('$'), something[1]);
                        }
                    }
                    Entry.a.Scenes.Add(scene.ID, scene);
                }
            }
        }
        validateAdventure();
    }

    private static void validateAdventure() // Should eventually do DRY stuff, but it's fine for now since I just wanted to slam this out before I forgot how I was going to do it
    {
        foreach (var action in Entry.a.Actions.Values)
        {
            foreach (var a in action.Actions)
            {
                string[] l = a.Action.Trim().Split([':', '%'], 2);
                if (l[0].ToUpperInvariant() == "DO")
                {
                    if (!Entry.a.Actions.ContainsKey(l[1])) c.colorPrintln($"**red**Action \"{l[1]}\" is called at line {a.LineNumber} in action \"{action.ID}\" but does not exist.");
                }
                if (l[0].ToUpperInvariant() == "NAV")
                {
                    if (!Entry.a.Scenes.ContainsKey(l[1])) c.colorPrintln($"**red**Scene \"{l[1]}\" is called at line {a.LineNumber} in action \"{action.ID}\" but does not exist.");
                }
            }
        }

        foreach (var scene in Entry.a.Scenes)
        {
            foreach (var s in scene.Value.Shortcuts)
            {
                string[] shortcut = s.Value.Trim().Split('%', 2);
                if (shortcut[0].ToUpperInvariant() == "DO")
                {
                    if (!Entry.a.Actions.ContainsKey(shortcut[1])) c.colorPrintln($"**red**Action \"{shortcut[1]}\" is called from within scene \"{scene.Value.ID}\" but does not exist.");
                }
                if (shortcut[0].ToUpperInvariant() == "NAV")
                {
                    if (!Entry.a.Scenes.ContainsKey(shortcut[1])) c.colorPrintln($"**red**Scene \"{shortcut[1]}\" is called from within scene \"{scene.Value.ID}\" but does not exist.");
                }
            }
        }

    }
}