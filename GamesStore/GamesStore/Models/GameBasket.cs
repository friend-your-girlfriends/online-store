using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesStore.Models
{
    public class GameBasket
    {
        public int GameBasketId { get; set; }
        public string BasketId { get; set; }
        public int GameId { get; set; }
        public int Quantity { get; set; }
    }
}
