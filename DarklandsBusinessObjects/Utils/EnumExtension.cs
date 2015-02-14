using System;
using System.ComponentModel;

namespace DarklandsBusinessObjects.Utils
{
    public static class EnumExtension
    {
        // http://stackoverflow.com/a/1415187
        public static string Description(this Enum value)
        {
            var type = value.GetType();

            var name = Enum.GetName(type, value);
            if (name == null) return null;

            var field = type.GetField(name);
            if (field == null) return name;

            var attr = Attribute.GetCustomAttribute(field, typeof (DescriptionAttribute)) as DescriptionAttribute;
            return attr != null ? attr.Description : name;
        }
    }
}