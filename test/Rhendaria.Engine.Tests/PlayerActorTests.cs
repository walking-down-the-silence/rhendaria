using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;
using Rhendaria.Abstraction;
using Xunit;

namespace Rhendaria.Engine.Tests
{
    public class PlayerActorTests
    {
        [Fact]
        public void MoveRight_ShouldHavePositionAt_1_0()
        {
            // Arrange
            TestClusterBuilder builder = new TestClusterBuilder(1);
            builder.AddSiloBuilderConfigurator<TestSiloBuilderConfigurator>();
            builder.AddClientBuilderConfigurator<TestClientBuilderConfigurator>();
            TestCluster cluster = builder.Build();
            Vector2D expected = new Vector2D(1, 0);

            // Act
            cluster.Deploy();
            var player = cluster.GrainFactory.GetGrain<IPlayerActor>("test.user");
            var actual = player.Move(Direction.Right);

            // Assert
            Assert.Equal(expected.Left, actual.Result.Left);
            Assert.Equal(expected.Top, actual.Result.Top);

            cluster.StopAllSilos();
        }

        [Fact]
        public void MoveRight_ReturnResultEqualsActualPosition()
        {
            // Arrange
            IPlayerActor player = new PlayerActor();

            // Act
            var result = player.Move(Direction.Right);
            var actual = player.GetPosition();

            // Assert
            Assert.Equal(result.Result.Left, actual.Result.Left);
            Assert.Equal(result.Result.Top, actual.Result.Top);
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
