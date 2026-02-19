namespace PetWorld.Integration.Tests.Infrastructure;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class IntegrationTestsCollection : ICollectionFixture<IntegrationTestFixture>
{
    public const string Name = "Integration tests";
}
