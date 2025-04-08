[![.NET](https://github.com/code-dispenser/Instructor/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/code-dispenser/Instructor/actions/workflows/dotnet.yml) [![Coverage Status](https://coveralls.io/repos/github/code-dispenser/Instructor/badge.svg?branch=main)](https://coveralls.io/github/code-dispenser/Instructor?branch=main) [![Nuget download][download-image]][download-url]

[download-image]: https://img.shields.io/nuget/dt/Instructor.Core
[download-url]: https://www.nuget.org/packages/Instructor.Core

<h1>
<img src="https://raw.githubusercontent.com/code-dispenser/Instructor/main/assets/Icon-64.png" align="center" alt="Instructor icon" /> Instructor
</h1>
<!--
# ![icon](https://raw.githubusercontent.com/code-dispenser/Instructor/main/assets/icon-64.png) When
-->
<!-- H1 for git hub, but for nuget the markdown is fine as it centers the image, uncomment as appropriate and do the same at the bottom of this file for the icon author -->

## Overview
**Instructor** is a lightweight, minimalist library focused on one job: dispatching instructions (commands and queries) to their respective handlers.

It integrates seamlessly with your existing IoC container—like Autofac or Microsoft.Extensions.DependencyInjection—without enforcing conventions or adding unnecessary abstractions. All handler resolution is delegated to the container you already use.

## Example Usage

A working example can be found in the [Instructor GitHub Repository.](https://github.com/code-dispenser/Instructor)

### Define Commands and Queries (aka Instructions)
Just implement IInstruction&lt;TValue&gt; for each instruction.
```csharp   
public class AddCustomerCommand: IInstruction<None>
{
    public Guid   CustomerID   { get;}
    public string CustomerName { get; } = default!;

    public AddCustomerCommand(CustomerData customerData)

        => (CustomerID, CustomerName) = (customerData.CustomerID, customerData.CustomerName);
}

public class GetCustomerQuery(Guid customerID) : IInstruction<CustomerData>
{
    public Guid CustomerID { get; } = customerID;
}

```

### Define Handlers
Implement IInstructionHandler&lt;TInstruction, TValue&gt; or optionally the marker interfaces ICommandHandler&lt;TInstruction, TValue&gt; 
or IQueryHandler&lt;TInstruction, TValue&gt;.  
```csharp   

public class AddCustomerCommandHandler : ICommandHandler<AddCustomerCommand, None>
{
    public AddCustomerCommandHandler(){ }//TODO: inject database dependencies here

    public async Task<None> Handle(AddCustomerCommand instruction, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"Added customer {instruction.CustomerName} with ID {instruction.CustomerID} to the database (not).");
        return None.Value;
    }
}

public class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, CustomerData>
{
    public GetCustomerQueryHandler() { } //TODO: inject dependencies here as usual
    public async Task<CustomerData> Handle(GetCustomerQuery instruction, CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync($"Getting the customer with ID {instruction.CustomerID} from the database (not).");

        return new CustomerData(instruction.CustomerID, "John Doe");
    }
}

```

#### Register the InstructionDispatcher and its factory delegate that defers resolution of instruction handlers via the underlying IoC container 
Autofcac example:
 ```csharp
    /*
        * Scan assemblies or add handlers manually i.e. builder.RegisterType<AddCustomerCommandHandler>().As<IInstructionHandler<AddCustomerCommand, None>>()
    */
    builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).AsClosedTypesOf(typeof(IInstructionHandler<,>));
    builder.Register<InstructionDispatcher>(c =>
    {
        var context = c.Resolve<IComponentContext>();
        return new InstructionDispatcher(type => context.Resolve(type));
    
    }).As<IInstructionDispatcher>().InstancePerLifetimeScope();

```
Microsoft.Extensions.DependencyInjection example:
 ```csharp
    /*
        * Add handlers manually or get a package like Scrutor to scan assemblies
    */
    .Services.AddTransient<IInstructionHandler<AddCustomerCommand, None>, AddCustomerCommandHandler>()
            .AddTransient<IInstructionHandler<GetCustomerQuery, CustomerData>, GetCustomerQueryHandler>()
            .AddSingleton<IInstructionDispatcher>(provider => new InstructionDispatcher(type => provider.GetRequiredService(type)))

```


#### Send your instructions via the InstructionDispatcher to be processed by the handlers
 ```csharp

    private readonly IInstructionDispatcher _instructionDispatcher;//set by constructor injection

    var newCustomer = new CustomerData(Guid.NewGuid(), "John Doe");//Data recieved from some client

    _ = await __instructionDispatcher.SendInstruction(new AddCustomerCommand(newCustomer));//just dispatch the command (instruction) for handling

    var customerData = await __instructionDispatcher.SendInstruction(new GetCustomerQuery(newCustomer.CustomerID));


```
