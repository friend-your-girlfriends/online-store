using System;
using System.ComponentModel.DataAnnotations;

namespace GamesStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string BasketId { get; set; }
        [Required]
        [StringLength(30,MinimumLength = 3, ErrorMessage = "Длина имени должна составлять от 3 до 30 символов.")]
        public string UserName { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 10, ErrorMessage = "Длина первого адреса должна составлять от 10 до 100 символов.")]
        public string Address { get; set; }
        [StringLength(100,MinimumLength = 10, ErrorMessage = "Длина второго адреса должна составлять от 10 до 100 символов.")]
        public string SecondAddress { get; set; }
        [StringLength(100,MinimumLength = 10, ErrorMessage = "Длина третьего адреса должна составлять от 10 до 100 символов.")]
        public string ThirdAddress { get; set; }
        [Required]
        [StringLength(30,MinimumLength = 3, ErrorMessage = "Длина города должна составлять от 10 до 100 символов.")]
        public string City { get; set; }
        [Required]
        [StringLength(30,MinimumLength = 3, ErrorMessage = "Длина страны должна составлять от 10 до 100 символов.")]
        public string Country { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public float Amount { get; set; }
        public bool UseGiftWrapping { get; set; }
    }
}