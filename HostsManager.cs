using System.Runtime.InteropServices;

namespace osuHosts;

public static class HostsManager
{
    public const string AdditionalDomainsFile = "AdditionalDonames.txt";
    
    private const string PREFIX = "# osu!Hosts";

    private static readonly string PrefixStart = $"{PREFIX} Start";
    private static readonly string PrefixEnd = $"{PREFIX} End";

    private const string WINDOWS_HOSTS = @"C:\Windows\System32\drivers\etc\hosts";

    private const string UNIX_HOSTS = "/etc/hosts";

    public static List<string> Domains = new();

    private static string HostsFile
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return WINDOWS_HOSTS;

            return UNIX_HOSTS;
        }
    }

    static HostsManager()
    {
        HostsLines = new List<string>();
    }
    
    private static List<string> HostsLines { get; set; }

    private static List<string> ReadAsLines()
    {
        using StreamReader reader = new(HostsFile);
        return reader.ReadToEnd().Replace("\r", String.Empty).Split('\n').ToList();
    }

    private static void SaveHosts()
    {
        using StreamWriter sw = new StreamWriter(HostsFile);
        foreach (var hostsLine in HostsLines)
        {
            sw.WriteLine(hostsLine);
        }
    }

    public static void Apply(string ip)
    {
        RemoveModifiedLines();
        
        // Mark Begin
        HostsLines.Add(PrefixStart);

        foreach (var domain in Domains)
        {
            // easy to check if it is a domain
            if (!domain.Contains('.')) continue;
            
            var line = $"{ip} {domain}";
            HostsLines.Add(line);
        }
        
        // Mark End
        HostsLines.Add(PrefixEnd);

        SaveHosts();
    }

    public static void Revert()
    {
        RemoveModifiedLines();
        
        SaveHosts();
    }

    private static void RemoveModifiedLines()
    {
        var skip = false;
        HostsLines = HostsLines.Where(line =>
        {
            if (line.StartsWith(PrefixEnd)) skip = false;
            if (line.StartsWith(PrefixStart)) skip = true;

            return !skip;
        }).Where(line => !line.StartsWith(PrefixEnd) && !line.StartsWith(PrefixStart)).ToList();
        
        HostsLines.ForEach(line => line = line.Trim());
        HostsLines = HostsLines.Where(line => !string.IsNullOrEmpty(line)).ToList();
    }

    public static void Init()
    {
        HostsLines = ReadAsLines();
        
        ReadAdditionalDomains();
    }

    private static void ReadAdditionalDomains()
    {
        foreach (var d in BuildinDonames)
        {
            Domains.Add(d);
        }

        if (!File.Exists(AdditionalDomainsFile))
        {
            // then create one
            File.Create(AdditionalDomainsFile);
            
            // and we are done
            return;
        }

        using StreamReader reader = new(AdditionalDomainsFile);
        var newLines = reader.ReadToEnd()
            .Replace("\r", String.Empty)
            .Split('\n')
            .Where(line => !BuildinDonames.ToHashSet().Contains(line));
        
        foreach (var line in newLines)
        {
            Domains.Add(line);
        }
    }
    
    private static string[] BuildinDonames = new[]
    {
        "osu.ppy.sh",
        "osu.ppy.sh",
        "a.ppy.sh",
        "c.ppy.sh",
        "i.ppy.sh",
        "s.ppy.sh",
        "assets.ppy.sh",
        "notify.ppy.sh",
        "bm1.ppy.sh",
        "bm2.ppy.sh",
        "bm3.ppy.sh",
        "bm4.ppy.sh",
        "bm5.ppy.sh",
        "bm6.ppy.sh",
        "bm7.ppy.sh",
        "bm8.ppy.sh",
        "bm9.ppy.sh",
        "c1.ppy.sh",
        "c2.ppy.sh",
        "c3.ppy.sh",
        "c4.ppy.sh",
        "c5.ppy.sh",
        "c6.ppy.sh",
        "c7.ppy.sh",
        "c8.ppy.sh",
        "c9.ppy.sh",
        "ce.ppy.sh",
        "osustats.ppy.sh",
        "lazer.ppy.sh",
        "www.ppy.sh",
        "merch.ppy.sh",
        "imgur-archive.ppy.sh",
        "forum-files.ppy.sh",
        "data-r2.ppy.sh",
        "data.ppy.sh",
        "auth-files.ppy.sh",
        "dev.ppy.sh",
        "status.ppy.sh",
    };
}