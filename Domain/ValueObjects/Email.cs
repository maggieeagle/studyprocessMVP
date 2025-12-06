using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public sealed partial class Email
    {
        private static readonly Regex _emailRegex = EmailRegex();

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
        private static partial Regex EmailRegex();

        public string Value { get; }

        public Email(string email)
        {
            email = email.Trim();

            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email must not be empty.");
            if (email.Length > 320) throw new DomainException("Email is too long.");
            if (!_emailRegex.IsMatch(email)) throw new DomainException("Email has invalid format.");

            Value = email;
        }

        public override bool Equals(object? obj) => obj is Email other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

        public override string ToString() => Value;
    }
}
