namespace osuHosts;

public static class Logger
{
    public static void Log(string message)
    {
        foreach (var line in message.Split('\n'))
        {
            Console.Write(GetPrefix());
            Console.WriteLine(line);
        }
    }

    public static void Log(TranslatableString ts)
    {
        Log(ts.ToString());
    }

    private static string GetPrefix()
    {
        return $"[{DateTime.Now:HH:mm:ss}] [osu!Hosts] ";
    }
}