
namespace FCG.Domain.Entities.Games
{
    public class Category : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public virtual ICollection<Game> Games { get; private set; }

        protected Category() { }

        public Category(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Games = new List<Game>();
        }

        public void Update(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            base.Update();
        }
    }
}