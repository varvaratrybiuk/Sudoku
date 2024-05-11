using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuComponents.Memento
{
    public class BoardSnapshot : IBoardSnapshot
    {
        public List<Cell> _draft { get; }
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Date { get; } = DateTime.Now;
        public BoardSnapshot(Board board)
        {
            _draft = new List<Cell>(board.Draft);
        }
    }
}
