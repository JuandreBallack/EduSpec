using System.ServiceProcess;
using EduSpecService;

namespace EduspecService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args != null && args.Length == 1 && args[0].Length > 1
                && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    default:
                        break;
                    case "install":
                    case "i":
                        SelfInstaller.InstallService();
                        break;
                    case "uninstall":
                    case "u":
                        SelfInstaller.UninstallService();
                        break;

                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new EduSpecService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
