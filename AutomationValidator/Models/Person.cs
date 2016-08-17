using AutomationValidator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationValidator.Models
{
    public class Person
    {
        [FieldNullable(IsNullable = false, Message = "Không được trống")]
        public String LastName { get; set; }

        [FieldLength(MaxLength = 10, Message = "Quá độ dài")]
        public String FirstName { get; set; }

        public int Age { get; set; }
    }
}
