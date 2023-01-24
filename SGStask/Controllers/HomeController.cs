using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SGStask.Models;

namespace SGStask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly int _itemsOnPage = 6;
        private readonly IMemoryCache _memoryCache;
        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("currencies")]
        public IActionResult GetCurrencies(int currentPage = 1)
        {
            try
            {
                var result = TryGetAllCurrencies().Valute.Skip((currentPage - 1) * _itemsOnPage).Take(_itemsOnPage);

                if (result.Any())
                    return Ok(result);
                else
                    return BadRequest(new { Error = "Превышено возможное количество станиц."});
            }
            catch(Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("currency/{id}")]
        public IActionResult GetCurrency(string id)
        {
            try
            {
                var result = TryGetAllCurrencies().Valute.FirstOrDefault(v => v.ID == id);

                if (result == null)
                    return BadRequest(new { Error = "Данные по валюте не найдены." });
                else
                    return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        private Root TryGetAllCurrencies()
        {
            if (!_memoryCache.TryGetValue(MemoryCacheKeys.CurrencyKey, out Root model))
            {
                throw new Exception("Данные о курсах валют не найдены. Попробуйте позже.");
            }

            return model;
        }
    }
}
