using System.Collections.Specialized;

namespace Khan
{
    public class CssRule
    {
        public CssRule()
        {
            Selectors = new StringCollection();
            CssProperties = new NameValueCollection();
            HtmlAttributes = new NameValueCollection();
        }

        public StringCollection Selectors { get; set; }
        public NameValueCollection CssProperties { get; set; }
        public NameValueCollection HtmlAttributes { get; set; }

        protected bool Equals(CssRule other)
        {
            return Equals(Selectors, other.Selectors) && Equals(HtmlAttributes, other.HtmlAttributes) && Equals(CssProperties, other.CssProperties);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CssRule) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Selectors != null ? Selectors.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (HtmlAttributes != null ? HtmlAttributes.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CssProperties != null ? CssProperties.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}