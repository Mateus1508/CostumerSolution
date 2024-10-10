namespace CostumerSolution.API.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; set; }

        public Email()
        {
            Value = string.Empty;
        }

        public Email(string email) : this()
        {
            Value = email.ToLower();
        }

        public override bool Equals(object? obj)
        {
            if (obj is Email other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
