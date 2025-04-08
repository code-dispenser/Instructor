namespace Instructor.Core.Demo.Common.Models;

public record CustomerData(Guid CustomerID, string CustomerName);
public readonly record struct None
{
    public static None Value { get; } = new None();
    public override string ToString() => "Ø";
}