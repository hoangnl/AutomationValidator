using AutomationValidator.Attributes;
using AutomationValidator.Models;

namespace AutomationValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var address1 = new Address { State = "viet nam" };
            var result1 = Validator.DynamicValidate(address1);

            var address2 = new Address { City = "new york", State = "quoc gia Hoa Ki" };
            var result2 = Validator.DynamicValidate(address2);

            var person1 = new Person { FirstName = "nguyen le hoang" };
            var result3 = Validator.DynamicValidate(person1);
        }

    }
}
