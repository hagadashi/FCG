using FCG.Domain.Entities.Users;
using System;

namespace FCG.Domain.Entities.Games
{
    public class Sale : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal DiscountPercentage { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsActive { get; private set; }
        public Guid GameId { get; private set; }
        public virtual Game Game { get; private set; }
        public Guid CreatedByUserId { get; private set; }
        public virtual User CreatedByUser { get; private set; }

        protected Sale() { }

        public Sale(string name, string description, decimal discountPercentage,
                    DateTime startDate, DateTime endDate, Game game, User createdByUser)
        {
            if (discountPercentage <= 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercentage));

            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            DiscountPercentage = discountPercentage;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = true;
            Game = game ?? throw new ArgumentNullException(nameof(game));
            GameId = game.Id;
            CreatedByUser = createdByUser ?? throw new ArgumentNullException(nameof(createdByUser));
            CreatedByUserId = createdByUser.Id;
        }

        public void Update(string name, string description, decimal discountPercentage, DateTime startDate, DateTime endDate)
        {
            if (discountPercentage <= 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercentage));

            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            DiscountPercentage = discountPercentage;
            StartDate = startDate;
            EndDate = endDate;
            base.Update();
        }

        public void Activate()
        {
            IsActive = true;
            base.Update();
        }

        public void Deactivate()
        {
            IsActive = false;
            base.Update();
        }

        public bool IsCurrentlyActive()
        {
            DateTime now = DateTime.UtcNow;
            return IsActive && now >= StartDate && now <= EndDate;
        }

        public decimal CalculateDiscountedPrice(decimal originalPrice)
        {
            if (!IsCurrentlyActive())
                return originalPrice;

            decimal discountAmount = originalPrice * (DiscountPercentage / 100);
            return originalPrice - discountAmount;
        }
    }
}