using OfficeOpenXml;
using System.Data;
using System.Runtime.CompilerServices;

namespace Rating
{
    public class RatingGenerator
    {
        private string _filePath = "C:\\Users\\User\\Desktop\\2 курс 2 семестр\\Lab6\\MySudokuGame\\Rating\\Rating.xlsx";

        public string[,] ReadRatingFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo fileInfo = new FileInfo(_filePath);
            string[,] data;

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.End.Row;
                int colCount = worksheet.Dimension.End.Column;
                data = new string[rowCount, colCount];

                for (int rowNumber = 1; rowNumber <= rowCount; rowNumber++)
                {
                    for (int colNumber = 1; colNumber <= colCount; colNumber++)
                    {
                        data[rowNumber - 1, colNumber - 1] = worksheet.Cells[rowNumber, colNumber].Value?.ToString();
                    }
                }
            }

            return data;
        }
        public DataTable GenerateDataTable()
        {
            DataTable dt = new DataTable();
            var excelReader = new RatingGenerator();
            string[,] data = excelReader.ReadRatingFile();
            int nbColumns = data.GetLength(1);
            int nbRows = data.GetLength(0);
            for (int col = 0; col < nbColumns; col++)
            {
                dt.Columns.Add(data[0, col]);
            }
            for (int row = 1; row < nbRows; row++)
            {
                DataRow dr = dt.NewRow();
                for (int col = 0; col < nbColumns; col++)
                {
                    dr[col] = data[row, col];
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        public void  WriteToFile(DataTable dataTable)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo fileInfo = new FileInfo(_filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo)) { 

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int startRow = worksheet.Dimension == null ? 1 : worksheet.Dimension.End.Row + 1;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        worksheet.Cells[startRow + i, j + 1].Value = dataTable.Rows[i][j].ToString();
                    }
                }

                package.Save();
            }
        }
    }
}
