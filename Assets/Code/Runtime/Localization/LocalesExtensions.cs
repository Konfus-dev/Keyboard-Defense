using System;
using System.Globalization;
using System.Linq;

namespace KeyboardDefense.Localization
{
    public static class LocalesExtensions
    {
        public static CultureInfo GetCultureInfo(this Locale locale)
        {
            var cultureInfo = new CultureInfo(GetAttribute<CultureInfoNameAttribute>(locale).Name, false);
            return cultureInfo;
        }
        
        private static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
}