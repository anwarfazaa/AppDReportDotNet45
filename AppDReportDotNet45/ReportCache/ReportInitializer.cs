using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace AppdReporter.ReportCache
{
    class ReportInitializer
    {
        String cacheDirectory;
        SQLiteConnection m_dbConnection;
        Boolean firstTimeFlag;
        string tempSqlCmd;

        public ReportInitializer() {
            cacheDirectory = Path.Combine(Application.StartupPath, "cache.sqlite");
            sqliteDbCreateFile();
            sqliteDbInitConnection();
            sqliteDbInitTable();
        }

        public ReportInitializer(string export)
        {

        }

        private void sqliteDbCreateFile()
        {
            if (!File.Exists(cacheDirectory))
            {
                SQLiteConnection.CreateFile("cache.sqlite");
                firstTimeFlag = true;
            }
        }

        public void sqliteDbInitConnection() {
            this.m_dbConnection = new SQLiteConnection("Data Source=cache.sqlite;Version=3;");
            this.m_dbConnection.Open();
        }

        private void sqliteDbInitTable() { 
            string sqlCmd;
            if (this.firstTimeFlag)
            {
                sqlCmd = "CREATE TABLE report (hrEntity NVARCHAR,hrName NVARCHAR, hrTimestamp NVARCHAR, hrDuration NVARCHAR);";
                SQLiteCommand command = new SQLiteCommand(sqlCmd, this.m_dbConnection);
                command.ExecuteNonQuery();

            } else
            {
                sqlCmd = "DELETE FROM report;";
                SQLiteCommand command = new SQLiteCommand(sqlCmd, this.m_dbConnection);
                command.ExecuteNonQuery();
            }
        }

        public void sqliteDbInsertRecord(string hrEntity,string hrName, string hrTimestamp, string hrDuration)
        {
            tempSqlCmd = "INSERT INTO report (hrEntity , hrName , hrTimestamp , hrDuration) VALUES ('" + hrEntity + "','" + hrName + "','" + hrTimestamp + "','" + hrDuration +"')";
            SQLiteCommand command = new SQLiteCommand(tempSqlCmd, this.m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void sqliteDbCloseConnection()
        {
            //this.m_dbConnection.Close();
        }










    }
}
