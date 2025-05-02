namespace GicBankApp.Domain.ValueObjects;

using System.Globalization;
using GicBankApp.Domain.Common;

public sealed class BusinessDate : ValueObject, IEquatable<BusinessDate> {
       public DateTime Value { get; }

        private BusinessDate(DateTime value)
        {
            Value = value.Date; 
        }

        public static BusinessDate From(DateTime dateTime)
        {
            return new BusinessDate(dateTime);
        }

        public static BusinessDate From(string yyyymmdd)
        {
            if (!DateTime.TryParseExact(yyyymmdd, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                throw new ArgumentException("Invalid date format. Expected yyyyMMdd.");

            return new BusinessDate(dt);
        }    

         public override string ToString() => Value.ToString("yyyyMMdd");

    public bool Equals(BusinessDate? other)
    {
        if (other is null) return false;
        return Value.Equals(other.Value);
    }
}