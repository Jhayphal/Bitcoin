using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Monitor
{
  public sealed class CurrencyObserver
  {
    private readonly ICurrencyRateProvider rateProvider;

    public event EventHandler<(double, Checkpoint)> OnFound;

    public CurrencyObserver(ICurrencyRateProvider rateProvider)
      => this.rateProvider = rateProvider
        ?? throw new ArgumentNullException(nameof(rateProvider));

    public async Task Observe(MonitorConfiguration configuration)
    {
      try
      {
        foreach (var checkpoint in configuration.Checkpoints)
        {
          var rate = await rateProvider.GetRate(checkpoint.Currency);

          if (rate.HasValue)
          {
            if (checkpoint.WasSended)
              ForSended(configuration, checkpoint, rate.Value);
            else
              ForNotSended(checkpoint, rate.Value);
          }
        }
      }
      catch (Exception e)
      {
        Trace.TraceError(e.Message + Environment.NewLine + e.StackTrace);
      }
      finally
      {
        rateProvider.Reset();
      }
    }

    private void ForSended(MonitorConfiguration configuration, Checkpoint checkpoint, double rate)
    {
      var checkpointValue = checkpoint.Value;

      if (checkpoint.Higher)
        checkpointValue -= configuration.Tolerance;
      else
        checkpointValue += configuration.Tolerance;

      checkpoint.WasSended = IsSuitable(rate, checkpoint.Higher, checkpointValue);
    }

    private void ForNotSended(Checkpoint checkpoint, double rate)
    {
      if (IsSuitable(rate, checkpoint.Higher, checkpoint.Value))
      {
        OnFound?.Invoke(this, (rate, checkpoint));

        checkpoint.WasSended = true;
      }
    }

    private bool IsSuitable(double rate, bool higher, double checkpoint)
      => higher
        ? rate >= checkpoint
        : rate <= checkpoint;
  }
}
