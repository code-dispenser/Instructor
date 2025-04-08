using Autofac;
using Instructor.Core.Common.Seeds;
using Instructor.Core.Demo.Areas.Customers;
using Instructor.Core.Demo.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Instructor.Core.Demo
{
    internal class Program
    {
        static async Task Main()
        {
            /*
                * Use your preferred DI container such as Microsoft.Extensions.DependencyInjection or Autofac. 
            */

            //var instructionDispatcher = ConfiguredMicrosoftContainer().GetRequiredService<IInstructionDispatcher>();

            var instructionDispatcher = ConfiguredAutofacContainer().Resolve<IInstructionDispatcher>();

            var newCustomer = new CustomerData(Guid.NewGuid(), "John Doe");//Data recieved from some client

            _ = await instructionDispatcher.SendInstruction(new AddCustomerCommand(newCustomer));//just dispatch the command (instruction) for handling

            var customerData = await instructionDispatcher.SendInstruction(new GetCustomerQuery(newCustomer.CustomerID));

            Console.WriteLine($"Returned: {customerData}");
            Console.ReadLine();
        }
        private static IContainer ConfiguredAutofacContainer()
        {
            var builder = new ContainerBuilder();
            /*
                * Scan assemblies like below or just add individually i.e. builder.RegisterType<AddCustomerCommandHandler>().As<IInstructionHandler<AddCustomerCommand, None>>()
            */ 
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).AsClosedTypesOf(typeof(IInstructionHandler<,>));
            builder.Register<InstructionDispatcher>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return new InstructionDispatcher(type => context.Resolve(type));

            }).As<IInstructionDispatcher>().InstancePerLifetimeScope();

            return builder.Build();

        }

        private static ServiceProvider ConfiguredMicrosoftContainer()
        
           => Host.CreateApplicationBuilder() //Add Scrutor nuget for scanning, 
                  .Services.AddScoped<IInstructionHandler<AddCustomerCommand, None>, AddCustomerCommandHandler>()
                           .AddScoped<IInstructionHandler<GetCustomerQuery, CustomerData>, GetCustomerQueryHandler>()
                           .AddScoped<IInstructionDispatcher>(provider => new InstructionDispatcher(type => provider.GetRequiredService(type)))
                  .BuildServiceProvider();

    }
}
