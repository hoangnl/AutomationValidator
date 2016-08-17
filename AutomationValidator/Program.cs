using AutomationValidator.Attributes;
using AutomationValidator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person();
            person.FirstName = "nguyen le hoang";
            var result = Validator.Validate(person, 1);
        }
    }
}
