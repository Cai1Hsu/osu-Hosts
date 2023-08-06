using System.Net;
using osuHosts;

void CheckAndElevate()
{
    Logger.Log(TranslationAssets.CheckPrivilege);
    if (PrivilegeManager.IsAdministrator()) return;

    Logger.Log(TranslationAssets.ElevateToAdministrator);
    PrivilegeManager.GrantMePrivilege();
}

const string IP_FILE = "result.csv";

TranslationAssets.BindTranslations();

ArgumentParser.AddOperation("--exit", TranslationAssets.ArgExitUsage,
    _ =>
    {
        Environment.Exit(0);
    });

ArgumentParser.AddOperation("--revert", TranslationAssets.ArgRevertUsage,
    _ =>
    {
        CheckAndElevate();
        
        HostsManager.Revert();
    });

ArgumentParser.AddOperation("--apply", TranslationAssets.ArgApplyUsage,
    arg =>
    {
        CheckAndElevate();
        
        if (!arg.HasValue) throw new Exception(TranslationAssets.NoSpecifiedIP.ToString());
        
        // Check validity of ip
        if (!IPAddress.TryParse(arg.Value, out _)) 
            throw new InvalidDataException(TranslationAssets.InvalidIp.ToString());
        
        HostsManager.Apply(arg.Value);
    });

ArgumentParser.AddOperation("--list", TranslationAssets.ArgListUsage,
    _ =>
    {
        var domains = HostsManager.Domains;

        foreach (var d in domains)
        {
            Logger.Log(d);
        }
        
        Logger.Log(TranslationAssets.AdditionalDomainsExplain);
        Environment.Exit(0);
    });

ArgumentParser.AddOperation("--help", TranslationAssets.ArgHelpUsage,
    _ =>
    {
        ArgumentParser.GenerateHelp();
        
        Environment.Exit(0);
    });
    
ArgumentParser.AddOperation("-h", TranslationAssets.ArgHelpUsage,
    _ =>
    {
        ArgumentParser.GenerateHelp();
        
        Environment.Exit(0);
    });

Logger.Log(TranslationAssets.InitlizeHostsManager);
HostsManager.Init();

Logger.Log(TranslationAssets.Initlized);

// Execute when everything is ready
if (args.Length != 0)ArgumentParser.ParseAndExecute(args);

CheckAndElevate();

// Normal start
Logger.Log(TranslationAssets.NormalStartTitle);

if (!File.Exists(IP_FILE))
{
    File.Create(IP_FILE);
    Logger.Log(TranslationAssets.IpFileNotExist);
    return;
}

var ipContent = File.ReadLines(IP_FILE);

if (ipContent.Count() == 0)
{
    Logger.Log(TranslationAssets.IpFileInvalid);
    return;
}

var ip = string.Empty;
// ignore comments and extract first ip
foreach (var line in ipContent)
{
    var trimed = line.Trim();
    
    if (trimed.Length == 0 || trimed[0] < '0' || trimed[0] > '9') continue;

    var split = trimed.Split(',');
    ip = split[0];
    break;
}

if (!IPAddress.TryParse(ip, out _))
{
    Logger.Log(TranslationAssets.InvalidIp);
    return;
}

Logger.Log(string.Format(TranslationAssets.LoadedIpFile.ToString(), ip));

Logger.Log(TranslationAssets.InterruptHint);
Console.CancelKeyPress += (_, _) =>
{
    Logger.Log("InterruptKeyReceived.");
    HostsManager.Revert();
    Logger.Log(TranslationAssets.RevertedHosts);
};

HostsManager.Apply(ip);
Logger.Log(TranslationAssets.AppliedHosts);

Thread.Sleep(Timeout.Infinite);