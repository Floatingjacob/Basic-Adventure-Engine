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