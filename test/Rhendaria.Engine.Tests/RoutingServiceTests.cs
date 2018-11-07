using System;
using FakeItEasy;
using Microsoft.Extensions.Options;
using Rhendaria.Abstraction;
using Rhendaria.Engine.Services;
using Xunit;

namespace Rhendaria.Engine.Tests
{
    public class RoutingServiceTests
    {
        [Fact]
        public void GetZoneId_size100_x0_y0_Expected_zone_0_0()
        {
            // Arrange.
            var options = GetFakeConstant(100);
            RoutingService service = new RoutingService(options);
            Vector2D position = new Vector2D(0, 0);

            // Act.
            string resultZoneId = service.GetZoneId(position);

            // Assert.
            Assert.Equal("zone_0_0", resultZoneId);
        }

        [Fact]
        public void GetZoneId_size100_x100_y0_Expected_zone_1_0()
        {
            // Arrange.
            var options = GetFakeConstant(100);
            RoutingService service = new RoutingService(options);
            Vector2D position = new Vector2D(100, 0);

            // Act.
            string resultZoneId = service.GetZoneId(position);

            // Assert.
            Assert.Equal("zone_1_0", resultZoneId);
        }

        [Fact]
        public void GetZoneId_size100_x0_y100_Expected_zone_0_1()
        {
            // Arrange.
            var options = GetFakeConstant(100);
            RoutingService service = new RoutingService(options);
            Vector2D position = new Vector2D(0, 100);

            // Act.
            string resultZoneId = service.GetZoneId(position);

            // Assert.
            Assert.Equal("zone_0_1", resultZoneId);
        }

        [Fact]
        public void GetZoneId_size100_xMinus1_yMinus1_Expected_zone_0_0()
        {
            // Arrange.
            var options = GetFakeConstant(100);
            RoutingService service = new RoutingService(options);
            Vector2D position = new Vector2D(-1, -1);

            // Act.
            string resultZoneId = service.GetZoneId(position);

            // Assert.
            Assert.Equal("zone_0_0", resultZoneId);
        }

        [Fact]
        public void GetZoneId_size100_x1_y1_Expected_zone_0_0()
        {
            // Arrange.
            var options = GetFakeConstant(100);
            RoutingService service = new RoutingService(options);
            Vector2D position = new Vector2D(1, 1);

            // Act.
            string resultZoneId = service.GetZoneId(position);

            // Assert.
            Assert.Equal("zone_0_0", resultZoneId);
        }

        [Fact]
        public void GetZoneId_size100_xMinus100_yMinus100_Expected_zone_Minus1_Minus1()
        {
            // Arrange.
            var options = GetFakeConstant(100);
            RoutingService service = new RoutingService(options);
            Vector2D position = new Vector2D(-100, -100);

            // Act.
            string resultZoneId = service.GetZoneId(position);

            // Assert.
            Assert.Equal("zone_-1_-1", resultZoneId);
        }

        [Fact]
        public void GetZoneId_positionNull_Expected_exception()
        {
            // Arrange.
            var options = GetFakeConstant(100);
            RoutingService service = new RoutingService(options);
            Vector2D position = null;

            // Act, Assert.
            Assert.Throws<ArgumentNullException>(()=>service.GetZoneId(position));
        }

        private IOptions<GameOptions> GetFakeConstant(int width, int? height = null)
        {
            if (height == null)
            {
                height = width;
            }

            GameOptions options = new GameOptions
            {
                ZoneHeight = height.Value,
                ZoneWidth = width
            };

            return Options.Create(options);
        }
    }
}
