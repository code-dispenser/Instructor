using Instructor.Core.Common.Seeds;
using Instructor.Core.Demo.Common.Models;

namespace Instructor.Core.Demo.Areas.Customers;

public class GetCustomerQuery : IInstruction<CustomerData>
{
    public Guid CustomerID { get; }
    public GetCustomerQuery(Guid customerID)
        
        => CustomerID = customerID;
}

public class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, CustomerData>
{
    //TODO: inject any dependencies here as normal
    public GetCustomerQueryHandler() { } 
    public async Task<CustomerData> Handle(GetCustomerQuery instruction, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"Getting the customer with ID {instruction.CustomerID} from the database (not).");

        return new CustomerData(instruction.CustomerID, "John Doe");
    }
}