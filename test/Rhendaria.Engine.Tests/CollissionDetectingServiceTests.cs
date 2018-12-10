using System.Linq;
using FakeItEasy;
using Rhendaria.Abstraction;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Engine.Services;
using Xunit;

namespace Rhendaria.Engine.Tests
{
    public class CollissionDetectingServiceTests
    {
        [Fact]
        public void ShouldDetectCollision_BiggerPlayerWins()
        {
            //Arange
            var detector = new CollisionDetectingService();

            var expectedWinner = A.Fake<IPlayerActor>();
            A.CallTo(() => expectedWinner.GetUsername()).Returns(nameof(expectedWinner));
            A.CallTo(() => expectedWinner.GetSize()).Returns(2);
            A.CallTo(() => expectedWinner.GetPosition()).Returns(new Vector2D(3, 3));

            var expectedLooser = A.Fake<IPlayerActor>();
            A.CallTo(() => expectedLooser.GetUsername()).Returns(nameof(expectedLooser));
            A.CallTo(() => expectedLooser.GetSize()).Returns(1);
            A.CallTo(() => expectedLooser.GetPosition()).Returns(new Vector2D(3, 4));

            //Act
            CollisionResult result = detector.DetectCollision(expectedWinner, new[] {expectedLooser}).Result;
            var expectedWinnerName = expectedWinner.GetUsername().Result;
            var expectedLooserName = expectedLooser.GetUsername().Result;

            var actualWinnerName = result.Winner.GetUsername().Result;
            var actualLooserName = result.Loosers.First().GetUsername().Result;

            //Assert
            Assert.False(result.IsEmpty());
            Assert.Equal(expectedWinnerName, actualWinnerName);
            Assert.Equal(new[] {expectedLooserName}, new[] {actualLooserName});
        }

        [Fact]
        public void ShouldDetectCollision_TheBiggestPlayerWins()
        {
            //Arange
            var detector = new CollisionDetectingService();

            var expectedWinner = A.Fake<IPlayerActor>();
            A.CallTo(() => expectedWinner.GetUsername()).Returns(nameof(expectedWinner));
            A.CallTo(() => expectedWinner.GetSize()).Returns(5);
            A.CallTo(() => expectedWinner.GetPosition()).Returns(new Vector2D(3, 3));

            var expectedLooser1 = A.Fake<IPlayerActor>();
            A.CallTo(() => expectedLooser1.GetUsername()).Returns(nameof(expectedLooser1));
            A.CallTo(() => expectedLooser1.GetSize()).Returns(2);
            A.CallTo(() => expectedLooser1.GetPosition()).Returns(new Vector2D(3, 4));

            var expectedLooser2 = A.Fake<IPlayerActor>();
            A.CallTo(() => expectedLooser2.GetUsername()).Returns(nameof(expectedLooser2));
            A.CallTo(() => expectedLooser2.GetSize()).Returns(1);
            A.CallTo(() => expectedLooser2.GetPosition()).Returns(new Vector2D(2, 3));

            //Act
            CollisionResult result = detector.DetectCollision(expectedWinner, new[] { expectedLooser1, expectedLooser2 }).Result;
            var expectedWinnerName = expectedWinner.GetUsername().Result;
            var expectedLooser1Name = expectedLooser1.GetUsername().Result;
            var expectedLooser2Name = expectedLooser2.GetUsername().Result;

            var actualWinnerName = result.Winner.GetUsername().Result;
            var actualLooser1Name = result.Loosers.First().GetUsername().Result;
            var actualLooser2Name = result.Loosers.Last().GetUsername().Result;

            //Assert
            Assert.False(result.IsEmpty());
            Assert.Equal(expectedWinnerName, actualWinnerName);
            Assert.Equal(
                new[] {expectedLooser1Name, expectedLooser2Name}.OrderBy(x => x),
                new[] {actualLooser1Name, actualLooser2Name}.OrderBy(x => x));
        }


        [Fact]
        public void ShouldNotDetectCollision_PlayersAreTooFar()
        {
            //Arange
            var detector = new CollisionDetectingService();

            var expectedWinner = A.Fake<IPlayerActor>();
            A.CallTo(() => expectedWinner.GetUsername()).Returns(nameof(expectedWinner));
            A.CallTo(() => expectedWinner.GetSize()).Returns(2);
            A.CallTo(() => expectedWinner.GetPosition()).Returns(new Vector2D(0, 0));

            var expectedLooser = A.Fake<IPlayerActor>();
            A.CallTo(() => expectedLooser.GetUsername()).Returns(nameof(expectedLooser));
            A.CallTo(() => expectedLooser.GetSize()).Returns(1);
            A.CallTo(() => expectedLooser.GetPosition()).Returns(new Vector2D(10, 10));

            //Act
            CollisionResult result = detector.DetectCollision(expectedWinner, new[] { expectedLooser }).Result;

            //Assert
            Assert.True(result.IsEmpty());
        }

        [Fact]
        public void ShouldNotDetectCollision_PlayersAreSameSize()
        {
            //Arange
            var detector = new CollisionDetectingService();

            var expectedWinner = A.Fake<IPlayerActor>();
            A.CallTo(() => expectedWinner.GetUsername()).Returns(nameof(expectedWinner));
            A.CallTo(() => expectedWinner.GetSize()).Returns(2);
            A.CallTo(() => expectedWinner.GetPosition()).Returns(new Vector2D(0, 0));

            var expectedLooser = A.Fake<IPlayerActor>();
            A.CallTo(() => expectedLooser.GetUsername()).Returns(nameof(expectedLooser));
            A.CallTo(() => expectedLooser.GetSize()).Returns(2);
            A.CallTo(() => expectedLooser.GetPosition()).Returns(new Vector2D(0, 1));

            //Act
            CollisionResult result = detector.DetectCollision(expectedWinner, new[] { expectedLooser }).Result;

            //Assert
            Assert.True(result.IsEmpty());
        }
    }
}