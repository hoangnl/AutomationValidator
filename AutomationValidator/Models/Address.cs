using AutomationValidator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationValidator.Models
{
    public class Address
    {
        [FieldNullable(IsNullable = false, Message = "Không được trống")]
        public String City { get; set; }

        [FieldLength(MaxLength = 10, Message = "Quá độ dài")]
        public String State { get; set; }
    }
}
