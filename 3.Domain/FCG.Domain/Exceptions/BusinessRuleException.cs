
namespace FCG.Domain.Exceptions
{

    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException(string message) : base(message)
        {
        }

        public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
