using System;
using System.Collections.Generic;
using System.Threading;

namespace Battleship
{
    class Game
    {
        /** <summary>The looping method that runs the entire game.</summary> */
        public void Run()
        {
            Start();
            while (!Done())
            {
                for (int i = 0; i < 2 && !Done(); ++i)
                {
                    Player player = _players[i], next_player = _players[(i + 1) % 2];
                    StartTurn(player, next_player);
                    {
                        int ship_count = next_player.GetShips().Count;
                        uint x, y;
                        ReadCoordinates(out x, out y);
                        bool hit = next_player.CheckHit(x, y);
                        bool sunk = next_player.GetShips().Count != ship_count;

                        Displayln($"<*> Opponent reported {(sunk ? "SUNK" : hit ? "HIT" : "MISS")}", true);
                    }
                    EndTurn(player, next_player);
                }
            }
            End();
        }

        /** <summary> Checks to see if a player has lost.</summary>
         * <returns>True if there exists a player who's fleet is empty.</returns>
         */
        private bool Done()
        {
            return _players.Exists(p => !p.HasShips());
        }

        #region setup and teardown
        /** <summary>Private helper method which runs at the start of the game.</summary> */
        private void Start()
        {
            // Get all the players names and init the player objects.
            for (int i = 0; i < 2; ++i)
            {
                Displayln("=== Battleship ===", true);
                Displayln("Welcome to the war! Who are the admirals?");
                Display("--> Admiral #" + Convert.ToString(i+1) + ": ");

                string name;
                ReadLine(out name);

                _players[i] = new Player(name);
            }

            // Iterate through players and allow them to place their ships.
            foreach (Player player in _players)
            {
                Displayln("=== Battleship ===", true);
                Displayln(player.GetName() + ", position your ships!");

                foreach (ShipType ship in player.GetShips())
                {
                    bool valid = true;
                    do
                    {
                        uint x, y;
                        bool bIsVertical;
                        ReadPlacementCoordinates(ship, out x, out y, out bIsVertical);
                        valid = player.PlaceShip(ship, x, y, bIsVertical);
                        if (!valid)
                            Displayln("\n<!> Sir, that spot is impossible!");
                    } while (!valid);
                }
            }
        }

        /** <summary>Private helper method which runs at the end of the game.</summary> */
        private void End()
        {
            Thread.Sleep(3000);
            Displayln("=== Game Over ===", true);
            Player winner = _players.Find(p => p.HasShips());
            Displayln($"<^> {winner.GetName()}, congratulations sir!");
        }
        #endregion

        #region turns
        /** <summary>Private helper method which runs at the start of every turn.</summary>
         * <param name="player">The current player.</param>
         * <param name="next_player">The next player / the current opponent being fired upon.</param>
         */
        private void StartTurn(Player player, Player next_player)
        {
            Displayln($" === {player.GetName()}'s Turn ===", true);
            Displayln("    - Enemy Fleet -");
            Displayln($"{next_player.GetBoard()}");

            Displayln("    - Your Fleet -");
            Displayln($"{player.GetBoard(true)}");
        }

        /** <summary>Private helper method which runs at the end of every turn.</summary>
         * <param name="player">The current player.</param>
         * <param name="next_player">The next player / the current opponent being fired upon.</param>
         */
        private void EndTurn(Player player, Player next_player)
        {
            Displayln("    - Enemy Fleet -");
            Displayln($"{next_player.GetBoard()}");

            Displayln("    - Your Fleet -");
            Displayln($"{player.GetBoard(true)}");

            if (!Done())
            {
                Thread.Sleep(3000);
                Displayln($"<!> Hand over controller to {next_player.GetName()}", true);
                Thread.Sleep(2000);
            }
        }
        #endregion

        #region reader wrappers
        /** <summary>This is a wrapper method for displaying strings.</summary>
         * <param name="str">The string to display.</param>
         */
        private void Display(string str)
        {
            Console.Write(str);
        }

        /** <summary>This is a wrapper method for displaying strings, and clearing the screen on demand.</summary>
         * <param name="str">The string to display.</param>
         * <param name="clear">Flag that clears the screens when set to true.</param>
         */
        private void Displayln(string str, bool clear = false)
        {
            if (clear)
                Console.Clear();
            Console.WriteLine(str);
        }

        /** <summary>A wrapper method for reading in input to a referenced string.</summary> */
        private void ReadLine(out string output)
        {
            output = Console.ReadLine();
        }
        #endregion

        #region reading methods
        /** <summary>A private helper method for reading in user specified starting coordinates.</summary>
         * <param name="type">The type of ship we are attempting to place.</param>
         * <param name="x">The column component of the coordinates.</param>
         * <param name="y">The row component of the coordinates.</param>
         * <param name="vertical">Flag to check if the ship should be placed vertically or not.</param>
         */
        private void ReadPlacementCoordinates(ShipType type, out uint x, out uint y, out bool vertical)
        {
            Displayln($"Where shall we position the {type.ToString()}, and should it be vertical?");
            Display("--> Enter position (A 0) and verticality (yes/no) (ex: A 0 yes): ");

            bool invalid;
            do
            {
                invalid = false;

                string input;
                ReadLine(out input);

                invalid = string.IsNullOrEmpty(input);

                string[] split_input = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Check that we got valid coordinates
                if (split_input.Length == 3)
                    if (char.IsLetter(split_input[0], 0) && 
                        char.IsDigit(split_input[1], 0) && 
                        (split_input[2].ToLower() == "yes" || split_input[2].ToLower() == "no"))
                    {
                        x = Convert.ToUInt32(char.ToUpper(Convert.ToChar(split_input[0])) - 'A');
                        y = Convert.ToUInt32(split_input[1]);
                        vertical = split_input[2].ToLower() == "yes";
                        return;
                    }

                invalid = true;

                // We failed to get our coordinates, let's go again.
                x = y = 0;
                vertical = false;
                Displayln("\n<!> Please sir, use the coordinate system!\n");
                Displayln($"Where shall we position the {type.ToString()}?");
                Display("--> Enter position (ex: A 0): ");
            } while (invalid);
        }

        /** <summary>A private helper method for reading in user specified coordinates.</summary>
         * <param name="x">The column component of the coordinates.</param>
         * <param name="y">The row component of the coordinates.</param>
         */
        private void ReadCoordinates(out uint x, out uint y)
        {
            Displayln("Where shall we fire sir?");
            Display("--> Enter position (ex: A 0): ");

            bool invalid;
            do
            {
                invalid = false;

                string input;
                ReadLine(out input);

                invalid = string.IsNullOrEmpty(input);

                string[] split_input = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Check that we got valid coordinates
                if (split_input.Length == 2)
                    if (char.IsLetter(split_input[0], 0) && char.IsDigit(split_input[1], 0))
                    {
                        x = Convert.ToUInt32(char.ToUpper(Convert.ToChar(split_input[0])) - 'A');
                        y = Convert.ToUInt32(split_input[1]);
                        return;
                    }

                invalid = true;

                // We failed to get our coordinates, let's go again.
                x = y = 0;
                Displayln("\n<!> Please sir, use the coordinate system!\n");
                Displayln("Where shall we fire sir?");
                Display("--> Enter position (ex: A 0): ");
            } while (invalid);
        }
        #endregion

        private List<Player> _players = new List<Player>(2) { null, null };
    }
}
