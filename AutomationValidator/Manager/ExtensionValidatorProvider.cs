// A generic custom type descriptor for the specified type  

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutomationValidator.Attributes;

namespace AutomationValidator.Manager
{
    public sealed class CustomTypeDescriptionProvider<T> : TypeDescriptionProvider where T : class
    {

        public CustomTypeDescriptionProvider(TypeDescriptionProvider parent)
            : base(parent)
        {
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return new AttributeCustomTypeDescriptor<T>(base.GetTypeDescriptor(objectType, instance));
        }
    }

    public sealed class AttributeCustomTypeDescriptor<T> : CustomTypeDescriptor where T : class
    {
        /// <summary>  
        /// Constructor  
        /// </summary>  
        public AttributeCustomTypeDescriptor(ICustomTypeDescriptor parent)
            : base(parent)
        {
        }

        public override AttributeCollection GetAttributes()
        {
            Type validationType = typeof(T).GetInterface(typeof(IDbValidationAttribute).Name);
            if (validationType != null)
            {
                var filedLengthInstance = GetPropertyOwner(base.GetProperties().Cast<PropertyDescriptor>().First()) as IDbValidationAttribute;
                var attributes = new List<Attribute>(base.GetAttributes().Cast<Attribute>());
                var fieldLengthAttribute = new FieldLengthAttribute();
                if (filedLengthInstance != null)
                    TypeDescriptor.AddAttributes(filedLengthInstance, fieldLengthAttribute);
                attributes.Add(fieldLengthAttribute);
                return new AttributeCollection(attributes.ToArray());
            }
            return base.GetAttributes();

        }
        /// <summary>  
        /// This method add a new property to the original collection  
        /// </summary>  
        public override PropertyDescriptorCollection GetProperties()
        {
            // Enumerate the original set of properties and create our new set with it  
            PropertyDescriptorCollection originalProperties = base.GetProperties();
            var newProperties = new List<PropertyDescriptor>();
            foreach (PropertyDescriptor pd in originalProperties)
            {
                if (ValidationManager.Validators.ContainsKey(typeof(T).Name))
                {
                    var validator = ValidationManager.Validators[typeof(T).Name];
                    if (validator.ContainsKey(pd.Name))
                    {
                        var property = validator[pd.Name];
                        if (property.Visible)
                        {
                            var attributeList = new List<Attribute>();
                            if (property.FieldLength != 0)
                            {
                                var fieldLengthAttribute = new FieldLengthAttribute
                                                               {
                                                                   MaxLength = property.FieldLength,
                                                                   Message = property.LengthMessage
                                                               };
                                attributeList.Add(fieldLengthAttribute);
                            }
                            if (!property.FieldNullable)
                            {
                                var fieldLengthAttribute = new FieldNullableAttribute
                                                               {
                                                                   IsNullable = property.FieldNullable,
                                                                   Message = property.NullableMessage
                                                               };
                                attributeList.Add(fieldLengthAttribute);

                            }
                            PropertyDescriptor newProperty = TypeDescriptor.CreateProperty(typeof(T), pd.Name,
                                                                                           pd.PropertyType,
                                                                                           attributeList.ToArray());
                            newProperties.Add(newProperty);
                        }
                    }


                }

            }
            // Finally return the list  
            return new PropertyDescriptorCollection(newProperties.ToArray(), true);
            return base.GetProperties();
        }
    }
}