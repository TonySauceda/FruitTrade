using FruitTrade.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FruitTrade.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allTrades = _tradeService.GetTrades();

            return Ok(allTrades);
        }

        [HttpGet("GetByCountry/{country}")]
        public IActionResult GetByCountry(string country)
        {
            var allTrades = _tradeService.GetTradesByCountry(country);

            return Ok(allTrades);
        }

        [HttpGet("GetByCommodity/{commodity}")]
        public IActionResult GetByCommodity(string commodity)
        {
            var allTrades = _tradeService.GetTradesByCommodity(commodity);

            return Ok(allTrades);
        }

        [HttpGet("CalculateTrade")]
        public IActionResult CalculateTrade([FromQuery] string commodity, [FromQuery] decimal price, [FromQuery] decimal tons = 1)
        {
            var trades = _tradeService.CalculateTrade(commodity, price, tons);

            return Ok(trades);
        }
    }
}
