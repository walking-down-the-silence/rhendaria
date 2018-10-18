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
            Vector2D expected = new Vector2D(1, 0);
            IPlayerActor player = new PlayerActor();

            // Act
            var result = player.Move(Direction.Right);
            var actual = player.GetPosition();

            // Assert
            Assert.Equal(expected.Left, actual.Result.Left);
            Assert.Equal(expected.Top, actual.Result.Top);
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
    }
}
