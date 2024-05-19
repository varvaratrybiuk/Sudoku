using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hints
{
    public class OpenSpecificCell : IHint
    {
        private readonly int _row;
        private readonly int _col;

        public OpenSpecificCell(int row, int col)
        {
            _row = row;
            _col = col;
        }

        public int[] Execute()
        {
            return new[] { _row, _col };
        }
    }
}
