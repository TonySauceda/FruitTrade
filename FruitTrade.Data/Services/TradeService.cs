using FruitTrade.Core.Models;
using FruitTrade.Core.Responses;
using FruitTrade.Data.Settings;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FruitTrade.Data.Services
{
    public class TradeService : ITradeService
    {
        private readonly FileSettings fileSettings;

        public TradeService(FileSettings fileSettings)
        {
            this.fileSettings = fileSettings;
        }
        public List<TradeResponse> CalculateTrade(string commodity, decimal price, decimal tons)
        {
            var data = GetDataFromFile();

            var dataFiltered = data
                .Where(x =>
                    x.Commodity.ToUpper() == commodity.ToUpper() &&
                    (price <= 0 || x.Variable_Cost == price))
                .ToList();

            var result = dataFiltered.Select(x => new TradeResponse
            {
                Country = x.Country,
                Total_Cost = x.Variable_Cost * tons + x.Trade_Cost,
                Trade_Cost = x.Trade_Cost,
                Variable_Cost = x.Variable_Cost
            })
            .OrderByDescending(x => x.Total_Cost)
            .ToList();

            return result;
        }

        public List<TradeModel> GetTrades()
        {
            return GetDataFromFile();
        }

        public List<TradeModel> GetTradesByCommodity(string commodity)
        {
            var data = GetDataFromFile();

            var result = data
                .Where(x => x.Commodity.ToUpper() == commodity.ToUpper())
                .ToList();

            return result;
        }

        public List<TradeModel> GetTradesByCountry(string country)
        {
            var data = GetDataFromFile();

            var result = data
                .Where(x => x.Country.ToUpper() == country.ToUpper())
                .ToList();

            return result;
        }

        private List<TradeModel> GetDataFromFile()
        {
            if (!File.Exists(fileSettings.Path))
                return new List<TradeModel>();

            return JsonConvert.DeserializeObject<List<TradeModel>>(File.ReadAllText(fileSettings.Path));
        }
    }
}
