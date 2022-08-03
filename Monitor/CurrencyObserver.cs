using System;
using System.Threading.Tasks;

namespace Monitor
{
  public sealed class CurrencyObserver
  {
    private readonly ICurrencyRateProvider rateProvider;

    public event EventHandler<(double, Checkpoint)> OnFound;

    public CurrencyObserver(ICurrencyRateProvider rateProvider)
      => this.rateProvider = rateProvider ?? throw new ArgumentNullException(nameof(rateProvider));

    public async Task Observe(MonitorConfiguration configuration)
    {
      try
      {
        foreach (var checkpoint in configuration.Checkpoints)
        {
          var rate = await rateProvider.GetRate(checkpoint.Currency);

          if (rate.HasValue && IsSuitable(rate.Value, checkpoint))
            OnFound?.Invoke(this, (rate.Value, checkpoint));
        }
      }
      finally
      {
        rateProvider.Reset();
      }
    }

    private bool IsSuitable(double rate, Checkpoint checkpoint) => checkpoint.Higher
      ? rate >= checkpoint.Value 
      : rate <= checkpoint.Value;
  }
}
