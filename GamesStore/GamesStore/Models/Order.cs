using System;

namespace GamesStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int BasketId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public bool UseGiftWrapping { get; set; }
    }
}