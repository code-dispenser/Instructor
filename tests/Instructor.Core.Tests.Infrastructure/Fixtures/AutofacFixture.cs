using Autofac;
using Instructor.Core.Common.Seeds;
using Instructor.Core.Tests.Infrastructure.InstructionHandlers.Customer;
using Instructor.Core.Tests.Infrastructure.Models;

namespace Instructor.Core.Tests.Infrastructure.Fixtures;

public class AutofacFixture
{

    public IInstructionDispatcher InstructionDispatcher { get; }
    public AutofacFixture()
    
        =>  InstructionDispatcher = ConfigureAutofac().Resolve<IInstructionDispatcher>();
   
    private static IContainer ConfigureAutofac()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<AddCustomerCommandHandler>().As<IInstructionHandler<AddCustomerCommand, None>>().InstancePerDependency();
        builder.RegisterType<GetCustomerQueryHandler>().As<IInstructionHandler<GetCustomerQuery, CustomerData>>().InstancePerDependency();
        builder.Register<InstructionDispatcher>(c =>
        {
            var context = c.Resolve<IComponentContext>();
            return new InstructionDispatcher(type => context.Resolve(type));
        }).As<IInstructionDispatcher>().InstancePerLifetimeScope();

        return builder.Build();
    }
}
