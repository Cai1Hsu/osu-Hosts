using System.Reflection;

namespace osuHosts;

public static class TranslationAssets
{
    public static void BindTranslations()
    {
        // enumerate all fields
        foreach (var field in typeof(TranslationAssets).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            // select out TranslatableString
            if (field.FieldType != typeof(TranslatableString)) continue;
            
            var translatableString = (TranslatableString)field.GetValue(null)!;
            // set all LanguageSpecificString.Translations to Translations
            foreach (var property in typeof(TranslatableString).GetProperties())
            {
                if (property.PropertyType != typeof(LanguageSpecificString)) continue;
                
                var languageSpecificString = (LanguageSpecificString) 
                    // So that the program wont crash.
                    (property.GetValue(translatableString) ?? translatableString.English);
                languageSpecificString.Translations = translatableString.Translations;
                languageSpecificString.UpdateTranslations();
            }
        }
    }

    static TranslationAssets()
    {
        SystemLanguage = GetSystemLanguage();
    }
    
    public static readonly Dictionary<string, Languages> LanguagesMap = new Dictionary<string, Languages>()
    {
        { "zh-CN", Languages.SChinese },
        { "en-US", Languages.English },
    };

    public static Languages SystemLanguage { get; set; }

    public static Languages GetSystemLanguage()
    {
        var lang = System.Globalization.CultureInfo.CurrentUICulture.Name;
        
        if (LanguagesMap.TryGetValue(lang, out var systemLanguage)) return systemLanguage;
        
        return Languages.English;
    }

    public static TranslatableString AskForSudoPrivilege = new()
    {
        English = new ("Please run this program with sudo or when you are root."),
        SChinese = new("请使用 sudo 或在 root 用户下运行本程序。"),
    };

    public static TranslatableString FailedElevateToAdministrator = new()
    {
        English = new ("Failed to elevate to Administrator. Please run this program through 'Run as Administrator'."),
        SChinese = new("无法提升权限。请使用“以管理员身份运行”运行本程序。"),
    };

    public static TranslatableString NoSpecifiedIP = new()
    {
        English = new("No ip specified, please use --apply=<ip> to specify ip."),
        SChinese = new("未指定 ip，请使用 --apply=<ip> 指定 ip。"),
    };

    public static TranslatableString InvalidIp = new()
    {
        English = new("Invalid ip, please use --apply=<ip> to specify ip."),
        SChinese = new("无效的 ip，请使用 --apply=<ip> 指定 ip。"),
    };

    public static TranslatableString HelpTitle = new()
    {
        English = new("osu!Hosts\n" +
                      "A program to speed up your connection to osu! servers by modifying `Hosts`.\n" +
                      "Usage: osuHosts [options]\n" +
                      "Options:"),
        SChinese = new("osu!Hosts\n" +
                       "一个通过修改 `Hosts` 来加速连接 osu! 服务器的程序。\n" +
                       "用法：osuHosts [选项]\n" +
                       "选项："),
    };

    public static TranslatableString HelpExample = new()
    {
        English = new("Examples:\n" +
                      "\tosuHosts.exe --revert\n" +
                      "\tosuHosts.exe --apply=1.1.1.1")
    };

    public static TranslatableString ArgHelpUsage = new()
    {
        English = new("-h/--help\tShow this help."),
        SChinese = new("-h/--help\t显示此帮助。"),
    };

    public static TranslatableString ArgRevertUsage = new()
    {
        English = new("--revert\tRevert Hosts file to original state."),
        SChinese = new("--revert\t将 Hosts 文件恢复"),
    };

    public static TranslatableString ArgApplyUsage = new()
    {
        English = new("--apply=<ip>\tApply Hosts file with specified ip."),
        SChinese = new("--apply=<ip>\t使用指定的 ip 应用 Hosts 文件。"),
    };

    public static TranslatableString NormalStartTitle = new()
    {
        English = new("============================\n" +
                      "osu!Hosts\n" +
                      "A program to speed up your connection to osu! servers by modifying `Hosts`.\n" +
                      "You can use --help or -h to see more usage."),
        SChinese = new("============================\n" +
                       "osu!Hosts\n" +
                       "一个通过修改 `Hosts` 来加速连接 osu! 服务器的程序。\n" +
                       "你可以通过添加 --help 或 -h 参数来查看更多使用方法。"),
    };
    
    public static TranslatableString CheckPrivilege = new()
    {
        English = new("Checking privilege..."),
        SChinese = new("正在检查权限..."),
    };
    
    public static TranslatableString ElevateToAdministrator = new()
    {
        English = new("Elevating to Administrator..."),
        SChinese = new("正在提升权限...请在弹出的窗口中选择“是”。"),
    };
    
    public static TranslatableString InitlizeHostsManager = new()
    {
        English = new("Initlizing HostsManager..."),
        SChinese = new("正在初始化 HostsManager..."),
    };

    public static TranslatableString IpFileNotExist = new()
    {
        English = new("result.csv does not exist. We have created this file for you.\n" +
                      "Add the target ip to the first line of the file and restart this program to enable acceleration."),
        SChinese = new("result.csv 不存在。我们已经为你创建了该文件。\n" +
                       "向文件中第一行添加目标 ip 后重启本程序即可开启加速。\n" +
                       "或使用 CloudflareST.exe 来自动测速。"),
    };

    public static TranslatableString LoadedIpFile = new()
    {
        English = new("result.csv loaded. {0} will be used as target ip."),
        SChinese = new("已加载 result.csv。目标 ip: {0}。"),
    };
    
    public static TranslatableString RevertedHosts = new()
    {
        English = new("Hosts file reverted."),
        SChinese = new("Hosts 文件已恢复。"),
    };
    
    public static TranslatableString AppliedHosts = new()
    {
        English = new("Hosts file applied."),
        SChinese = new("Hosts 文件已应用。\n" +
                       "加速已启动。"),
    };

    public static TranslatableString AdditionalDomainsExplain = new()
    {
        English = new($"Add additional domains to {HostsManager.AdditionalDomainsFile}. One domain per line."),
        SChinese = new($"额外的域名请添加到 {HostsManager.AdditionalDomainsFile}。每行一个域名。"),
    };
    
    public static TranslatableString ArgListUsage = new()
    {
        English = new("--list\tList all domains to modify."),
        SChinese = new("--list\t列出所有要修改的域名。"),
    };

    public static TranslatableString InterruptHint = new()
    {
        English = new("Press Ctrl+C to stop acceleration."),
        SChinese = new("正在加速中，按 Ctrl+C 停止。"),
    };

    public static TranslatableString Initlized = new()
    {
        English = new("Initlized."),
        SChinese = new("初始化完成。\n" +
                       "如果遇到在强制关闭后无法上网的情况，请尝试手动删除 Hosts 文件中的相关内容或重启本程序。"),
    };
    
    public static TranslatableString IpFileInvalid = new()
    {
        English = new("result.csv is invalid. Please make sure there is only one valid ip in result.csv."),
        SChinese = new("result.csv 无效。请确保 result.csv 中有且只有一行有效的 ip。"),
    };

    public static TranslatableString ArgExitUsage = new()
    {
        English = new("--exit\tInternal use only, for elevating privilege."),
        SChinese = new("--exit\t仅供内部使用，用于提升权限。"),
    };
    
    public static TranslatableString UnrecognizedArgument = new()
    {
        English = new("Unrecognized argument: {0}"),
        SChinese = new("无法识别的参数：{0}"),
    };
}