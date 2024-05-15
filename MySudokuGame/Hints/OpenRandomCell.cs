using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hints
{
    public class OpenRandomCell : IHint
    {
        private List<int[]> _usedCells;
        private int _size;

        public OpenRandomCell(List<int[]> usedCells, int size)
        {
            _usedCells = usedCells;
            _size = size;
        }

        public int[] Execute()
        {
            var random = new Random();
            int i, j;
            var availableCells = Enumerable.Range(0, _size)
                                            .SelectMany(row => Enumerable.Range(0, _size).Select(col => new int[] { row, col }))
                                            .Where(cell => !_usedCells.Any(uc => uc[0] == cell[0] && uc[1] == cell[1]))
                                            .ToList();
            if (availableCells.Count == 0)
            {
                return null;
            }
            int index = random.Next(0, availableCells.Count);
            int[] cell = availableCells[index];
            return cell;
        }
    }
}
