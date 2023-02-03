using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SGStask.Models;
using System.Globalization;

namespace SGStask.Services
{
    public class CurrencyUpdateService : BackgroundService
    {
        private readonly int _updateDelay = 1800000; //12 hours in milliseconds
        private readonly int _storeTime = 1; // 1 day
        private readonly string _requestUrl = "https://www.cbr-xml-daily.ru/daily_json.js";
        private readonly IMemoryCache _memoryCache;
        
        public CurrencyUpdateService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

                    HttpClient client = new HttpClient();

                    var httpResponseMessage = await client.GetAsync(_requestUrl);
                    string jsonResponce = await httpResponseMessage.Content.ReadAsStringAsync();

                    var currencies = JsonConvert.DeserializeObject<Root>(jsonResponce);

                    _memoryCache.Set(MemoryCacheKeys.CurrencyKey, currencies, TimeSpan.FromDays(_storeTime));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(_updateDelay, stoppingToken);
            }
        }
    }
}
