using System.Collections.Generic;

namespace Battleship
{
    public class Player
    {
        public Player(string name)
        {
            _name = name;
            _board = new Board(10);
            _ships = new List<Ship> {
                //new Ship(5, ShipType.CARRIER),
                //new Ship(4, ShipType.BATTLESHIP),
                new Ship(3, ShipType.DESTROYER),
                //new Ship(3, ShipType.SUBMARINE),
                //new Ship(2, ShipType.PATROL_BOAT)
            };
        }

        /**<summary>Checks to see whether or not the player has any ships left.</summary>
         * <returns>True if the player has any ships left. False otherwise.</returns>
         */
        public bool HasShips()
        {
            return _ships.Count != 0;
        }

        /** <summary>Get's the full name of the player, including rank.</summary> 
         * <returns>The name of the player prepended with their new rank.</returns>
         */
        public string GetName()
        {
            return "Admiral " + _name;
        }

        /** <summary>Gets all the ship types in the player's fleet. This does not return references to the ships themeselves.</summary>
         * <returns>A list of all the ship types in the player's fleet.</returns>
         */
        public List<ShipType> GetShips()
        {
            List<ShipType> types = new List<ShipType>();
            foreach (Ship ship in _ships)
                types.Add(ship.GetShipType());
            return types;
        }

        /** <summary>A wrapper method for the player's board object.</summary>
         * <param name="showShips">Flag which checks if the player's ship positions should be reveled.</param>
         * <returns>The player's board with or without the ships.</returns>
         */
        public string GetBoard(bool showShips = false)
        {
            return _board.Get(showShips);
        }

        /** <summary>Checks the given coordinates to see if there was a hit made.</summary>
         * <param name="x">The column component of the coordinates.</param>
         * <param name="y">The row component of the coordinates.</param>
         */
        public bool CheckHit(uint x, uint y)
        {
            if (_board.MarkTile(x, y))
            {
                Ship ship = _ships.Find(ship => ship.CheckPosition(x, y));
                if (ship != null)
                {
                    if (ship.TakeDamage(1))
                        _ships.Remove(ship);
                    return true;
                }
               return false;
            }
            return false;
        }

        /** <summary>Attempts to place the specified ship model starting at the given coordinates.</summary>
         * <param name="type">The type of the ship to placed</param>
         * <param name="x">The column component of the coordinates.</param>
         * <param name="y">The row component of the coordinates.</param>
         * <param name="vertically">Flag to specify if the ship should be placed vertically.</param>
         * <returns>True if the placement was successful. False otherwise.</returns>
         */
        public bool PlaceShip(ShipType type, uint x, uint y, bool vertically)
        {
            // Find the correct ship.
            Ship ship = _ships.Find(s => s.GetShipType() == type);
            if (_board.MarkShip(ship.Span(), x, y, vertically))
            {
                ship.Place(x, y, vertically);
                return true;
            }
            return false;

        }

        private readonly Board _board;
        private readonly string _name;
        private readonly List<Ship> _ships;
    }
}
