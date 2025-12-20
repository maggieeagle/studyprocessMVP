using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public sealed partial class Email
    {
        private static readonly Regex _emailRegex = EmailRegex();

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
        private static partial Regex EmailRegex();

        public string Value { get; private set; } = null!;

        private Email() { }

        public Email(string value)
        {
            value = value.Trim();

            if (string.IsNullOrWhiteSpace(value)) throw new DomainException("Email must not be empty.");
            if (value.Length > 320) throw new DomainException("Email is too long.");
            if (!_emailRegex.IsMatch(value)) throw new DomainException("Email has invalid format.");

            Value = value;
        }

        public override bool Equals(object? obj) => obj is Email other && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

        public override string ToString() => Value;
    }
}
