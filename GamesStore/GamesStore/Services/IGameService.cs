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
        Task<List<Game>> GameInBasketInfoAsync(string basketId);
        float GetTotalPriceAsync();
        float GetGamePrice();
        void AddToBasketGame(int gameId, string basketId);
        void CreateBasket(string cookieValue);
    }
    public class GameService : IGameService
    {
        private GameContext _db = new GameContext();
        private GameBasket _gameBasket = new GameBasket();

        public void CreateBasket(string basketId)
        {
            Basket basket = new Basket();
            basket.BasketId = basketId;

            _db.Baskets.Add(basket);
            _db.SaveChanges();
        }

        public void AddToBasketGame(int gameId, string basketId)
        {
            _gameBasket.GameBasketId = _db.GameBaskets.Count() + 1;
            _gameBasket.BasketId = basketId;
            _gameBasket.GameId = gameId;

            if (_db.GameBaskets.Count() == 0)
            {
                _db.GameBaskets.Add(_gameBasket);
                _db.SaveChanges();
            }

            foreach (var dbGameBaket in _db.GameBaskets)
            {
                if (dbGameBaket.BasketId == basketId)
                {
                    _db.GameBaskets.Update(_gameBasket);
                }
                else
                {
                    _db.GameBaskets.Add(_gameBasket);
                }
                break;
            }

            var basket = new Basket();

            basket.BasketId = basketId;
            basket.TotalPrice = CalculateTotalPrice(basketId);

            if (_db.Baskets.Count() == 0)
            {
                _db.Baskets.Add(basket);
                _db.SaveChanges();
            }

            foreach (var dbBasket in _db.Baskets)
            {
                if (dbBasket.BasketId == basketId)
                {
                    _db.Baskets.Update(basket);
                }
                else
                {
                    _db.Baskets.Add(basket);
                }
                break;
            }

            _db.SaveChanges();
        }

        private float CalculateTotalPrice(string basketId)
        {
            /* SELECT SUM(Price) FROM Games INNER JOIN GameBaskets ON GameBaskets.GameId = Games.GameId 
                                 WHERE GameBaskets.BasketId = basketId; */
            return _db.Games.Join(_db.GameBaskets.Where(p => p.BasketId == basketId),
                p => p.GameId,
                i => i.GameId,
                (p, c) => p.Price).Sum();
        }

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

            /* SELECT * FROM Games WHERE Games.CategoryId = categoryId */
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

        public async Task<List<Game>> GameInBasketInfoAsync(string basketId)
        {
            /* SELECT * FROM GameBaskets WHERE GameBaskets.BasketId = basketId */
            var gameBasket = _db.GameBaskets.Where(gameBasket => gameBasket.BasketId == basketId);

            /* SELECT * FROM Games WHERE Games.GameId = (SELECT GameId FROM GameBaskets WHERE GameBaskets.BasketId = basketId) */
            var games = await _db.Games.Where(game => game.GameId == gameBasket.Select(s => s.GameId).FirstOrDefault()).ToListAsync();

            return games;
        }

        public float GetTotalPriceAsync()
        {
            throw new NotImplementedException();
        }

        public float GetGamePrice()
        {
            throw new NotImplementedException();
        }
    }
}