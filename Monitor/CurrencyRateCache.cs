using Bitcoininfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor
{
  public class CurrencyRateCache : ICurrencyRateProvider
  {
    private readonly Dictionary<string, double?> cache = new Dictionary<string, double?>();

    public async Task<double?> GetRate(string currency) => cache.TryGetValue(currency, out var rate) 
      ? rate
      : cache[currency] = (await CoinRateInfo.GetActual(currency))?.Last;

    public void Reset() => cache.Clear();
  }
}
