using GuessMyNumber.Models;
using GuessMyNumber.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GuessMyNumber.Test
{
    public class GameRepositoryTests
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
                Id = new Guid("bf9cfe3f-bfb2-4ec2-ac48-8f29ed1be8be"), // Not ended
                TryCount = 10,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 25, 00)
            },
            new Game()
            {
                Id = new Guid("29c61380-29d6-46b5-9ad5-eba88c53173f"),
                TryCount = 3,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 30, 00)
            },
            new Game()
            {
                Id =  new Guid("b1b81d6f-0985-45c9-8193-8dae3a9ae511"),
                TryCount = 2,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 25, 59)
            },
            new Game()
            {
                Id =  new Guid("ad85d9b5-405e-4d0d-a85e-2d4c55d9093b"),
                TryCount = 99,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 34, 00)
            },
            new Game()
            {
                Id = new Guid("d41fc0c6-e5c4-4e9b-a037-8d404bedf593"), // Not ended
                TryCount = 3,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 11, 12, 00, 00)
            },
            new Game()
            {
                Id = new Guid("bdbffa0e-50fa-4d81-b3df-5c89a0a3758c"), // Not ended
                TryCount = 5,
                StartDateTime = new DateTime(2020, 11, 11, 12, 25, 00),
                EndDateTime = new DateTime(2020, 11, 10, 12, 00, 00)
            }
        };

        [Theory]
        [InlineData(5)]
        [InlineData(40)]
        [InlineData(72)]
        [InlineData(99)]
        public void Add_AddGameToRepository_GameIsRepository(int numberToGuess)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            var game = Game.CreateGame(numberToGuess);

            // act
            gameRepository.Add(game);
            var gameInRepository = gameRepository.GetGameById(game.GetId);

            // assert
            Assert.Equal(game, gameInRepository);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        public void GetBestGames_IfGamesEnd_ReturnedBestGamesIncludeGames(int gamesCount)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            MockedGames.ForEach(game => gameRepository.Add(game));

            // act
            var bestGames = MockedGames.Where(x => !x.IsPlaying)
                .OrderBy(x => x.TryCount)
                .ThenBy(x => x.PlayTime)
                .Take(gamesCount);

            var bestGamesInRepository = gameRepository.GetBestGames(gamesCount);

            // assert
            Assert.Equal(bestGames.Count(), bestGamesInRepository.Count());
            Assert.Equal(bestGames, bestGamesInRepository);
        }

        [Theory]
        [InlineData("607f4f6a-d0b0-4e58-8673-09efb238c1d3")]
        [InlineData("29c61380-29d6-46b5-9ad5-eba88c53173f")]
        [InlineData("b1b81d6f-0985-45c9-8193-8dae3a9ae511")]
        [InlineData("ad85d9b5-405e-4d0d-a85e-2d4c55d9093b")]
        public void GetBestGames_IfGameEnd_ReturnedBestGamesIncludeGame(string endedGameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            MockedGames.ForEach(game => gameRepository.Add(game));

            // act
            var bestGamesFromRepository = gameRepository.GetBestGames(MockedGames.Count());
            bool isEndedGameInBestGames = bestGamesFromRepository.Any(x => x.GetId.Equals(endedGameId));

            // assert
            Assert.True(isEndedGameInBestGames);
        }

        [Theory]
        [InlineData("bf9cfe3f-bfb2-4ec2-ac48-8f29ed1be8be")]
        [InlineData("d41fc0c6-e5c4-4e9b-a037-8d404bedf593")]
        [InlineData("bdbffa0e-50fa-4d81-b3df-5c89a0a3758c")]
        public void GetBestGames_IfGameNotEnd_ReturnedBestGamesNotIncludeGame(string notEndedGameId)
        {
            // arrange
            IGameRepository gameRepository = new GameRepository();
            MockedGames.ForEach(game => gameRepository.Add(game));

            // act
            var bestGamesFromRepository = gameRepository.GetBestGames(MockedGames.Count());
            bool isNotEndedGameInBestGames = bestGamesFromRepository.Any(x => x.GetId.Equals(notEndedGameId));

            // assert
            Assert.False(isNotEndedGameInBestGames);
        }

    }
}
