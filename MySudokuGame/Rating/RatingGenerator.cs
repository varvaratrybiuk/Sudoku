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
        private static RatingGenerator _instance;
        private static readonly object _lock = new object();
        private readonly string _filePath;

        private RatingGenerator(string filePath)
        {
            _filePath = filePath ?? Path.GetFullPath("../../../../Rating/Rating.xlsx");
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"File not found: {_filePath}");
            }
        }

        public static RatingGenerator GetInstance(string filePath = null)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RatingGenerator(filePath);
                    }
                }
            }
            return _instance;
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
                    columnData.Sort((a, b) =>
                    {
                        if (a == null && b == null) return 0;
                        if (a == null) return 1;
                        if (b == null) return -1;
                        return a.CompareTo(b);
                    });
                    data.Add(columnName, columnData);
                }
            }

            return data;
        }

        public DataTable GenerateDataTable()
        {
            DataTable dt = new DataTable();
            var data = ReadRatingFile();

            foreach (var columnName in data.Keys)
            {
                dt.Columns.Add(columnName);
            }

            int nbRows = data.Values.First().Count;

            for (int row = 0; row < nbRows; row++)
            {
                DataRow dr = dt.NewRow();
                int col = 0;
                foreach (var columnData in data.Values)
                {
                    dr[col++] = columnData[row];
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        
        public void WriteToFile(string columnName, string time)
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
                    if (worksheet.Cells[1, i].Value?.ToString() == columnName)
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
