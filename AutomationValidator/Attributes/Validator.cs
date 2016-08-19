using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Collections;
using AutomationValidator.Manager;

namespace AutomationValidator.Attributes
{

    public class Validator
    {
        public static List<String> Validate(object o)
        {
            return Validate(o, false);
        }

        public static List<String> Validate
            (object o, bool allowNullObject)
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
                            (o, info, errors);
                        if (errors.Count > a) break;
                        if (info.PropertyType.IsClass ||
                            info.PropertyType.IsInterface)
                        {
                            errors.AddRange(Validate
                                                (info.GetValue(o, null), true));
                        }
                        if (info.PropertyType.IsGenericType)
                        {
                            var list = info.GetValue(o, null) as IList;
                            foreach (object t in list)
                            {
                                errors.AddRange(Validate
                                                    (t, true));
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
                            (o, method, errors);
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

        public static List<String> DynamicValidate<T>(T o) where T : class
        {
            TypeDescriptor.AddProvider(new CustomTypeDescriptionProvider<T>(TypeDescriptor.GetProvider(typeof(T))), o);
            var errors = new List<String>();
            o.GetType().GetProperties().ToList().ForEach
                (info =>
                {
                    PropertyDescriptor propDescriptor = TypeDescriptor.GetProperties(o).Cast<PropertyDescriptor>().SingleOrDefault(p => info.Name == p.Name);
                    if (propDescriptor != null)
                    {
                        var attributeList =
                          propDescriptor.Attributes.Cast<Attribute>().Where(p => p.GetType().GetInterface(typeof(IDbValidationAttribute).Name, false) != null && p.GetType().GetInterface(typeof(IDbValidationAttribute).Name, false).Name == typeof(IDbValidationAttribute).Name).ToList();
                        foreach (Attribute customAttribute in attributeList)
                        {
                            int a = errors.Count;
                            ((IDbValidationAttribute)customAttribute).Validate
                                (o, info, errors);
                            if (errors.Count > a) break;
                            if (info.PropertyType.IsClass ||
                                info.PropertyType.IsInterface)
                            {
                                errors.AddRange(Validate
                                                    (info.GetValue(o, null), true));
                            }
                            if (info.PropertyType.IsGenericType)
                            {
                                IList list = info.GetValue(o, null) as IList;
                                foreach (object t in list)
                                {
                                    errors.AddRange(Validate
                                                        (t, true));
                                }
                            }
                        }
                    }
                }
                 );
            return errors;

        }
    }
}