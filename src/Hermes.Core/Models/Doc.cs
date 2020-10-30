using System;

namespace Tristan.Core.Models
{
    public class Mail : IEquatable<Mail>
    { 
        public Guid Id { get; set; }

        public string Oggetto { get; set; }

        public bool Equals(Mail other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Mail) obj);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Mail left, Mail right) => Equals(left, right);

        public static bool operator !=(Mail left, Mail right) => !Equals(left, right);
    }
}