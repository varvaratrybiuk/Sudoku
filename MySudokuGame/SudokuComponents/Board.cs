
using SudokuComponents.Memento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            if(cells.Any(old => old.row == cell.row && old.col == cell.col)) return false;
            cells.Add(cell);
            return true;
        }
        public IBoardSnapshot Save()
        {
            return new BoardSnapshot(this);
        }

        public void Restore(IBoardSnapshot snapshot)
        {
            if (snapshot is BoardSnapshot)
            {
                var memento = (BoardSnapshot)snapshot;
                Draft = new List<Cell>(memento._draft);
            }
            else
            {
                throw new ArgumentException("The snapshot is not of type BoardSnapshot");
            }
        }
    }
}
