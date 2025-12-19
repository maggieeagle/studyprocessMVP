namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            if (obj is not BaseEntity other) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return Id != 0 && Id == other.Id;
        }

        public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
    }
}
