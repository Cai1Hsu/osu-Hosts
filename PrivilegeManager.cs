using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace osuHosts;

public static class PrivilegeManager
{
    public static bool IsAdministrator()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows implementation
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        
        // Unix implementation
        // Check if user is root
        // a simple way?
        return Environment.UserName.ToLower() == "root";
    }
    
    public static void GrantMePrivilege()
    {
        if (IsAdministrator()) return;
        
        // ask for admin privilege
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Verb = "runas";
            startInfo.FileName = (Process.GetCurrentProcess().MainModule 
                                  ?? throw new Exception(TranslationAssets.FailedElevateToAdministrator.ToString()))
                .FileName;
            startInfo.Arguments = "--exit";//String.Join(" ", Environment.GetCommandLineArgs().Skip(1));
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = true;

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                throw new Exception(TranslationAssets.FailedElevateToAdministrator + "\nReason: " + ex.Message);
            }
            
            return;
        }
        
        // ask user to run with sudo
        throw new ExternalException(TranslationAssets.AskForSudoPrivilege.ToString());
    }
}