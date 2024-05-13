namespace SudokuComponents
{
    public class Cell
    {
        public int row {  get;}
        public int col { get;}
         public int value { get;}
        public Cell(int[] cell, int value)
        {
            this.row = cell[0];
            this.col = cell[1];
            this.value = value;
        }
    }
}
