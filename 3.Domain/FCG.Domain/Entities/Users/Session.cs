
namespace FCG.Domain.Entities.Users
{
    public class Session : Entity
    {
        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsActive { get; private set; }
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }

        protected Session() { }

        public Session(string token, DateTime expiresAt, User user)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
            ExpiresAt = expiresAt;
            IsActive = true;
            User = user ?? throw new ArgumentNullException(nameof(user));
            UserId = user.Id;
        }

        public void Deactivate()
        {
            IsActive = false;
            base.Update();
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpiresAt;
        }
    }
}