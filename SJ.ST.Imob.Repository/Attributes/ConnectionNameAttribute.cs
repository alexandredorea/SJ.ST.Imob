using System;

namespace SJ.ST.Imob.Repository
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ConnectionNameAttribute : Attribute
    {
        public ConnectionNameAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty connection name is not allowed", nameof(value));
            Name = value;
        }

        public virtual string Name { get; private set; }
    }
}
