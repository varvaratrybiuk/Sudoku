using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hints
{
    public class OpenRandomCell : IHint
    {
        private int[,] _openedCells;

        public OpenRandomCell(int[,] openedCells)
        {
            _openedCells = openedCells;
        }

        public int[] Execute()
        {
            List<(int, int)> freeCells = new List<(int, int)>();

            for (int i = 0; i < _openedCells.GetLength(0); i++)
            {
                for (int j = 0; j < _openedCells.GetLength(1); j++)
                {
                    if (_openedCells[i, j] == 0)
                        freeCells.Add(new(i, j));
                }
            }

            if (freeCells.Count == 0)
            {
                return null; // no free cells
            }

            Random random = new Random();

            int randomIndex = random.Next(0, freeCells.Count);

            int row = freeCells[randomIndex].Item1;

            int col = freeCells[randomIndex].Item2;
            return [row, col];
        }
    }
}
