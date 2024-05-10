namespace SudokuComponents
{
    public class Cell
    {
        public int row {  get; set; }
        public int col { get; set; }
         public int value { get; set; }
        public Cell(int[] cell, int value)
        {
            this.row = cell[0];
            this.col = cell[1];
            this.value = value;
        }
    }
}
