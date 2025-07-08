using System;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapFromAttribute : Attribute
{
    public string SourceProperty { get; }

    public MapFromAttribute(string sourceProperty)
    {
        SourceProperty = sourceProperty;
    }
}
