using Xunit;

namespace Battleship.Test
{
    public class ShipTest
    {
        [Fact]
        public void Test_DamageShipHalfway_ShouldNotSink()
        {
            Ship ship = new Ship(2, ShipType.BATTLESHIP);
            bool sunk = ship.TakeDamage(1);

            Assert.False(sunk);
        }

        [Fact]
        public void Test_DamageShipFully_ShouldSink()
        {
            Ship ship = new Ship(2, ShipType.BATTLESHIP);
            bool sunk = ship.TakeDamage(2);

            Assert.True(sunk);
        }

        [Fact]
        public void Test_CheckPosition_ShouldNotBeAtPosition()
        {
            Ship ship = new Ship(2, ShipType.BATTLESHIP);
            ship.Place(0, 0, true);
            bool isAtPosition = ship.CheckPosition(1, 1);

            Assert.False(isAtPosition);
        }

        [Fact]
        public void Test_CheckPosition_ShouldBeAtPosition()
        {
            Ship ship = new Ship(2, ShipType.BATTLESHIP);
            ship.Place(0, 0, true);
            bool isAtPosition = ship.CheckPosition(0, 0);

            Assert.True(isAtPosition);
        }
    }
}
