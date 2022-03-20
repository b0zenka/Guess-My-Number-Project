using GuessMyNumber.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GuessMyNumber.Services
{
    public class GameService : IGameService
    {
        private const int MIN_VALUE = 1;
        private const int MAX_VALUE = 100;

        private readonly AuthenticationSettings authenticationSettings;
        private readonly IGameRepository gameRepository;
        private readonly Random random;

        public GameService(AuthenticationSettings authenticationSettings, IGameRepository gameRepository)
        {
            this.authenticationSettings = authenticationSettings;
            this.gameRepository = gameRepository;
            this.random = new Random();
        }

        public Game CreateNewGame()
        {
            int numberToGuess = random.Next(MIN_VALUE, MAX_VALUE + 1);
            var game = Game.CreateGame(numberToGuess);

            gameRepository.Add(game);

            return game;
        }

        public string GenerateToken(string gameId)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, gameId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);  
        }
    }
}
