using FCG.Domain.Entities.Games;
using FCG.Domain.Entities.Users;

namespace FCG.Infrastructure.Data.Migrations
{
    public static class DbInitializer
    {
        public static async Task SeedDataAsync(AppDbContext context)
        {
            // Verifica se o contexto já tem dados
            if (context.Roles.Any())
            {
                return; // Banco de dados já possui dados iniciais
            }

            // Cria roles iniciais
            var adminRole = new Role("Admin", "Administrador do sistema", false);
            var userRole = new Role("User", "Usuário padrão", true);

            await context.Roles.AddRangeAsync(adminRole, userRole);
            await context.SaveChangesAsync();
            
            var adminUser = new User(
                username: "admin",
                email: "admin@fcg.com",
                passwordHash: "AQAAAAIAAYagAAAAELuZqr6Xe4dIFvy6d9sTTwAgvS6qGTyKLA1b5tcz5QoLQavXCzcSEZ4w7SpJADkYYA==", // "Admin@123"
                firstName: "Admin",
                lastName: "FCG",
                role: adminRole
            );

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            // Criar categorias iniciais
            var categories = new List<Category>
            {
                new Category("Ação", "Jogos de ação e aventura"),
                new Category("Estratégia", "Jogos de estratégia e simulação"),
                new Category("RPG", "Jogos de interpretação de papéis"),
                new Category("Esporte", "Jogos de esportes e competições"),
                new Category("Corrida", "Jogos de corrida e velocidade")
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Criar jogos iniciais
            var games = new List<Game>
            {
                new Game(
                    title: "Super Adventure",
                    description: "Um jogo de aventura emocionante onde você explora mundo fantásticos.",
                    price: 59.99M,
                    imageUrl: "https://placeholder.com/games/super-adventure.jpg",
                    category: categories[0]
                ),
                new Game(
                    title: "Strategy Master",
                    description: "Domine o campo de batalha com estratégia e tática.",
                    price: 49.99M,
                    imageUrl: "https://placeholder.com/games/strategy-master.jpg",
                    category: categories[1]
                ),
                new Game(
                    title: "Fantasy RPG",
                    description: "Role-playing game com um mundo vasto para explorar.",
                    price: 69.99M,
                    imageUrl: "https://placeholder.com/games/fantasy-rpg.jpg",
                    category: categories[2]
                ),
                new Game(
                    title: "Soccer Pro",
                    description: "Jogo de futebol realista com times oficiais.",
                    price: 39.99M,
                    imageUrl: "https://placeholder.com/games/soccer-pro.jpg",
                    category: categories[3]
                ),
                new Game(
                    title: "Speed Racers",
                    description: "Corridas de alta velocidade em pistas incríveis.",
                    price: 29.99M,
                    imageUrl: "https://placeholder.com/games/speed-racers.jpg",
                    category: categories[4]
                )
            };

            await context.Games.AddRangeAsync(games);
            await context.SaveChangesAsync();

            // Criar uma promoção inicial
            var sale = new Sale(
                name: "Promoção de Lançamento",
                description: "Desconto especial para os primeiros compradores",
                discountPercentage: 20,
                startDate: DateTime.UtcNow.AddDays(-1),
                endDate: DateTime.UtcNow.AddDays(30),
                game: games[0],
                createdByUser: adminUser
            );

            await context.Sales.AddAsync(sale);
            await context.SaveChangesAsync();
        }
    }
}