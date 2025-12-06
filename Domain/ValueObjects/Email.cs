using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public sealed class Email
    {
        private static readonly Regex _emailRegex =
            new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public string Value { get; }

        public Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email must not be empty.");
            email = email.Trim();
            if (email.Length > 320)
                throw new DomainException("Email is too long.");

            if (!_emailRegex.IsMatch(email))
                throw new DomainException("Email has invalid format.");

            Value = email;
        }

        public override bool Equals(object? obj) =>
            obj is Email other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

        public override string ToString() => Value;
    }
}
