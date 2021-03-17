using System;

namespace GamesStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string BasketId { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string SecondAddress { get; set; }
        public string ThirdAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public bool UseGiftWrapping { get; set; }
    }
}