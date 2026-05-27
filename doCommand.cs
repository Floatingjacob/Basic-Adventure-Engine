// This has it's own file because it is really chunky
// TODO: add support for while loops, OR, AND, integers, math, etc.
public partial class Adventure
{
   private bool doCommand(string command, string? caller = null) // Boolean so the caller can decide whether to continue excecuting commands 
    {
        string[] a = command.Split([':', '%'], 2);
        a[0] = a[0].TrimStart();
        for (var i = 0; i < a.Length; i++) // Janky ahh commenting system
        {

            if (a[i].Contains('#'))
            {
                a[i] = a[i].Split('#', 2)[0];
            }
        }

        if (caller != null) Caller = caller;
        if (a[0].ToUpper() == "IF")
        {

            ifStack.Push(evalIf(slap(a[1])));

        }
        else if (a[0].ToUpper() == "ENDIF")
        {
            if (ifStack.Count > 0) ifStack.Pop();
        }
        bool shouldExecute = ifStack.All(x => x);

        if (shouldExecute)
        {
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
                    return false;
                case "PRINT":
                    c.colorPrint(slap(a[1]), false);
                    break;
                case "PRINTL":
                    c.colorPrintln(slap(a[1]));
                    break;
                case "QUIT":
                    playing = false;
                    return false;
                case "READ":
                    Console.ReadLine();
                    break;
                case "SET":
                    try
                    {
                        string[] arg = slap(a[1]).Split('=');
                        if (arg[1][0] == '%')
                        {
                            arg[1] = arg[1].Replace("%READ", getInput());

                            // Add more stuff eventually
                        }
                        Variable var = new Variable { Name = arg[0], Value = arg[1] };
                        if (Actions[Caller].Variables == null) Actions[Caller].Variables = [];
                        if (Actions[Caller].Variables.TryAdd(arg[0], var) != true) Actions[Caller].Variables[arg[0]] = var;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case "GSET":
                    try
                    {
                        string[] arg = slap(a[1]).Split('=');
                        if (arg[1][0] == '%')
                        {
                            arg[1] = arg[1].Replace("%READ", getInput());

                            // Add more stuff eventually
                        }
                        GlobalVariable var = new GlobalVariable { Name = arg[0], Value = arg[1] };
                        if (Globals == null) Globals = [];
                        if (Globals.TryAdd(arg[0], var) != true) Globals[arg[0]] = var;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
                case "DO":
                    foreach (string action in Actions[a[1]].Actions) if (!doCommand(action, Caller)) return false;
                    break;
                case "DELAY":
                    Thread.Sleep(int.Parse(a[1].Trim()));
                    break;
                case "RESET":
                    Globals.Clear();
                    foreach(string s in Actions.Keys)
                    {
                        if (Actions[s].Variables != null) Actions[s].Variables.Clear();
                    }
                    displayScene(adventureInfo.StartingScene);
                    break;              
                default:
                    if (a[0].ToUpper() == "ENDIF") break;
                    if (a[0].ToUpper() == "IF") break;
                    Console.WriteLine($"Unknown command: \"{a[0]}\" from caller \"{caller}\"");
                    break;
            }
        }
        return true;
    }
}