using GuessMyNumber.Models;
using GuessMyNumber.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;
using Moq;

namespace GuessMyNumber.Test
{
    public class GameServiceTests
    {
        readonly List<Game> MockedGames = new List<Game>()
        {
            new Game()
            {
                Id = new Guid("607f4f6a-d0b0-4e58-8673-09efb238c1d3"),
                TryCount = 10,
                NumberToGuess = 55,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 50, 00)
            },
            new Game()
            {
                Id = new Guid("bf9cfe3f-bfb2-4ec2-ac48-8f29ed1be8be"), // Not ended
                TryCount = 10,
                NumberToGuess = 55,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 25, 00)
            },
            new Game()
            {
                Id = new Guid("29c61380-29d6-46b5-9ad5-eba88c53173f"),
                TryCount = 3,
                NumberToGuess = 55,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 30, 00)
            },
            new Game()
            {
                Id =  new Guid("b1b81d6f-0985-45c9-8193-8dae3a9ae511"),
                TryCount = 2,
                NumberToGuess = 66,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 25, 59)
            },
            new Game()
            {
                Id =  new Guid("ad85d9b5-405e-4d0d-a85e-2d4c55d9093b"),
                TryCount = 99,
                NumberToGuess = 9,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 34, 00)
            },
            new Game()
            {
                Id = new Guid("d41fc0c6-e5c4-4e9b-a037-8d404bedf593"), // Not ended
                TryCount = 3,
                NumberToGuess = 9,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 00, 00)
            },
            new Game()
            {
                Id = new Guid("bdbffa0e-50fa-4d81-b3df-5c89a0a3758c"), // Not ended
                TryCount = 5,
                NumberToGuess = 51,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 10, 12, 00, 00)
            }
        };

        [Fact]
        public void CreateNewGame_AddNewGameToRepository_NewGameIsInRepository()
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);

            // act
            IGame game = service.CreateNewGame();
            IGame? gameFromRepository = gameRepository.GetGameById(game.GetId);

            // assert
            Assert.Equal(game, gameFromRepository);
        }

        [Theory]
        [InlineData("bdbffa0e-50fa-4d81-b3df-5c89a0a3758c")]
        [InlineData("ab3b5791-7ba2-4a8f-9cab-5db2150c4de0")]
        [InlineData("118f836f-3ff9-441a-9c8b-11ab96285a64")]
        public void GenerateToken_IfGenerateTokenWithGameId_GameIdIsInToken(string gameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);

            // act
            var principal = service.GenerateToken(gameId);
            var gameIdFromToken = principal?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // assert
            Assert.Equal(gameId, gameIdFromToken);
        }

        [Theory]
        [InlineData("bdbffa0e-50fa-4d81-b3df-5c89a0a3758c", "ab3b5791-7ba2-4a8f-9cab-5db2150c4de0")]
        [InlineData("ab3b5791-7ba2-4a8f-9cab-5db2150c4de0", "bdbffa0e-50fa-4d81-b3df-5c89a0a3758c")]
        [InlineData("118f836f-3ff9-441a-9c8b-11ab96285a64", "bdbffa0e-50fa-4d81-b3df-5c89a0a3758c")]
        public void GenerateToken_IfGameIdDiffrentThenExpected_OtherGameIdIsInToken(string gameId, string otherGameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);

            // act
            var principal = service.GenerateToken(gameId);
            var gameIdFromToken = principal?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // assert
            Assert.NotEqual(otherGameId, gameIdFromToken);
        }

        [Theory]
        [InlineData(10, "")]
        [InlineData(52, "3f62cac8-980d-4628-a6c5-6bd87335dd3a")]
        [InlineData(-52, "6e5bf07f-c5c4-4b84-9455-33b375480a76")]
        public void Guess_GameIsNull_ThrowsNullReferenceException(int guess, string gameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);
            MockedGames.ForEach(game => gameRepository.Add(game));

            // act
            Action action = () => service.Guess(guess, gameId);

            // assert
            Assert.Throws<NullReferenceException>(action);
        }

        [Theory]
        [InlineData(10, "bf9cfe3f-bfb2-4ec2-ac48-8f29ed1be8be")]
        [InlineData(1, "d41fc0c6-e5c4-4e9b-a037-8d404bedf593")]
        [InlineData(-52, "bdbffa0e-50fa-4d81-b3df-5c89a0a3758c")]
        public void Guess_GuessNumberLowerThenNumberToGuess_ReturnGameResultTooLow(int guess, string gameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);
            MockedGames.ForEach(game => gameRepository.Add(game));

            // act
            var gameResult = service.Guess(guess, gameId).GameResult;

            // assert
            Assert.Equal(Enums.GameResult.TooLow, gameResult);
        }

        [Theory]
        [InlineData(99, "bf9cfe3f-bfb2-4ec2-ac48-8f29ed1be8be")]
        [InlineData(89, "d41fc0c6-e5c4-4e9b-a037-8d404bedf593")]
        [InlineData(1000, "bdbffa0e-50fa-4d81-b3df-5c89a0a3758c")]
        public void Guess_GuessNumberBiggerThenNumberToGuess_ReturnGameResultTooBig(int guess, string gameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);
            MockedGames.ForEach(game => gameRepository.Add(game));

            // act
            var gameResult = service.Guess(guess, gameId).GameResult;

            // assert
            Assert.Equal(Enums.GameResult.TooBig, gameResult);
        }

        [Theory]
        [InlineData(55, "bf9cfe3f-bfb2-4ec2-ac48-8f29ed1be8be")]
        [InlineData(9, "d41fc0c6-e5c4-4e9b-a037-8d404bedf593")]
        [InlineData(51, "bdbffa0e-50fa-4d81-b3df-5c89a0a3758c")]
        public void Guess_GuessNumberEqualsNumberToGuess_ReturnGameResultWinner(int guess, string gameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);
            MockedGames.ForEach(game => gameRepository.Add(game));

            // act
            var gameResult = service.Guess(guess, gameId).GameResult;

            // assert
            Assert.Equal(Enums.GameResult.Winner, gameResult);
        }

        [Theory]
        [InlineData(15, "bf9cfe3f-bfb2-4ec2-ac48-8f29ed1be8be")]
        [InlineData(15, "d41fc0c6-e5c4-4e9b-a037-8d404bedf593")]
        [InlineData(33, "bdbffa0e-50fa-4d81-b3df-5c89a0a3758c")]
        public void Guess_GuessNumber_ReturnTryCountIncreasedByOne(int guess, string gameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);
            MockedGames.ForEach(game => gameRepository.Add(game));
            var tryCountExpected = gameRepository?.GetGameById(gameId)?.TryCount + 1;

            // act
            var tryCountAcctual = service.Guess(guess, gameId).TryCount;

            // assert
            Assert.Equal(tryCountExpected, tryCountAcctual);
        }

        [Theory]
        [InlineData(55, "bf9cfe3f-bfb2-4ec2-ac48-8f29ed1be8be")]
        [InlineData(9, "d41fc0c6-e5c4-4e9b-a037-8d404bedf593")]
        [InlineData(51, "bdbffa0e-50fa-4d81-b3df-5c89a0a3758c")]
        public void Guess_GuessNumberEqualsNumberToGuess_EndTimeGreatherThenStartTime(int guess, string gameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);
            MockedGames.ForEach(game => gameRepository.Add(game));
            var game = gameRepository.GetGameById(gameId);

            // act
            service.Guess(guess, gameId);

            // assert
            Assert.True(game?.EndDateTime > game?.StartDateTime);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void GetBestGames_IfGamesEnd_ReturnedBestGamesIncludeGames(int gamesCount)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            IGameService service = new GameService(gameRepository);
            MockedGames.ForEach(game => gameRepository.Add(game));

            // act
            var expected = MockedGames.Where(x => !x.IsPlaying)
                .OrderBy(x => x.TryCount)
                .ThenBy(x => x.PlayTime)
                .Take(gamesCount);

            var actual = service.GetBestGames(gamesCount);

            // assert
            Assert.Equal(expected.Count(), actual.Count());
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10)]
        public void GetBestGames_ForValidInputs_ReturnBestGames(int count)
        {
            // arrange
            var gameRepositoryMock = new Mock<IGameRepository>();

            gameRepositoryMock.Setup(x => x.GetBestGames(It.IsAny<int>())).Returns(MockedGames);

            var gameService = new GameService(gameRepositoryMock.Object);

            // act
            var bestGames = gameService.GetBestGames(count);

            //assert
            Assert.Equal(bestGames, MockedGames);
        }
    }
}
