using SudokuComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySudoku.Extensions;
public static class BoardExtensions
{
    public static int[,] GetOpenedCells(this Board board)
    {
        int[,] openedCells = new int[board.Size, board.Size];

        foreach (var cell in board.cells)
        {
            openedCells[cell.row, cell.col] = 1;
        }

        return openedCells;
    }
}
