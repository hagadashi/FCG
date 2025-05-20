using FCG.Domain.Entities.Users;
using System;

namespace FCG.Domain.Entities.Games
{
    public class Library : Entity
    {
        public DateTime PurchasedAt { get; private set; }
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }
        public Guid GameId { get; private set; }
        public virtual Game Game { get; private set; }

        protected Library() { }

        public Library(User user, Game game)
        {
            PurchasedAt = DateTime.UtcNow;
            User = user ?? throw new ArgumentNullException(nameof(user));
            UserId = user.Id;
            Game = game ?? throw new ArgumentNullException(nameof(game));
            GameId = game.Id;
        }
    }
}