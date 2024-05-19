
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
        public List<Cell> Cells { get; private set; } = new List<Cell>();
        public int Size { get; }
        public List<Cell> Draft { get; private set; } = new List<Cell>();

        public Board(int size) => Size = size;

        public bool TryFillCell(Cell cell)
        {
            if (Cells.Any(old => old.Row == cell.Row && old.Col == cell.Col)) 
                return false;
        
            Cells.Add(cell);
            return true;
        }

        public IBoardSnapshot Save()
        {
            return new BoardSnapshot(this);
        }

        public void Restore(IBoardSnapshot snapshot)
        {
            if (snapshot is BoardSnapshot boardSnapshot)
            {
                Draft = new List<Cell>(boardSnapshot.Draft);
            }
            else
            {
                throw new ArgumentException("The snapshot is not of type BoardSnapshot");
            }
        }
    }
}
