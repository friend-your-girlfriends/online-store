using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GamesStore.Models
{
    public class GameContext : IdentityDbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}