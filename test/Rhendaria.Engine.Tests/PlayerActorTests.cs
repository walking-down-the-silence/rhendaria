using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Xunit;

namespace Rhendaria.Engine.Tests
{
    public class PlayerActorTests
    {
        [Fact]
        public async void MoveRight_ShouldHavePositionAt_1_0()
        {
            // Arrange
            TestClusterBuilder builder = new TestClusterBuilder(1);
            builder.AddSiloBuilderConfigurator<TestSiloBuilderConfigurator>();
            builder.AddClientBuilderConfigurator<TestClientBuilderConfigurator>();
            TestCluster cluster = builder.Build();
            Vector2D expected = new Vector2D(10, 0);

            // Act
            cluster.Deploy();
            var player = cluster.GrainFactory.GetGrain<IPlayerActor>("test.user");
            var actual = await player.Move(Vector2D.XAxis);

            // Assert
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);

            cluster.StopAllSilos();
        }

        [Fact]
        public async void MoveRight_ReturnResultEqualsActualPosition()
        {
            // Arrange
            TestClusterBuilder builder = new TestClusterBuilder(1);
            builder.AddSiloBuilderConfigurator<TestSiloBuilderConfigurator>();
            builder.AddClientBuilderConfigurator<TestClientBuilderConfigurator>();
            TestCluster cluster = builder.Build();

            // Act
            cluster.Deploy();
            var player = cluster.GrainFactory.GetGrain<IPlayerActor>("test.user");
            var result = await player.Move(Vector2D.XAxis);
            var actual = await player.GetState();

            // Assert
            Assert.Equal(result.X, actual.Position.X);
            Assert.Equal(result.Y, actual.Position.Y);
        }

        private class TestSiloBuilderConfigurator : ISiloBuilderConfigurator
        {
            public void Configure(ISiloHostBuilder hostBuilder)
            {
                hostBuilder.AddMemoryGrainStorageAsDefault(options =>
                {
                    options.InitStage = 1;
                    options.NumStorageGrains = 1;
                });
            }
        }

        private class TestClientBuilderConfigurator : IClientBuilderConfigurator
        {
            public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
            {
            }
        }
    }
}
