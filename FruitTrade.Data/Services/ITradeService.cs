using FruitTrade.Core.Models;
using FruitTrade.Core.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FruitTrade.Data.Services
{
    public interface ITradeService
    {
        List<TradeResponse> CalculateTrade(string commodity, decimal price, decimal tons);
        List<TradeModel> GetTrades();
        List<TradeModel> GetTradesByCountry(string country);
        List<TradeModel> GetTradesByCommodity(string commodity);
    }
}
