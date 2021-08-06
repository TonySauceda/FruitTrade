using FruitTrade.Core.Models;
using FruitTrade.Core.Responses;
using FruitTrade.Data.Services;
using FruitTrade.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FruitTrade.UnitTests
{
    public class TradeControllerTests
    {
        [Fact]
        public void Get_ListOfTradeModel()
        {
            //Arrange
            var mockService = new Mock<ITradeService>();
            mockService
                .Setup(serv => serv.GetTrades())
                .Returns(GetTestTrades());

            var controller = new TradeController(mockService.Object);

            //Act
            var actionResult = controller.Get() as OkObjectResult;

            //Assert
            var result = actionResult.Value as IEnumerable<TradeModel>;

            Assert.Equal(GetTestTrades().Count, result.Count());
        }

        [Fact]
        public void GetByCountry_ListOfTradesModel()
        {
            //Arrange
            string country = "mx";
            var mockService = new Mock<ITradeService>();
            mockService
                .Setup(serv => serv.GetTradesByCountry(country))
                .Returns(GetTestTrades().Where(x => x.Country == country).ToList());

            var controller = new TradeController(mockService.Object);

            //Act
            var actionResult = controller.GetByCountry(country) as OkObjectResult;

            //Assert
            var result = actionResult.Value as IEnumerable<TradeModel>;
            int correctCount = GetTestTrades().Where(x => x.Country == country).ToList().Count;
            Assert.Equal(correctCount, result.Count());
        }

        [Fact]
        public void GetByCommodity_ListOfTradesModel()
        {
            //Arrange
            string commodity = "raspberry";
            var mockService = new Mock<ITradeService>();
            mockService
                .Setup(serv => serv.GetTradesByCommodity(commodity))
                .Returns(GetTestTrades().Where(x => x.Commodity == commodity).ToList());

            var controller = new TradeController(mockService.Object);

            //Act
            var actionResult = controller.GetByCommodity(commodity) as OkObjectResult;

            //Assert
            var result = actionResult.Value as IEnumerable<TradeModel>;
            int correctCount = GetTestTrades().Where(x => x.Commodity == commodity).ToList().Count;
            Assert.Equal(correctCount, result.Count());
        }

        [Fact]
        public void CalculateTrade_ListOfTradeResponse()
        {
            //Arrange
            string commodity = "mango";
            decimal price = 50;
            decimal tons = 345;
            var mockService = new Mock<ITradeService>();

            var data = GetTestTrades();
            var dataFiltered = data
            .Where(x =>
                x.Commodity == commodity &&
                (price <= 0 || x.Variable_Cost == price))
            .ToList();

            var tradeResponse = dataFiltered.Select(x => new TradeResponse
            {
                Country = x.Country,
                Total_Cost = x.Variable_Cost * tons + x.Trade_Cost,
                Trade_Cost = x.Trade_Cost,
                Variable_Cost = x.Variable_Cost
            })
            .OrderByDescending(x => x.Total_Cost)
            .ToList();

            mockService
                .Setup(serv => serv.CalculateTrade(commodity, price, tons))
                .Returns(tradeResponse);

            var controller = new TradeController(mockService.Object);

            //Act
            var actionResult = controller.CalculateTrade(commodity, price, tons) as OkObjectResult;

            //Assert
            var result = actionResult.Value as IEnumerable<TradeResponse>;
            Assert.Equal(tradeResponse.Count, result.Count());
        }


        private List<TradeModel> GetTestTrades()
        {
            var result = new List<TradeModel>
            {
                new TradeModel { Commodity = "mango", Country = "mx", Trade_Cost = 20, Variable_Cost = 50 },
                new TradeModel { Commodity = "mango", Country = "br", Trade_Cost = 33, Variable_Cost = 55 },
                new TradeModel { Commodity = "raspberry", Country = "mx", Trade_Cost = 20, Variable_Cost = 34.1m },
                new TradeModel { Commodity = "raspberry", Country = "pa", Trade_Cost = 28, Variable_Cost = 35 },
                new TradeModel { Commodity = "mango", Country = "pa", Trade_Cost = 19, Variable_Cost = 51.1m }
            };

            return result;
        }
    }
}
