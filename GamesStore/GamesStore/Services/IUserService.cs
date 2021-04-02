using GamesStore.Models;
using GamesStore.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesStore.Services
{
    public interface IUserService
    {
        Task<List<Category>> CategoryInfoAsync();
        Task<List<Game>> GameInfoAsync();
        Task<List<Game>> CategoryGamesInfoAsync(string categoryName);
        IQueryable<GameBasketView> GameInBasketInfoAsync(string basketId);
        void AddOrder(Order order);
        Task<List<int>> QuantityGamesAsync(string basketId);
        float GetTotalPriceAsync();
        float GetGamePrice();
        void AddToBasketGame(int gameId, string basketId);
        void CreateBasket(string cookieValue);
    }
    public class UserService : IUserService
    {
        private GameContext _db = new GameContext();

        public void CreateBasket(string basketId)
        {
            Basket basket = new Basket();
            basket.BasketId = basketId;

            _db.Baskets.Add(basket);
            _db.SaveChanges();
        }

        public void AddToBasketGame(int gameId, string basketId)
        {
            CreateOrUpdateGameBasket(gameId, basketId);

            CreateOrUpdateBasket(basketId);

        }

        private void CreateOrUpdateGameBasket(int gameId, string basketId)
        {
            var gameBasket = new GameBasket();

            gameBasket.BasketId = basketId;
            gameBasket.GameId = gameId;
            gameBasket.Quantity = 1;

            if (_db.GameBaskets.Count() == 0)
            {
                _db.GameBaskets.Add(gameBasket);
                _db.SaveChanges();
            }

            var gameBasketList = _db.GameBaskets.Where(g => g.BasketId == basketId).ToList();

            if (gameBasketList.Count != 0)
            {
                foreach (var dbGameBasket in _db.GameBaskets)
                {
                    if (dbGameBasket.GameId == gameBasket.GameId && dbGameBasket.BasketId == gameBasket.BasketId)
                    {
                        gameBasket.Quantity += dbGameBasket.Quantity;
                        _db.GameBaskets.Update(gameBasket);
                    }
                }
            }
            else
            {
                _db.GameBaskets.Add(gameBasket);
            }

            _db.SaveChanges();
        }

        private void CreateOrUpdateBasket(string basketId)
        {
            var basket = new Basket();

            // basket.TotalPrice = CalculateTotalPrice(basketId);

            if (_db.Baskets.Count() == 0)
            {
                basket.BasketId = basketId;
                _db.Baskets.Add(basket);
                _db.SaveChanges();
            }

            var basketList = _db.Baskets.Where(g => g.BasketId == basketId).ToList();

            if (basketList.Count != 0)
            {
                foreach (var dbBasket in _db.Baskets)
                {
                    if (dbBasket.BasketId == basket.BasketId)
                    {
                        _db.Baskets.Update(basket);
                    }
                }
            }
            else
            {
                basket.BasketId = basketId;
                _db.Baskets.Add(basket);
            }

            _db.SaveChanges();
        }

        private IQueryable CalculateTotalPrice(string basketId)
        {
            /* SELECT SUM(Price) FROM Games INNER JOIN GameBaskets ON GameBaskets.GameId = Games.GameId 
                                 WHERE GameBaskets.BasketId = basketId; */
            var result = _db.Games.Join(_db.GameBaskets.Where(p => p.BasketId == basketId),
                p => p.GameId,
                i => i.GameId,
                (p, c) => new 
                { 
                 TotalPrice = p.Price,
                });

            return result;
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

        public IQueryable<GameBasketView> GameInBasketInfoAsync(string basketId)
        {
            /* SELECT * FROM Games INNER JOIN GameBaskets ON GameBaskets.GameId = Games.GameId 
                     WHERE GameBaskets.BasketId = basketId; */
            var result = _db.Games.Join(_db.GameBaskets.Where(p => p.BasketId == basketId),
                            p => p.GameId,
                            i => i.GameId,
                            (p, i) => new GameBasketView
                            {
                                Name = p.Name,
                                Price = p.Price,
                                Quantity = i.Quantity
                            });

            return result;
        }

        public float GetTotalPriceAsync()
        {
            throw new NotImplementedException();
        }

        public float GetGamePrice()
        {
            throw new NotImplementedException();
        }

        public async Task<List<int>> QuantityGamesAsync(string basketId)
        {
            var gameBasket = _db.GameBaskets.Where(gameBasket => gameBasket.BasketId == basketId);

            var quantiy = await gameBasket.Select(g => g.Quantity).ToListAsync();

            return quantiy;
        }

        public void AddOrder(Order order)
        {
            _db.Orders.Add(order);
        }
    }
}