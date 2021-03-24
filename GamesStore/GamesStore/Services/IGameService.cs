using GamesStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesStore.Services
{
    public interface IGameService
    {
        Task<List<Category>> CategoryInfoAsync();
        Task<List<Game>> GameInfoAsync();
        Task<List<Game>> CategoryGamesInfoAsync(string categoryName);
    }
    public class GameService : IGameService
    {
        private GameContext _db = new GameContext();

        public Task<List<Game>> CategoryGamesInfoAsync(string categoryName)
        {
            int categoryId = 0;

            foreach (var category in _db.Categories)
            {
                if (category.Name.ToLower() == categoryName.ToLower())
                {
                    categoryId = category.CategoryId;
                    break;
                }
            }

            var games = _db.Games.Where(game => game.CategoryId == categoryId).ToListAsync();

            return games;
        }

        public async Task<List<Category>> CategoryInfoAsync()
        {
            var categories = await _db.Categories.ToListAsync();

            return categories;
        }

        public async Task<List<Game>> GameInfoAsync()
        {
            var games = await _db.Games.ToListAsync();

            return games;
        }
    }
}
