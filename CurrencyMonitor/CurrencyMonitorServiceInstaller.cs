using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace CurrencyMonitor
{
  [RunInstaller(true)]
  public partial class CurrencyMonitorServiceInstaller : Installer
  {
    public CurrencyMonitorServiceInstaller()
    {
      InitializeComponent();

      ServiceProcessInstaller processInstaller = new ServiceProcessInstaller
      {
        Account = ServiceAccount.LocalSystem
      };

      ServiceInstaller serviceInstaller = new ServiceInstaller
      {
        StartType = ServiceStartMode.Automatic,
        ServiceName = MonitorService.Name,
        DisplayName = "Monitoring Bitcoin currency service",
        Description = "Send email notifications by currency checkpoints"
      };

      Installers.Add(processInstaller);
      Installers.Add(serviceInstaller);
    }
  }
}
