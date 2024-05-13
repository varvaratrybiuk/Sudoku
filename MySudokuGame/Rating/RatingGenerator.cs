using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Rating
{
    public class RatingGenerator
    {
        private string _filePath = Path.GetFullPath("../../../../Rating/Rating.xlsx");
        public RatingGenerator() { }
        public RatingGenerator(string filePath)
        {
            _filePath = Path.GetFullPath(filePath);
        }
        public Dictionary<string, List<string>> ReadRatingFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo fileInfo = new FileInfo(_filePath);
            Dictionary<string, List<string>> data = new();

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.End.Row;
                int colCount = worksheet.Dimension.End.Column;

                for (int colNumber = 1; colNumber <= colCount; colNumber++)
                {
                    List<string> columnData = new List<string>();
                    string columnName = worksheet.Cells[1, colNumber].Value?.ToString();
                    for (int rowNumber = 2; rowNumber <= rowCount; rowNumber++)
                    {
                        columnData.Add(worksheet.Cells[rowNumber, colNumber].Value?.ToString());
                    }

                    data.Add(columnName, columnData);
                }
            }

            return data;
        }
        public DataTable GenerateDataTable(string chooselvl)
        {
            DataTable dt = new DataTable();
            var data = ReadRatingFile();

            if (data.ContainsKey(chooselvl))
            {
                dt.Columns.Add(chooselvl);

                int nbRows = data[chooselvl].Count;

                for (int row = 0; row < nbRows; row++)
                {
                    DataRow dr = dt.NewRow();
                    dr[chooselvl] = data[chooselvl][row];
                    dt.Rows.Add(dr);
                }
                var orderedRows = dt.AsEnumerable()
                        .Where(r => !string.IsNullOrEmpty(r.Field<string>(chooselvl)))
                        .OrderBy(r => r.Field<string>(chooselvl)).Take(10);
                if(orderedRows.Any()) 
                    dt = orderedRows.CopyToDataTable();
            }

            return dt;
        }
        public void  WriteToFile(string columnName, string time)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo fileInfo = new FileInfo(_filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.End.Row;
                int colCount = worksheet.Dimension.End.Column;
                int columnIndex = 0;
                for (int i = 1; i <= colCount; i++)
                {
                    if (worksheet.Cells[1, i].Value.ToString() == columnName)
                    {
                        columnIndex = i;
                        break;
                    }
                }
                if (columnIndex == 0)
                {
                    return;
                }
                worksheet.Cells[rowCount + 1, columnIndex].Value = time;
                package.Save();
            }
        }
    }
}
