using System;
using System.Collections.Generic;
using System.Text;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;

namespace AppdReporter.ReportCache
{
    class ExcelReport
    {

        Workbook report;
        Worksheet reportWorksheet;
        int row;

        public ExcelReport()
        {
            report = new Workbook();
            reportWorksheet = new Worksheet("Health Rules Report");
            reportWorksheet.Cells[0, 0] = new Cell("Affected Entity");
            reportWorksheet.Cells[0, 1] = new Cell("Health Rule");
            reportWorksheet.Cells[0, 2] = new Cell("Event Timestamp");
            reportWorksheet.Cells[0, 3] = new Cell("Event Duration (Minutes)");
            reportWorksheet.Cells.ColumnWidth[0, 0] = 10000;
            reportWorksheet.Cells.ColumnWidth[0, 1] = 10000;
            reportWorksheet.Cells.ColumnWidth[0, 2] = 10000;
            reportWorksheet.Cells.ColumnWidth[0, 3] = 10000;
            row = 1;
        }

        public void insertExcelFileRecord(string hrEntity, string hrName, string hrTimestamp, string hrDuration)
        {
            reportWorksheet.Cells[row, 0] = new Cell(hrEntity);
            reportWorksheet.Cells[row, 1] = new Cell(hrName);
            reportWorksheet.Cells[row, 2] = new Cell(hrTimestamp);
            reportWorksheet.Cells[row, 3] = new Cell(hrDuration);
            row += 1;
        }

        public void saveExcelFile(string reportOutputFilePath)
        {
            for (int i = row; i < (100 + row); i++)
                reportWorksheet.Cells[i, 0] = new Cell("");
            report.Worksheets.Add(reportWorksheet);
            report.Save(reportOutputFilePath);
        }

    }
}
