using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;

namespace AutomationValidator.Attributes
{


    public class Validator
    {

        public static int stt = 1;

        public int IntLc = 0;
        public Validator(int intLcnt)
        {
            IntLc = intLcnt;
        }

        public Validator()
        {
        }


        public static List<String> Validate(object o, int i)
        {
            return Validate(o, false, "E", i);
        }

        public static List<String> Validate
            (object o, bool allowNullObject, string nullMessage, int i)
        {
            var errors = new List<String>();
            if (o != null)
            {
                foreach (var info in o.GetType().GetProperties())
                {
                    foreach (object customAttribute in info.GetCustomAttributes
                        (typeof(IDbValidationAttribute), true))
                    {
                        int a = errors.Count;
                        ((IDbValidationAttribute)customAttribute).Validate
                            (o, info, errors, i);
                        if (errors.Count > a) break;
                        if (info.PropertyType.IsClass ||
                            info.PropertyType.IsInterface)
                        {
                            errors.AddRange(Validate
                                                (info.GetValue(o, null), true, null, i));
                        }
                        if (info.PropertyType.IsGenericType)
                        {
                            IList list = info.GetValue(o, null) as IList;
                            for (int j = 0; j < list.Count; j++)
                            {
                                errors.AddRange(Validate
                                                (list[j], true, null, j));
                            }
                        }
                    }
                }//end foreach
                foreach (var method in o.GetType().GetMethods())
                {
                    foreach (object customAttribute in method.GetCustomAttributes
                        (typeof(IDbValidationAttribute), true))
                    {
                        ((IDbValidationAttribute)customAttribute).Validate
                            (o, method, errors, i);
                    }
                }
            }
            else if (!allowNullObject)
            {
                var item = "Thiếu thông tin ";
                errors.Add(item);
            }
            return errors;
        }
    }

    public interface IDbValidationAttribute
    {
        void Validate(object o, PropertyInfo propertyInfo, List<String> errors, int i);
        void Validate(object o, MethodInfo methodInfo, List<String> errors, int i);
    }

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
        public void Validate(object o, MethodInfo info,
                             List<String> errors, int i) { }
        public void Validate(object o, PropertyInfo propertyInfo,
                             List<String> errors, int i)
        {
            object value = propertyInfo.GetValue(o, null);
            if ((value is string))
            {
                if (String.IsNullOrEmpty(((string)value)) && !IsNullable)
                {
                    String item = "Lỗi";
                    errors.Add(item);
                }
            }
            else
            {
                if (value == null && !IsNullable)
                {
                    String item = "Lỗi";
                    errors.Add(item);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FieldLengthAttribute : Attribute, IDbValidationAttribute
    {
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
        private string _mMessage = "";

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
        public void Validate(object o, MethodInfo info, List<String> errors, int i) { }
        public void Validate(object o, PropertyInfo propertyInfo,
                             List<String> errors, int i)
        {
            var value = propertyInfo.GetValue(o, null);
            if (!(value is string)) return;
            if (MaxLength != 0 && ((string)value).Length > MaxLength)
            {
                String item = "Lỗi";
                errors.Add(item);
                //errors.Add(String.Format
                //               (_mMessage, propertyInfo.Name, MaxLength)+"-"+i.ToString("0000"));
            }

            if (!string.IsNullOrEmpty(FixLength))
            {
                if (MaxLength != 0 && ((string)value).Length.ToString() != FixLength)
                {
                    String item = "Lỗi";
                    errors.Add(item);
                }
            }
        }
    }
}