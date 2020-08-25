using Xunit;

namespace Battleship.Test
{
    public class PlayerTest
    {
        [Fact]
        void Test_CheckPlayerHasShips_ReturnsTrue()
        {
            Player player = new Player("Tester");
            bool hasShips = player.HasShips();

            Assert.True(hasShips);
        }

        [Fact]
        void Test_PlaceAndDamageAndSinkShip_ReturnsHasShipsFalse()
        {
            Player player = new Player("Tester");
            ShipType ship = player.GetShips()[0];
            bool hasShips = player.HasShips();
            Assert.True(hasShips);
            
            bool placed = player.PlaceShip(ship, 0, 0, false);
            Assert.True(placed);
            
            for (uint i = 0; i < 3; i++)
            {
                bool hit = player.CheckHit(i, 0);
                Assert.True(hit);
            }
            
            hasShips = player.HasShips();
            Assert.False(hasShips);
        }
    }
}
