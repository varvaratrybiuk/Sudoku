using Hints;
using Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SudokuComponents
{
    public class SudokuGame
    {
        public Board Board { get; }
        private IHint? _hint;
        private Generator _solver;

        public SudokuGame(SudokuLvl lvl)
        {
            Board = new Board((int)lvl);
            _solver = new Generator(lvl);
        }

        public void StartGame()
        {
            _solver.Generate();
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            int size = _solver.SolvedSudoku.GetLength(0);
            var random = new Random();
            for (int i = 0; i < size; i++)
            {
                int j = random.Next(1, size);
                Board.TryFillCell(new Cell([i, j], _solver.SolvedSudoku[i, j]));
            }
        }

        public void AddToBoard(int[] values, int n)
        {
            Board.Draft.Add(new Cell(values, n));
        }

        public bool OpenCell(IHint hint)
        {
            var result = hint.Execute();
            if (result != null)
            {
                if (!Board.TryFillCell(new Cell(result, _solver.SolvedSudoku[result[0], result[1]])))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool Check(int[] values, int n)
        {
            int i = values[0];
            int j = values[1];
            return _solver.SolvedSudoku[i, j] == n && Board.TryFillCell(new Cell(values, n));
        }

        public bool FullBoard()
        {
            return Board.Cells.Count == _solver.SolvedSudoku.Length;
        }

        public List<Cell> GetCells()
        {
            return Board.Cells;
        }

        public List<Cell> GetDraft()
        {
            return Board.Draft;
        }
    }
}
