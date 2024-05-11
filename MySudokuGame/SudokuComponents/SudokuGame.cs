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
        private Board board;
        private IHint? _hint;
        private Generator _solver;
        public SudokuGame(SudokuLvl lvl)
        {
            this.board = new Board((int)lvl);
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
                board.TryFillCell(new Cell([i, j], _solver.SolvedSudoku[i, j]));
            }
        }
        public void AddToBoard(int[] values, int n)
        {
            board.Draft.Add(new Cell(values, n));
        }
        public bool OpenCell(IHint hint)
        {
            var result = hint.Execute();
            if (result != null)
            {
                if (!board.TryFillCell(new Cell(result, _solver.SolvedSudoku[result[0], result[1]])))
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
            if (_solver.SolvedSudoku[i, j] == n)
            {
                board.TryFillCell(new Cell(values, n));
                return true;
            }
            return false;
        }
        public List<Cell> GetCells()
        {
            return board.cells;
        }
        public List<Cell> GetDraft()
        {
            return board.Draft;
        }
    }

}
