using System;

namespace KeyboardDefense.Localization
{
    public class CultureInfoNameAttribute : Attribute
    {
        public CultureInfoNameAttribute(string name)
        {
            Name = name;
        }
        
        public string Name { get; }
    }
}