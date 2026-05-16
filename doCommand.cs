// This has it's own file because it is really chunky
public partial class Adventure
{
   private void doCommand(string command, string? caller = null)
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

                        string[] arg = slap(a[1]).Split('=');
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
                        string[] arg = slap(a[1]).Split('=');
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
            }

        }

    }
}