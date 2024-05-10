using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hints
{
    public class OpenRandomCell : IHint
    {
        private int _FieldSize;

        public OpenRandomCell(int fieldSize)
        {
            _FieldSize = fieldSize;
        }
        public int[] Execute()
        {
            Random random = new Random();
            int i = random.Next(1, _FieldSize);
            int j = random.Next(1, _FieldSize);
            return [i, j];
        }
    }
}
