using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using WinFormsApp1.Classes;


namespace WinFormsApp1
{
    class Sqliteclass
    {
        public DataRow[] datarows = null;
        public SQLiteDataAdapter dataadapter = null;
        public DataSet dataset = new DataSet();
        public DataTable datatable = new DataTable();
        //Конструктор
        public Sqliteclass()
        {

        }
        #region ExecuteNonQuery
        public int iExecuteNonQuery(string FileData, string sSql, int where) //Выполнение запроса к базе с возвращением результата (возвращаеь число затронутых ею записей)
        {
            int n = 0;
            try
            {
                using (SQLiteConnection con = new SQLiteConnection()) 
                {
                    if (where == 0)
                    {
                        con.ConnectionString = @"Data Source=" + FileData + "; Version=3; New=True;";//строка соединения c БД, где FileData путь к файлу БД, а New == true создать новую, false - открыть существующую.
                    }
                    else
                    {
                        con.ConnectionString = @"Data Source=" + FileData + ";Version=3;New=False;";
                    }
                    con.Open();
                    using (SQLiteCommand sqlCommand = con.CreateCommand()) // экземпляр провайдера команд 
                    {
                        sqlCommand.CommandText = sSql; // sSql используется для свойства CommandText, с помощью которого устанавливается SQL-выражение, которое будет выполняться. 
                        n = sqlCommand.ExecuteNonQuery();//метод из System.Data.SQLite.dll Выполняет для подключения инструкцию Transact-SQL и возвращает количество задействованных в инструкции строк.
                    }
                    con.Close();
                }
            }
            catch
            {
                n = 0;
            }
            return n;
        }
        #endregion
        #region Execute
        public DataRow[] drExecute(string FileData, string sSql) // получение коллекции строк таблицы
        {

            try
            {
                using (SQLiteConnection con = new SQLiteConnection())
                {
                    con.ConnectionString = @"Data Source=" + FileData + ";Version=3;New=False;";
                    con.Open();
                    using (SQLiteCommand sqlCommand = con.CreateCommand())
                    {
                        dataadapter = new SQLiteDataAdapter(sSql, con); //Создает адаптер данных с предоставленным текстом команды select, связанный с указанным соединением.
                        dataset.Reset(); // очищает ранее созданный dataset
                        dataadapter.Fill(dataset); //Заполняем dataset
                        datatable = dataset.Tables[0]; // Обращение к таблице
                        datarows = datatable.Select(); // Запрос на выборку данных
                    }
                    con.Close();
                }
            }
            catch (Exception)
            {
                datarows = null;
            }
            return datarows;
        }
        #endregion
    }
}
