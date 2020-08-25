/** 
 * This file contains the logic for creating ships for the game. The enum 
 * ShipType replaces the need to inherit multiple classes from Ship, since no
 * value would be gained by inheriting since all ships are functionally the
 * same. The only difference betweeen ships is their health.
 */

using System;
using System.Collections.Generic;

namespace Battleship
{
    /** ShipType is an enumeration of all types of ships that can be created. */
    public enum ShipType
    {
        BATTLESHIP,
        SUBMARINE,
        PATROL_BOAT,
        DESTROYER,
        CARRIER
    }

    /** Ship defines the actual object in play, which has a position and takes damage. */
    public class Ship
    {
        public Ship(int health, ShipType type)
        {
            _health = health;
            _type = type;
            _positions = new List<uint[]>(_health);
            for (int i = 0; i < _health; ++i)
                _positions.Add(new uint[2] { 0, 0 });
        }

        /** <summary>Span returns the ship's health since in the physical realm, a ship is as long as it's health.</summary>
         * <returns>THe span/length of the ship.</returns>
         */
        public uint Span()
        {
            return (uint)_health;
        }

        /** <summary>Get the type of the ship.</summary>
         * <returns>The type of the ship.</returns>
         */
        public ShipType GetShipType()
        {
            return _type;
        }

        /** <summary>Place sets the coordinates of the ship according to the health of the ship.</summary>
         * <param name="x">The column component of the position. This should be checked before being added to make sure the value does not extend th board.</param>
         * <param name="y">The row component of the position. This should be checked before being added to make sure the value does not extend th board.</param>
         * <param name="vertically">Defines whether or not the ship shold be placed vertically.</param>
         */
        public void Place(uint x, uint y, bool vertically = false)
        {
            for (uint i = 0; i < _health; ++i)
                _positions[(int)i] = new uint[2] {y + (vertically ? i : 0), x + (!vertically ? i : 0) };
        }

        /**<summary>Checks that the given position is one that the ship occupies.</summary>
         * <param name="x">The column component of the coordinates.</param>
         * <param name="y">The row component of the coordinates.</param>
         * <returns>True if the ship occupies the given coordinates. False otherwise.</returns>
         */
        public bool CheckPosition(uint x, uint y)
        {
            return _positions.Exists(pos => pos[0] == y && pos[1] == x);
        }

        /** <summary>Handles the ship being hit.</summary>
         * <returns>True if the damage done has reduced the ship's health to 0.</returns>
         */
        public bool TakeDamage(int DamageIn)
        {
            _health = Math.Max(0, _health - DamageIn);
            return _health == 0;
        }

        private readonly ShipType _type;
        private int _health = 1;
        private List<uint[]> _positions;
    }
}
