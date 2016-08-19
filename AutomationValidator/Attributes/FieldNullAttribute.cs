using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutomationValidator.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldNullableAttribute : Attribute, IDbValidationAttribute
    {
        private string _mMessage = "{0} cannot be null";

        private string _ReplaceName;

        public string ReplaceName
        {
            get
            {
                return _ReplaceName;
            }
            set
            {
                _ReplaceName = value ?? string.Empty;
            }
        }

        public FieldNullableAttribute()
        {
            _ReplaceName = string.Empty;
            IsNullable = false;
        }

        public bool IsNullable { get; set; }

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
            object value = propertyInfo.GetValue(o, null);
            if ((value is string))
            {
                if (String.IsNullOrEmpty(((string)value)) && !IsNullable)
                {
                    String item = Message;
                    errors.Add(item);
                }
            }
            else
            {
                if (value == null && !IsNullable)
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
