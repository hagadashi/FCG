using FCG.Domain.Entities.Games;
using FCG.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todas as configurações de entidades definidas na assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Configurações de chave estrangeira e relacionamentos

            // User -> Role (N:1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Session -> User (N:1)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Game -> Category (N:1)
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Category)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Library -> User (N:1)
            modelBuilder.Entity<Library>()
                .HasOne(l => l.User)
                .WithMany(u => u.Libraries)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Library -> Game (N:1)
            modelBuilder.Entity<Library>()
                .HasOne(l => l.Game)
                .WithMany(g => g.Libraries)
                .HasForeignKey(l => l.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            // Sale -> Game (N:1)
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Game)
                .WithMany(g => g.Sales)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // Sale -> User (N:1)
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.CreatedByUser)
                .WithMany(u => u.CreatedSales)
                .HasForeignKey(s => s.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }

}