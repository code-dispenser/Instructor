using Instructor.Core.Tests.Infrastructure.Models;

namespace Instructor.Core.Tests.Infrastructure;

public static class DataFactory
{
    public static Guid   CustomerID   = Guid.NewGuid();
    public static string CustomerName = "John Doe";
    public static CustomerData GetCustomerData()

        => new CustomerData(CustomerID, CustomerName);
}
