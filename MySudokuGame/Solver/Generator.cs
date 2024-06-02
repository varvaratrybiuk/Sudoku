using System;
using System.Reflection.Emit;

namespace Solver
{
    public class Generator
    {
        public int[,] SolvedSudoku { get; private set; }
        private SudokuLvl _level;
        private int SudokeSize;
        public Generator(SudokuLvl level)
        {
            _level = level;
            SudokeSize = (int)_level;
            SolvedSudoku = new int[(int)_level, (int)_level];
        }

        public bool Generate()
        {
            var array = ShuffleMix();
            return GenerateHelper(0, 0, array);
        }

        private bool GenerateHelper(int i, int j, List<int> array)
        {
            if (i == SudokeSize)
            {
                i = 0;
                if (++j == SudokeSize)
                    return true;
            }
            if (SolvedSudoku[i, j] != 0)
                return GenerateHelper(i + 1, j, array);

            foreach (var number in array)
            {
                if (isSafe(number, i, j))
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
            var numbers = Enumerable.Range(1, SudokeSize).ToList();
            Random rad = new Random();
            for (int i = numbers.Count - 1; i >= 1; i--)
            {
                int j = rad.Next(i + 1);
                int tmp = numbers[j];
                numbers[j] = numbers[i];
                numbers[i] = tmp;
            }
            return numbers;
        }
        private bool isSafe(int number, int i, int j)
        {

            for (int x = 0; x < SudokeSize; x++)
            {
                if (SolvedSudoku[i, x] == number)
                    return false;
            }

            for (int x = 0; x < SudokeSize; x++)
            {
                if (SolvedSudoku[x, j] == number)
                    return false;
            }

            int startRow = i - i % (int)Math.Sqrt(SudokeSize);
            int startCol = j - j % (int)Math.Sqrt(SudokeSize);
            for (int K = 0; K < Math.Sqrt(SudokeSize); K++)
            {
                for (int L = 0; L < Math.Sqrt(SudokeSize); L++)
                {
                    if (SolvedSudoku[K + startRow, L + startCol] == number)
                        return false;
                }
            }
            return true;

        }

    }

}
