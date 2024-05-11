using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuComponents.Memento
{
    public class BoardEditor
    {
        private List<IBoardSnapshot> _boardsHistory = new List<IBoardSnapshot>();
        private Board _board;
        public BoardEditor(Board board)
        {
            _board = board;
        }
        public void Save() => _boardsHistory.Add(_board.Save());
        public void Undo()
        {
            if (_boardsHistory.Count == 0)
            {
                return;
            }
            var snapshot = _boardsHistory.Last();

            if (snapshot != null)
            {
                _boardsHistory.Remove(snapshot);
                _board.Restore(snapshot);

            }

        }
        public void Clear()
        {
            _boardsHistory.Clear();
        }
    }
}
