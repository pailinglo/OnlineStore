using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Utilities
{
    public class DateComparison : ValidationAttribute
    {
        private readonly string toBeCompared;
        private readonly string comparisonType;

        public DateComparison(string toBeCompared, string comparisonType)
        {
            this.toBeCompared = toBeCompared;
            this.comparisonType = comparisonType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;

            if (value.GetType() == typeof(IComparable))
            {
                throw new ArgumentException("value has not implemented IComparable interface");
            }

            var currentValue = (IComparable)value;

            var property = validationContext.ObjectType.GetProperty(this.toBeCompared);

            if (property == null)
            {
                throw new ArgumentException("Comparison property with this name not found");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue.GetType() == typeof(IComparable))
            {
                throw new ArgumentException("Comparison property has not implemented IComparable interface");
            }

            if (!ReferenceEquals(value.GetType(), comparisonValue.GetType()))
            {
                throw new ArgumentException("The properties types must be the same");
            }

            bool compareToResult;

            switch (comparisonType)
            {
                case "LessThan":
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) >= 0;

                    break;

                case "LessThanOrEqualTo":
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) > 0;

                    break;

                case "EqualTo":
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) != 0;

                    break;

                case "GreaterThan":
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) <= 0;

                    break;

                case "GreaterThanOrEqualTo":
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) < 0;

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return compareToResult ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
        }
    }
}
