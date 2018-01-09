//using Microsoft.Data.Sqlite;

namespace FunTimer.Lib.Data
{
    public class SQLiteDataLayer
    {
        public static void InitializeDatabase()
        {
            //using (SqliteConnection db = new SqliteConnection("Filename=timeRecords2.db"))// Data Source = timeRecords2.db; FailIfMissing=True;"))
            //{
            //    db.Open();

            //    string tblComm = "CREATE TABLE IF NOT EXISTS TimeRecords (" +
            //        " RecordId INTEGER PRIMARY KEY AUTOINCREMENT, " +
            //        " RecordType int NOT NULL, " +
            //        " StartTime text NOT NULL, " +
            //        " EndTime text NOT NULL " +
            //        ");";

            //    SqliteCommand createTable = new SqliteCommand(tblComm, db);

            //    createTable.ExecuteReader();
            //}
        }

    }
}
