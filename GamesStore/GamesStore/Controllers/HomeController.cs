using GamesStore.Models;
using GamesStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using PagedList;
using System;

namespace GamesStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IGameService _gameService = new GameService();
        private static string _basketId;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //GET Home/Index?category
        public async Task<IActionResult> Index(int page = 1, string category = "")
        {
            int pageSize = 2;
            var categories = await _gameService.CategoryInfoAsync();
            var games = await _gameService.GameInfoAsync();

            List<Game> categoryGames = null;

            if (!string.IsNullOrEmpty(category))
                categoryGames = await _gameService.CategoryGamesInfoAsync(category);

            ViewBag.Categories = categories;

            if (categoryGames != null)
            {
                ViewBag.Games = categoryGames;
            }
            else
            {
                ViewBag.Games = games;
            }

            return View(games.ToPagedList(page, pageSize));
        }

        private void CreateCookie()
        {
            if (_basketId != null)
                return;

            if (Request.Cookies["BasketId"] == null)
            {
                _basketId = Guid.NewGuid().ToString();
                Response.Cookies.Append("BasketId", _basketId);
            }
            else
            {
                _basketId = Request.Cookies["BasketId"];
            }
        }
        //GET Home/Basket
        [HttpGet]
        public IActionResult Basket()
        {
            CreateCookie();

            return View();
        }

        //POST Home/Basket?gameId
        [HttpPost]
        public async Task<IActionResult> Basket(int gameId)
        {
            CreateCookie();

            _gameService.AddToBasketGame(gameId, _basketId);

            var games = await _gameService.GameInBasketInfoAsync(_basketId);

            ViewBag.Games = games;

            return View();

        }

        [HttpGet]
        public IActionResult Order()
        {


            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
