using System;
using System.Collections.Generic;

namespace FCG.Domain.Entities.Games
{
    public class Game : Entity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string ImageUrl { get; private set; }
        public bool IsActive { get; private set; }
        public Guid CategoryId { get; private set; }
        public virtual Category Category { get; private set; }
        public virtual ICollection<Library> Libraries { get; private set; }
        public virtual ICollection<Sale> Sales { get; private set; }

        protected Game() { }

        public Game(string title, string description, decimal price, string imageUrl, Category category)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            Price = price;
            ImageUrl = imageUrl;
            IsActive = true;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            CategoryId = category.Id;
            Libraries = new List<Library>();
            Sales = new List<Sale>();
        }

        public void Update(string title, string description, decimal price, string imageUrl, Category category)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            Price = price;
            ImageUrl = imageUrl;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            CategoryId = category.Id;
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

        public decimal GetCurrentPrice()
        {
            return Price;
        }
    }
}