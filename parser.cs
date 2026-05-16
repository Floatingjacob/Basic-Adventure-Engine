using System.IO.Compression;

public static class Parser
{
    // Should eventually make this convert "adventure source" to some sort of h2 database for speed 
    public static void parseAdventure(string adventureFile)
    {
        ZipFile.ExtractToDirectory(adventureFile, "tmp", true);
        foreach (String l in File.ReadAllLines(Path.Join("tmp", "meta")))
        {
            string[] line = l.Split(':');
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
        foreach (String d in Directory.GetDirectories("tmp"))
        {
            string[] dir = d.Split(Path.DirectorySeparatorChar);

            // I know that there's a more efficent way to do this, but I don't know what that way is lol
            if (dir[dir.Length - 1] == "actions")
            {
                foreach (String f in Directory.EnumerateFiles(d))
                {
                    Action action = new() { Actions = [], ID = "UNDEFINED", Shortcuts = [] };
                    string[] lines = File.ReadAllLines(f);
                    foreach (string line in lines)
                    {
                        if (line == "") continue;
                        string[] something = line.Split(':', 2);

                        switch (something[0])
                        {
                            case "ID":
                                action.ID = something[1];
                                action.Actions.Add(line);
                                break;
                            default:
                                action.Actions.Add(line);
                                break;
                        }
                        if (line.StartsWith('$'))
                        {
                            action.Shortcuts.Add(something[0].Trim('$'), something[1]);
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
                        string[] something = line.Split(':');

                        switch (something[0])
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
                                    switch (a)
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
                        string[] something = line.Split([':', '%'], 2);

                        switch (something[0])
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
                        }
                        if (line.StartsWith('$'))
                        {
                            scene.Shortcuts.Add(something[0].Trim('$'), something[1]);
                        }
                    }
                    Entry.a.Scenes.Add(scene.ID, scene);
                }
            }

        }
    }
}