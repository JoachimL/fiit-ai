using System;
using System.Collections.Generic;
using System.Text;

namespace StrongR.ReadStack.EntityPropertyConverter
{
    /// <summary>
    /// From https://github.com/jtourlamain/DevProtocol.Azure.EntityPropertyConverter/blob/master/src/DevProtocol.Azure.EntityPropertyConverter/EntityPropertyConverterAttribute.cs
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityPropertyConverterAttribute : Attribute
    {
        public Type ConvertToType;

        public EntityPropertyConverterAttribute()
        {

        }
        public EntityPropertyConverterAttribute(Type convertToType)
        {
            ConvertToType = convertToType;
        }
    }
}
