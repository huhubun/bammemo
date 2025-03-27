namespace Bammemo.Service.Abstractions.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class MapAttribute<TSource, TTarget> : Attribute
{
}
