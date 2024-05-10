
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SudokuComponents
{
   public class Board
    {
        public List<Cell> cells { get; private set; } = new List<Cell>();
        public int Size { get; private set; }
        public List<Cell> Draft {  get; set; } = new List<Cell> ();
        public Board(int size) => Size = size;
        public bool TryFillCell(Cell cell)
        {
            if(cells.Contains(cell)) return false;
            cells.Add(cell);
            return true;
        }
    }
}
