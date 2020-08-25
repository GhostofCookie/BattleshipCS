using System;

namespace Battleship
{
    /** Board defines an object which is a set of tiles upon which ships can be "placed". */
    public class Board
    {
        public Board(uint size)
        {
            _size = size;
            _board = new char[size, size];
            for(int i = 0; i < _board.GetLength(0); ++i)
               for(int j = 0; j < _board.GetLength(1); ++j)
                    _board[i, j] = unmarked_tile;
        }

        /** <summary>MarkShip is used to mark where the ship goes on the board. Ships are placed from the left/top most end out to the right/bottom.</summary>
         * <param name="size">The size/length of the ship being placed.</param>
         * <param name="col">The column in which the placement should start.</param>
         * <param name="row">The row in which the ship placment should start.</param>
         * <param name="vertical">Denotes whether or not the ship should be placed vertically.</param>
         * <returns>Whether or not the placement was successful or not. Returns false on incorrect coordinates or on ship overlap.</returns>
         */
        public bool MarkShip(uint size, uint col, uint row, bool vertical)
        {
            // Check that our coordinates are on the board.
            if (size == 0 || size > _size || row >= _size || col >= _size || (row + size - 1 >= _size && vertical) || (col + size - 1 >= _size && !vertical))
                return false;

            for (uint i = 0; i < size; ++i)
            {
                uint y = row + (vertical ? i : 0), x = col + (!vertical ? i : 0);
                // Check to make sure we aren't placing our new ship on an existsing ship.
                if (_board[y, x] == marked_tile_ship)
                {
                    // We've overlapped a ship, which is cheating, so we undo the placement.
                    for (int j = (int)i - 1; j >= 0; --j)
                    {
                        uint ny = row + (vertical ? (uint)j : 0), nx = col + (!vertical ? (uint)j : 0);
                         _board[ny, nx] = unmarked_tile;
                    }
                    return false;
                }
                
                // Spot is available, so p[lace the ship.
               _board[y, x] = marked_tile_ship;
            }
            
            return true;
        }

        /** <summary>MarkTile is used to mark whether or not a tile has been hit or missed.</summary>
         * <param name="col">The column of the corrdinate to mark.</param>
         * <param name="row">The rowof the corrdinate to mark.</param>
         * <returns>True if the tile is markable and thus newly makred, false if the tile has already been marked as a hit or a miss.</returns>
         */
        public bool MarkTile(uint col, uint row)
        {
            if (_board[row, col] == marked_tile_hit || _board[row, col] == marked_tile_miss)
                return false;

            _board[row, col] = _board[row, col] == marked_tile_ship ? marked_tile_hit : marked_tile_miss;
            return true;
        }

        /** <summary>Get returns the string version of the board, inclusding row and column numbers and characters.</summary>
         * <param name="showShips">Flag to check whether or not we should reveal the placement of the shiops on the board.</param>
         * <returns>The string version of the board.</returns>
         */
        public string Get(bool showShips = false)
        {
            string str = " ";
            for (int i = 0; i < _board.GetLength(0); ++i)
                str += " " + Convert.ToChar(i + 'A');
            str += '\n';

            for (int i = 0; i < _board.GetLength(0); ++i)
            {
                str += Convert.ToString(i);
                for (int j = 0; j < _board.GetLength(1); ++j)
                    str += " " + (!showShips ? (_board[i, j] == marked_tile_ship ? unmarked_tile : _board[i, j]) : _board[i, j]);
                str += '\n';
            }
            return str;
        }

        private readonly uint _size;
        private readonly char[,] _board;
        private readonly char unmarked_tile = '.';
        private readonly char marked_tile_miss = 'o';
        private readonly char marked_tile_hit = 'x';
        private readonly char marked_tile_ship = '#';
    }
}
