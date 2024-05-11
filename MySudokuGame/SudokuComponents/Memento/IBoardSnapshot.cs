using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuComponents.Memento
{
    public interface IBoardSnapshot
    {
        public Guid Id { get; }
        public DateTime Date { get; }
    }
}
