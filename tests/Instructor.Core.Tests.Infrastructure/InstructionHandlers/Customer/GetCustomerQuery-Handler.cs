using Instructor.Core.Common.Seeds;
using Instructor.Core.Tests.Infrastructure.Models;

namespace Instructor.Core.Tests.Infrastructure.InstructionHandlers.Customer;

public class GetCustomerQuery(Guid customerID) : IInstruction<CustomerData>
{
    public Guid CustomerID { get; } = customerID;
}

public class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, CustomerData>
{
    public async Task<CustomerData> Handle(GetCustomerQuery instruction, CancellationToken cancellationToken)
    
        => await Task.FromResult(new CustomerData(instruction.CustomerID, DataFactory.CustomerName));
    
}
