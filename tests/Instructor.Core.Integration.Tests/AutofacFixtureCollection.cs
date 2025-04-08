using Instructor.Core.Tests.Infrastructure.Fixtures;

namespace Instructor.Core.Integration.Tests;

[CollectionDefinition(nameof(AutofacFixtureCollection))]
public class AutofacFixtureCollection : ICollectionFixture<AutofacFixture>
{
}

