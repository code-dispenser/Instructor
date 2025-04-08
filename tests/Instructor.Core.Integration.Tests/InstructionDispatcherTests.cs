using FluentAssertions;
using Instructor.Core.Common.Seeds;
using Instructor.Core.Tests.Infrastructure;
using Instructor.Core.Tests.Infrastructure.Fixtures;
using Instructor.Core.Tests.Infrastructure.InstructionHandlers.Customer;
using Instructor.Core.Tests.Infrastructure.Models;

namespace Instructor.Core.Integration.Tests;


[Collection(nameof(AutofacFixtureCollection))]
public class InstructionDispatcherTests(AutofacFixture autofacFixture)
{
    private readonly IInstructionDispatcher _instructionDispatcher = autofacFixture.InstructionDispatcher;

    [Fact]
    public async Task The_dispatcher_should_send_a_command_to_be_processed_by_its_registered_handler()
    {
        var customerData       = DataFactory.GetCustomerData();
        var addCustomerCommand = new AddCustomerCommand(customerData);
        var theResult          = await _instructionDispatcher.SendInstruction(addCustomerCommand, CancellationToken.None);

        theResult.Should().BeOfType<None>();

    }

    [Fact]
    public async Task The_dispatcher_should_send_a_query_to_be_processed_by_its_registered_handler()
    {
        var customerID    = DataFactory.CustomerID;
        var customerQuery = new GetCustomerQuery(customerID);
        var theResult     = await _instructionDispatcher.SendInstruction(customerQuery, CancellationToken.None);

        theResult.Should().Match<CustomerData>(c => c.CustomerID == customerID && c.CustomerName == DataFactory.CustomerName);
    }
}