using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutomationValidator.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class FieldLengthAttribute : Attribute, IDbValidationAttribute
    {
        private string _replaceName;

        public string ReplaceName
        {
            get
            {
                return _replaceName;
            }
            set
            {
                _replaceName = value ?? string.Empty;
            }
        }
        private string _mMessage = string.Empty;

        public int MaxLength { get; set; }

        public string FixLength
        {
            get
            ;
            set;
        }

        public string Message
        {
            get
            {
                return _mMessage;
            }
            set
            {
                _mMessage = value ?? String.Empty;
            }
        }
        public void Validate(object o, PropertyInfo propertyInfo,
                             List<String> errors)
        {
            var value = propertyInfo.GetValue(o, null);
            if (!(value is string)) return;
            if (MaxLength != 0 && ((string)value).Length > MaxLength)
            {
                String item = Message;
                errors.Add(item);
            }

            if (!string.IsNullOrEmpty(FixLength))
            {
                if (MaxLength != 0 && ((string)value).Length.ToString() != FixLength)
                {
                    String item = Message;
                    errors.Add(item);
                }
            }
        }

        public void Validate(object o, MethodInfo methodInfo, List<string> errors)
        {

        }
    }
}
