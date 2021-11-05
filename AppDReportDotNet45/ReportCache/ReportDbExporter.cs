using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;

namespace AppdReporter.ReportCache
{
    class ReportDbExporter 
    {
        ExcelReport excelReport;
        public ReportDbExporter()
        {
            excelReport = new ExcelReport();
        }

        public ReportDbExporter(string visual)
        {

        }

        public void prepareExcelReport(string excelFile)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=cache.sqlite;Version=3;"))
            {
                con.Open();

                string sqliteQuery = "SELECT * FROM report";
                using (SQLiteCommand cmd = new SQLiteCommand(sqliteQuery, con))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read()) 
                        {
                            excelReport.insertExcelFileRecord(rdr.GetValue(0).ToString(), 
                                rdr.GetValue(1).ToString(), 
                                rdr.GetValue(2).ToString(), 
                                rdr.GetValue(3).ToString());
                        }
                    }
                }

                con.Close();
            }
            excelReport.saveExcelFile(excelFile);
        }


        public void reportToListView(ListView lv)
        {
            

            using (SQLiteConnection con = new SQLiteConnection("Data Source=cache.sqlite;Version=3;"))
            {
                con.Open();

                string sqliteQuery = "SELECT * FROM report";
                using (SQLiteCommand cmd = new SQLiteCommand(sqliteQuery, con))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            

                            string[] row = { rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString() };
                            var listViewItem = new ListViewItem(row);
                            lv.Items.Add(listViewItem);

                        }
                    }
                }
                foreach (ColumnHeader column in lv.Columns)
                {
                    column.Width = -2;
                }
                con.Close();
            }
        }
    }
}
