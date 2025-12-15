// Board.cs
using System;
using System.Collections.Generic;
using System.Linq;
using SnakeandLadder;

namespace SnakeandLadder
{
    public class Board
    {
        private readonly Dictionary<int, Cell> cells = new Dictionary<int, Cell>();
        private readonly List<Snake> snakes = new List<Snake>();
        private readonly List<Ladder> ladders = new List<Ladder>();
        private readonly Dictionary<int, int> specialPositions = new Dictionary<int, int>();

        private static readonly Random random = new Random();
        private const int MAX_CELL = 100;
        private const int MIN_CELL = 2;
        public IEnumerable<Ladder> Ladders => ladders;
        public IEnumerable<Snake> Snakes => snakes;

        public Board()
        {
            GenerateRandomBoard(numSnakes: 8, numLadders: 8);
        }

        // Constructor used for loading saved games
        public Board(List<Snake> loadedSnakes, List<Ladder> loadedLadders)
        {
            InitializeCells();
            LoadBoardSetup(loadedSnakes, loadedLadders);
        }

        private void InitializeCells()
        {
            cells.Clear();
            for (int i = 1; i <= 100; i++)
            {
                cells[i] = new Cell() { Index = i };
            }
            specialPositions.Clear();
            snakes.Clear();
            ladders.Clear();
        }

        public void LoadBoardSetup(List<Snake> loadedSnakes, List<Ladder> loadedLadders)
        {
            InitializeCells();
            foreach (var s in loadedSnakes) AddSpecialItem(s.Head, s.Tail, isLadder: false);
            foreach (var l in loadedLadders) AddSpecialItem(l.Bottom, l.Top, isLadder: true);
        }

        public void GenerateRandomBoard(int numSnakes, int numLadders)
        {
            InitializeCells();

            // --- Generate Ladders ---
            for (int i = 0; i < numLadders; i++)
            {
                int bottom, top;
                do
                {
                    bottom = random.Next(MIN_CELL, MAX_CELL - 20);
                    top = random.Next(bottom + 10, MAX_CELL - 1);
                } while (specialPositions.ContainsKey(bottom) || specialPositions.ContainsKey(top) || bottom >= top);

                AddSpecialItem(bottom, top, isLadder: true);
            }

            // --- Generate Snakes ---
            for (int i = 0; i < numSnakes; i++)
            {
                int head, tail;
                do
                {
                    head = random.Next(MAX_CELL / 5, MAX_CELL - 1);
                    tail = random.Next(MIN_CELL, head - 10);
                } while (specialPositions.ContainsKey(head) || specialPositions.ContainsKey(tail) || head <= tail || head == MAX_CELL);

                AddSpecialItem(head, tail, isLadder: false);
            }
        }

        private void AddSpecialItem(int start, int end, bool isLadder)
        {
            specialPositions[start] = end;
            if (isLadder)
            {
                ladders.Add(new Ladder() { Bottom = start, Top = end });
                cells[start].HasLadder = true;
            }
            else
            {
                snakes.Add(new Snake() { Head = start, Tail = end });
                cells[start].HasSnake = true;
            }
        }

        public int GetNewPosition(int pos)
        {
            if (specialPositions.TryGetValue(pos, out int newPos))
            {
                return newPos;
            }
            return pos;
        }

       
    }
}