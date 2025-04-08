using Instructor.Core.Common.Seeds;
using Instructor.Core.Tests.Infrastructure.Models;

namespace Instructor.Core.Tests.Infrastructure.InstructionHandlers.Customer;

public class AddCustomerCommand(CustomerData customerData) : IInstruction<None>
{
    public Guid CustomerID     { get; } = customerData.CustomerID;
    public string CustomerName { get; } = customerData.CustomerName;
}

public class AddCustomerCommandHandler : ICommandHandler<AddCustomerCommand, None>
{
    public async Task<None> Handle(AddCustomerCommand instruction, CancellationToken cancellationToken)
        
        => await Task.FromResult(None.Value);
}
