using Xunit;

namespace Battleship.Test
{
    public class BoardTest
    {
        [Fact]
        public void Test_MarkTileShip_ShouldPlaceShip()
        {
            Board board = new Board(1);
            bool placed = board.MarkShip(1, 0, 0, false);

            Assert.True(placed);
        }

        [Fact]
        public void Test_MarkTileShip_ShouldNotPlaceShip()
        {
            Board board = new Board(1);
            board.MarkShip(1, 0, 0, false);
            bool placed = board.MarkShip(1, 0, 0, false);

            Assert.False(placed);
        }

        [Fact]
        public void Test_MarkTileMissTwice_ShouldMarkFirstCorrectly()
        {
            Board board = new Board(1);
            bool miss = board.MarkTile(0, 0);
            bool fail = board.MarkTile(0, 0);

            Assert.True(miss);
            Assert.False(fail);
        }

        [Fact]
        public void Test_MarkTileHitTwice_ShouldMarkFirstCorrectly()
        {
            Board board = new Board(1);
            bool placed = board.MarkShip(1, 0, 0, false);
            bool hit = board.MarkTile(0, 0);
            bool fail = board.MarkTile(0, 0);

            Assert.True(placed);
            Assert.True(hit);
            Assert.False(fail);
        }

        [Fact]
        public void Test_BoardToString_ReturnsCorrectString()
        {
            Board board = new Board(1);
            string str = board.Get();

            Assert.Equal("  A\n0 .\n", str);
        }
    }
}
