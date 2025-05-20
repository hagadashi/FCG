
using FCG.Domain.Entities.Games;

namespace FCG.Domain.Entities.Users
{
    public class User : Entity
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public bool IsActive { get; private set; }
        public Guid RoleId { get; private set; }
        public virtual Role Role { get; private set; }
        public virtual ICollection<Session> Sessions { get; private set; }
        public virtual ICollection<Library> Libraries { get; private set; }
        public virtual ICollection<Sale> CreatedSales { get; private set; }

        protected User() { }

        public User(string username, string email, string passwordHash, string firstName, string lastName, Role role)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            FirstName = firstName;
            LastName = lastName;
            IsActive = true;
            Role = role ?? throw new ArgumentNullException(nameof(role));
            RoleId = role.Id;
            Sessions = new List<Session>();
            Libraries = new List<Library>();
            CreatedSales = new List<Sale>();
        }

        public void Update(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            base.Update();
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash ?? throw new ArgumentNullException(nameof(newPasswordHash));
            base.Update();
        }

        public void ChangeRole(Role newRole)
        {
            Role = newRole ?? throw new ArgumentNullException(nameof(newRole));
            RoleId = newRole.Id;
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

        public string FullName => $"{FirstName} {LastName}";
    }
}