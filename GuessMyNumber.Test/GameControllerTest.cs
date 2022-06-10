using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using GuessMyNumber.Services;
using GuessMyNumber.Controllers;
using Microsoft.AspNetCore.Mvc;
using GuessMyNumber.Models;
using System.Security.Claims;
using HttpContextMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GuessMyNumber.Test
{
    public class GameControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        readonly List<Game> MockedGames = new List<Game>()
        {
            new Game()
            {
                Id = new Guid("607f4f6a-d0b0-4e58-8673-09efb238c1d3"),
                TryCount = 10,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 50, 00)
            },
            new Game()
            {
                Id = new Guid("bf9cfe3f-bfb2-4ec2-ac48-8f29ed1be8be"),
                TryCount = 10,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 25, 55)
            },
            new Game()
            {
                Id = new Guid("29c61380-29d6-46b5-9ad5-eba88c53173f"),
                TryCount = 3,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 30, 00)
            },
        };
        private readonly WebApplicationFactory<Program> factory;

        public GameControllerTest(WebApplicationFactory<Program> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // arrange
            var serviceMock = new Mock<IGameService>();
            var gameController = new GameController(serviceMock.Object);

            // act
            var result = gameController.Index();

            // assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Guess_CreateNewGame_ContainsCookieGamesCookieAboutGame()
        {
            var appliation = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => { });

            var client = appliation.CreateClient();
            var resposne = await client.GetAsync("/Game/Guess");

            var containsGamesCookie = resposne.Headers.FirstOrDefault(x => x.Key == "Set-Cookie").Value?
                                                      .Any(x => x.StartsWith("Games.Cookie"));

            Assert.Equal(System.Net.HttpStatusCode.OK, resposne.StatusCode);
            Assert.True(containsGamesCookie);
        }

        [Fact]
        public void HighScores_ThereIsNoGame_RetrnsEmptyList()
        {
            // arrange
            var serviceMock = new Mock<IGameService>();
            var games = new List<Game>();
            serviceMock.Setup(x => x.GetBestGames(It.IsAny<int>())).Returns(games);

            var gameController = new GameController(serviceMock.Object);

            // act
            var result = gameController.HighScores();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<IGame>>(viewResult.ViewData.Model);
            Assert.True(model.Count().Equals(0));
        }

        [Fact]
        public void HighScores_ThereAreFinishedGame_RetrunsBestGamesList()
        {
            // arrange
            var serviceMock = new Mock<IGameService>();
            serviceMock.Setup(x => x.GetBestGames(It.IsAny<int>())).Returns(MockedGames);

            var gameController = new GameController(serviceMock.Object);

            // act
            var result = gameController.HighScores();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<IGame>>(viewResult.ViewData.Model);
            Assert.Equal(MockedGames.Count(), model.Count());
        }
    }
}
