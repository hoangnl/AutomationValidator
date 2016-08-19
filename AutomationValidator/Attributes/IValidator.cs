using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutomationValidator.Attributes
{
    public interface IDbValidationAttribute
    {
        void Validate(object o, PropertyInfo propertyInfo, List<String> errors);
        void Validate(object o, MethodInfo methodInfo, List<String> errors);
    }
}
