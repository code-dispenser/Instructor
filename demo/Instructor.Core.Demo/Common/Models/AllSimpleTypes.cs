using System.Text.Json.Serialization;

namespace Instructor.Core.Demo.Common.Models;

public record CustomerData(Guid CustomerID, string CustomerName);

public sealed record None
{
    public static None Value { get; } = new();

    public override string ToString() => "Ø";

}