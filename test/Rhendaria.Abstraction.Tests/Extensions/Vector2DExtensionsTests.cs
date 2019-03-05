using Xunit;
using Rhendaria.Abstraction.Extensions;

namespace Rhendaria.Abstraction.Tests
{
    public class Vector2DExtensionsTests
    {
        [Fact]
        public void Shift__OnVector_10_10_Right__Should_Shift_To_20_10()
        {
            // Arrange
            int speed = 10;
            Vector2D source = new Vector2D(10, 10);
            Vector2D right = new Vector2D(40, 10);
            Vector2D expected = new Vector2D(20, 10);

            // Act
            Vector2D actual = source.Shift(right, speed);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shift__OnVector_10_10_Left__Should_Shift_To_0_10()
        {
            // Arrange
            int speed = 10;
            Vector2D source = new Vector2D(10, 10);
            Vector2D left = new Vector2D(-20, 10);
            Vector2D expected = new Vector2D(0, 10);

            // Act
            Vector2D actual = source.Shift(left, speed);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shift__OnVector_10_10_Up__Should_Shift_To_10_20()
        {
            // Arrange
            int speed = 10;
            Vector2D source = new Vector2D(10, 10);
            Vector2D up = new Vector2D(10, 40);
            Vector2D expected = new Vector2D(10, 20);

            // Act
            Vector2D actual = source.Shift(up, speed);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Shift__OnVector_10_10_Down__Should_Shift_To_10_0()
        {
            // Arrange
            int speed = 10;
            Vector2D source = new Vector2D(10, 10);
            Vector2D down = new Vector2D(10, -20);
            Vector2D expected = new Vector2D(10, 0);

            // Act
            Vector2D actual = source.Shift(down, speed);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
