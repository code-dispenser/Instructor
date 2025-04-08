using Instructor.Core.Common.Seeds;
using Instructor.Core.Demo.Common.Models;

namespace Instructor.Core.Demo.Areas.Customers;

public class AddCustomerCommand : IInstruction<None>
{
    public Guid   CustomerID   { get; }
    public string CustomerName { get; } = default!;

    public AddCustomerCommand(CustomerData customerData)

        => (CustomerID, CustomerName) = (customerData.CustomerID, customerData.CustomerName);
}

public class AddCustomerCommandHandler : ICommandHandler<AddCustomerCommand, None>
{
    //TODO: inject any dependencies here as normal 
    public AddCustomerCommandHandler() { }

    public async Task<None> Handle(AddCustomerCommand instruction, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"Added customer {instruction.CustomerName} with ID {instruction.CustomerID} to the database (not).");
        return None.Value;
    }
}