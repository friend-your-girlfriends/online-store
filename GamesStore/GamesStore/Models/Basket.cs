using System.Collections.Generic;

namespace GamesStore.Models
{
    public class Basket
    {
        public int BasketId { get; set; }
        public int GameId { get; set; }
        public float TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}