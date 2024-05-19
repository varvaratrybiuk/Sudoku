using System;
using System.Reflection.Emit;

namespace Solver
{
    public class Generator
    {
        public int[,] SolvedSudoku { get; private set; }
        private readonly SudokuLvl _level;
        private readonly int SudokuSize;
        private static readonly Random _random = new Random();

        public Generator(SudokuLvl level)
        {
            _level = level;
            SudokuSize = (int)_level;
            SolvedSudoku = new int[SudokuSize, SudokuSize];
        }

        public bool Generate()
        {
            var array = ShuffleMix();
            return GenerateHelper(0, 0, array);
        }

        private bool GenerateHelper(int i, int j, List<int> array)
        {
            if (i == SudokuSize)
            {
                i = 0;
                if (++j == SudokuSize)
                    return true;
            }
            if (SolvedSudoku[i, j] != 0)
                return GenerateHelper(i + 1, j, array);

            foreach (var number in array)
            {
                if (IsSafe(number, i, j))
                {
                    SolvedSudoku[i, j] = number;
                    if (GenerateHelper(i + 1, j, array))
                        return true;
                    SolvedSudoku[i, j] = 0;
                }
            }
            return false;
        }

        private List<int> ShuffleMix()
        {
            var numbers = Enumerable.Range(1, SudokuSize).ToList();
            for (int i = numbers.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
            }
            return numbers;
        }

        private bool IsSafe(int number, int row, int col)
        {
            return IsRowSafe(number, row) && IsColumnSafe(number, col) && IsBoxSafe(number, row, col);
        }

        private bool IsRowSafe(int number, int row)
        {
            for (int col = 0; col < SudokuSize; col++)
            {
                if (SolvedSudoku[row, col] == number)
                    return false;
            }
            return true;
        }

        private bool IsColumnSafe(int number, int col)
        {
            for (int row = 0; row < SudokuSize; row++)
            {
                if (SolvedSudoku[row, col] == number)
                    return false;
            }
            return true;
        }

        private bool IsBoxSafe(int number, int row, int col)
        {
            int boxSize = (int)Math.Sqrt(SudokuSize);
            int startRow = row - row % boxSize;
            int startCol = col - col % boxSize;

            for (int i = 0; i < boxSize; i++)
            {
                for (int j = 0; j < boxSize; j++)
                {
                    if (SolvedSudoku[startRow + i, startCol + j] == number)
                        return false;
                }
            }
            return true;
        }
    }
}
