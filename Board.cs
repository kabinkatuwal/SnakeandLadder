using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeandLadder
{
    internal class Board
    {
        [cite_start]// Dictionary to store all 100 cells (Map<int, Cell> cells) 
        private readonly Dictionary<int, Cell> cells = new Dictionary<int, Cell>();

        // List<Snake> and List<Ladder> (cite: 81)
        private readonly List<Snake> snakes = new List<Snake>();
        private readonly List<Ladder> ladders = new List<Ladder>();

        // Quick look-up for special landings (head/bottom -> tail/top)
        private readonly Dictionary<int, int> specialPositions = new Dictionary<int, int>();

        public Board()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // 1. Initialize 100 cells (1 to 100)
            for (int i = 1; i <= 100; i++)
            {
                cells[i] = new Cell() { Index = i };
            }

            // 2. Hardcode some Snakes and Ladders (MVP implementation)
            // LADDERS (Bottom to Top)
            AddSpecialItem(4, 14, isLadder: true);
            AddSpecialItem(9, 31, isLadder: true);
            AddSpecialItem(20, 38, isLadder: true);
            AddSpecialItem(28, 84, isLadder: true);

            // SNAKES (Head to Tail)
            AddSpecialItem(99, 54, isLadder: false);
            AddSpecialItem(70, 50, isLadder: false);
            AddSpecialItem(52, 11, isLadder: false);
            AddSpecialItem(48, 9, isLadder: false);
        }

        private void AddSpecialItem(int start, int end, bool isLadder)
        {
            specialPositions[start] = end;

            if (isLadder)
            {
                ladders.Add(new Ladder() { Bottom = start, Top = end });
                cells[start].HasLadder = true;
            }
            else // Is Snake
            {
                snakes.Add(new Snake() { Head = start, Tail = end });
                cells[start].HasSnake = true;
            }
        }

        /// <summary>
        /// Checks if a position is the start of a snake or ladder and returns the new position.
        /// </summary>
        public int GetNewPosition(int pos) // (cite: 82)
        {
            if (specialPositions.TryGetValue(pos, out int newPos))
            {
                return newPos;
            }
            return pos; // Returns the same position if no snake or ladder is found
        }

        // Properties for UI to access the list of snakes and ladders for drawing
        public IEnumerable<Ladder> Ladders => ladders;
        public IEnumerable<Snake> Snakes => snakes;
    }
}
