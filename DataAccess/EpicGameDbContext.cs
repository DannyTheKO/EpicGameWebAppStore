using Microsoft.EntityFrameworkCore;
using EpicGameWebAppStore.Domain.Entities;

namespace EpicGameWebAppStore.DataAccess
{
    public class EpicGameDbContext : DbContext
    {
        public EpicGameDbContext(DbContextOptions<EpicGameDbContext> options) : base(options) { }

        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Account> Accounts { get; set; }
        // public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Game> Games { get; set; }
        /* public DbSet<CartDetail> CartDetails { get; set; } */
        public DbSet<Discount> Discounts { get; set; }
        /* public DbSet<AccountGame> AccountGames { get; set; } */
    }
}
