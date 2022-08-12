using System;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;

namespace CurrencyMonitor
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string[] args)
    {
      if (Environment.UserInteractive)
      {
        if ((args?.Length ?? 0) == 0)
          return;

        var currencyMonitorServiceInstaller = new CurrencyMonitorServiceInstaller();

        var context = new InstallContext();
        context.Parameters.Add("AssemblyPath", Assembly.GetExecutingAssembly().Location);
        context.Parameters.Add("LogToConsole", "false");

        using (TransactedInstaller installer = new TransactedInstaller())
        {
          installer.Context = context;
          installer.Installers.Add(currencyMonitorServiceInstaller);

          try
          {
            if (string.Equals(args[0], "i", StringComparison.OrdinalIgnoreCase))
              installer.Install(new Hashtable());
            else
              installer.Uninstall(null);
          }
          catch (Exception e)
          {
            Console.WriteLine(e.Message);
          }

          Console.ReadLine();
        }

        return;
      }

      Trace.Listeners.Add(new EventLogTraceListener(MonitorService.Name));
      Trace.Listeners.Add(new TextWriterTraceListener("log.txt"));

      ServiceBase.Run(new MonitorService());
    }
  }
}
