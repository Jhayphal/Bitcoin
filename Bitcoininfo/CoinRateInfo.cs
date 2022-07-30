using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bitcoininfo
{
  public static class CoinRateInfo
  {
    private readonly static Lazy<HttpClient> client = new Lazy<HttpClient>();

    public static async Task<BitcoinExchangeRates> GetActual(string currency)
    {
      var allRates = await GetActual();

      return allRates.TryGetValue(currency.ToUpper(), out var rate) ? rate : null;
    }

    public static async Task<Dictionary<string, BitcoinExchangeRates>> GetActual() => await GetActual(client.Value);

    public static async Task<Dictionary<string, BitcoinExchangeRates>> GetActual(HttpClient client)
    {
      var jsonResponse = await client.GetStringAsync("https://blockchain.info/ru/ticker"); // https://www.blockchain.com/ru/api/exchange_rates_api
      var bitcoinExchangeRates = BitcoinExchangeRates.FromJson(jsonResponse);

      return bitcoinExchangeRates ?? new Dictionary<string, BitcoinExchangeRates>();
    }
  }
}
