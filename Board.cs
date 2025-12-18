using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeandLadder
{
    public class Board
    {
        private readonly Dictionary<int, Cell> cells = new Dictionary<int, Cell>();
        private readonly List<Snake> snakes = new List<Snake>();
        private readonly List<Ladder> ladders = new List<Ladder>();
        private readonly Dictionary<int, int> specialPositions = new Dictionary<int, int>();

        public IEnumerable<Ladder> Ladders => ladders;
        public IEnumerable<Snake> Snakes => snakes;

        public Board()
        {
            InitializeCells();
            SetupManualBoard();
        }

        private void InitializeCells()
        {
            cells.Clear();
            specialPositions.Clear();
            snakes.Clear();
            ladders.Clear();

            for (int i = 1; i <= 100; i++)
            {
                cells[i] = new Cell() { Index = i };
            }
        }

        private void SetupManualBoard()
        {
            AddSpecialItem(1, 38, true);
            AddSpecialItem(4, 14, true);
            AddSpecialItem(9, 31, true);
            AddSpecialItem(21, 42, true);
            AddSpecialItem(28, 84, true);
            AddSpecialItem(36, 44, true);
            AddSpecialItem(51, 67, true);
            AddSpecialItem(71, 91, true);
            AddSpecialItem(80, 100, true);

            AddSpecialItem(16, 6, false);
            AddSpecialItem(48, 30, false);
            AddSpecialItem(62, 19, false);
            AddSpecialItem(64, 60, false);
            AddSpecialItem(87, 24, false);
            AddSpecialItem(93, 73, false);
            AddSpecialItem(95, 75, false);
            AddSpecialItem(98, 79, false);
        }

        private void AddSpecialItem(int start, int end, bool isLadder)
        {
            specialPositions[start] = end;

            if (isLadder)
            {
                ladders.Add(new Ladder() { Bottom = start, Top = end });
            }
            else
            {
                snakes.Add(new Snake() { Head = start, Tail = end });
            }
        }

        public int GetNewPosition(int pos)
        {
            return specialPositions.ContainsKey(pos) ? specialPositions[pos] : pos;
        }
    }
}