
namespace FCG.Domain.Entities.Users
{

    public class Role : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsDefault { get; private set; }
        public virtual ICollection<User> Users { get; private set; }

        protected Role() { }

        public Role(string name, string description, bool isDefault)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            IsDefault = isDefault;
            Users = new List<User>();
        }

        public void Update(string name, string description, bool isDefault)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            IsDefault = isDefault;
            base.Update();
        }
    }

}