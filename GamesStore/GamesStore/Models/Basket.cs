using System.Collections.Generic;

namespace GamesStore.Models
{
    public class Basket
    {
        public string BasketId { get; set; }
        public float TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}