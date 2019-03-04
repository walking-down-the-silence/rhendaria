using System.Linq;
using System.Threading.Tasks;
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
        public async void ShouldDetectCollision_BiggerPlayerWins()
        {
            // Arange
            var detector = new CollisionDetectingService();

            var expectedWinner = A.Fake<IPlayerActor>();
            var expectedWinnerPosition = new Vector2D(3, 3);
            var expectedWinnerState = new PlayerInfo
            {
                Nickname = nameof(expectedWinner),
                Position = expectedWinnerPosition,
                SpriteSize = 2
            };
            A.CallTo(() => expectedWinner.GetState()).Returns(Task.FromResult(expectedWinnerState));
            A.CallTo(() => expectedWinner.GetPosition()).Returns(expectedWinnerPosition);

            var expectedLooser = A.Fake<IPlayerActor>();
            var expectedLooserPosition = new Vector2D(3, 4);
            var expectedLooserState = new PlayerInfo
            {
                Nickname = nameof(expectedLooser),
                Position = expectedLooserPosition,
                SpriteSize = 1
            };
            A.CallTo(() => expectedLooser.GetState()).Returns(Task.FromResult(expectedLooserState));
            A.CallTo(() => expectedLooser.GetPosition()).Returns(expectedLooserPosition);

            // Act
            CollisionResult result = await detector.DetectCollision(expectedWinner, new[] { expectedLooser });
            var expectedWinnerName = expectedWinnerState.Nickname;
            var expectedLooserName = expectedLooserState.Nickname;

            var actualWinner = await result.Winner.GetState();
            var actualLooser = await result.Loosers.First().GetState();

            // Assert
            Assert.False(result.IsEmpty());
            Assert.Equal(expectedWinnerName, actualWinner.Nickname);
            Assert.Equal(expectedLooserName, actualLooser.Nickname);
        }

        [Fact]
        public async void ShouldDetectCollision_TheBiggestPlayerWins()
        {
            // Arange
            var detector = new CollisionDetectingService();

            var expectedWinner = A.Fake<IPlayerActor>();
            var expectedWinnerPosition = new Vector2D(3, 3);
            var expectedWinnerState = new PlayerInfo
            {
                Nickname = nameof(expectedWinner),
                Position = expectedWinnerPosition,
                SpriteSize = 5
            };
            A.CallTo(() => expectedWinner.GetState()).Returns(Task.FromResult(expectedWinnerState));
            A.CallTo(() => expectedWinner.GetPosition()).Returns(expectedWinnerPosition);

            var expectedLooser1 = A.Fake<IPlayerActor>();
            var expectedLooser1Position = new Vector2D(3, 4);
            var expectedLooser1State = new PlayerInfo
            {
                Nickname = nameof(expectedLooser1),
                Position = expectedLooser1Position,
                SpriteSize = 2
            };
            A.CallTo(() => expectedLooser1.GetState()).Returns(Task.FromResult(expectedLooser1State));
            A.CallTo(() => expectedLooser1.GetPosition()).Returns(expectedLooser1Position);

            var expectedLooser2 = A.Fake<IPlayerActor>();
            var expectedLooser2Position = new Vector2D(2, 3);
            var expectedLooser2State = new PlayerInfo
            {
                Nickname = nameof(expectedLooser1),
                Position = expectedLooser2Position,
                SpriteSize = 1
            };
            A.CallTo(() => expectedLooser2.GetState()).Returns(Task.FromResult(expectedLooser2State));
            A.CallTo(() => expectedLooser2.GetPosition()).Returns(expectedLooser2Position);

            // Act
            CollisionResult result = await detector.DetectCollision(expectedWinner, new[] { expectedLooser1, expectedLooser2 });
            var expectedWinnerName = expectedWinnerState.Nickname;
            var expectedLooser1Name = expectedLooser1State.Nickname;
            var expectedLooser2Name = expectedLooser2State.Nickname;

            var actualWinner = await result.Winner.GetState();
            var actualLooser1 = await result.Loosers.First().GetState();
            var actualLooser2 = await result.Loosers.Last().GetState();

            // Assert
            Assert.False(result.IsEmpty());
            Assert.Equal(expectedWinnerName, actualWinner.Nickname);
            Assert.Equal(
                new[] { expectedLooser1Name, expectedLooser2Name }.OrderBy(x => x),
                new[] { actualLooser1.Nickname, actualLooser2.Nickname }.OrderBy(x => x));
        }


        [Fact]
        public async void ShouldNotDetectCollision_PlayersAreTooFar()
        {
            // Arange
            var detector = new CollisionDetectingService();

            var expectedWinner = A.Fake<IPlayerActor>();
            var expectedWinnerPosition = new Vector2D(0, 0);
            var expectedWinnerState = new PlayerInfo
            {
                Nickname = nameof(expectedWinner),
                Position = expectedWinnerPosition,
                SpriteSize = 2
            };
            A.CallTo(() => expectedWinner.GetState()).Returns(Task.FromResult(expectedWinnerState));
            A.CallTo(() => expectedWinner.GetPosition()).Returns(expectedWinnerPosition);

            var expectedLooser = A.Fake<IPlayerActor>();
            var expectedLooserPosition = new Vector2D(10, 10);
            var expectedLooserState = new PlayerInfo
            {
                Nickname = nameof(expectedLooser),
                Position = expectedLooserPosition,
                SpriteSize = 1
            };
            A.CallTo(() => expectedLooser.GetState()).Returns(Task.FromResult(expectedLooserState));
            A.CallTo(() => expectedLooser.GetPosition()).Returns(expectedLooserPosition);

            // Act
            CollisionResult result = await detector.DetectCollision(expectedWinner, new[] { expectedLooser });

            // Assert
            Assert.True(result.IsEmpty());
        }

        [Fact]
        public void ShouldNotDetectCollision_PlayersAreSameSize()
        {
            // Arange
            var detector = new CollisionDetectingService();

            var expectedWinner = A.Fake<IPlayerActor>();
            var expectedWinnerPosition = new Vector2D(0, 0);
            var expectedWinnerState = new PlayerInfo
            {
                Nickname = nameof(expectedWinner),
                Position = expectedWinnerPosition,
                SpriteSize = 2
            };
            A.CallTo(() => expectedWinner.GetState()).Returns(Task.FromResult(expectedWinnerState));
            A.CallTo(() => expectedWinner.GetPosition()).Returns(expectedWinnerPosition);

            var expectedLooser = A.Fake<IPlayerActor>();
            var expectedLooserPosition = new Vector2D(0, 1);
            var expectedLooserState = new PlayerInfo
            {
                Nickname = nameof(expectedLooser),
                Position = expectedLooserPosition,
                SpriteSize = 2
            };
            A.CallTo(() => expectedLooser.GetState()).Returns(Task.FromResult(expectedLooserState));
            A.CallTo(() => expectedLooser.GetPosition()).Returns(expectedLooserPosition);

            // Act
            CollisionResult result = detector.DetectCollision(expectedWinner, new[] { expectedLooser }).Result;

            // Assert
            Assert.True(result.IsEmpty());
        }
    }
}