using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Infrastructure.Common;
using SQLite;
using Todo.Mobile.Common;
using Todo.Mobile.Infrastructure.Common;
using Todo.Mobile.Infrastructure.EventStore;
using Xamarin.Forms;
using SQLiteConnection = SQLite.SQLiteConnection;
using Todo.BoundedContext.Data;

[assembly: Dependency(typeof(Todo.Mobile.iOS.SQLiteIOS))]
namespace Todo.Mobile.iOS
{

    public class SQLiteIOS : ISQLite
    {
        public SQLiteAsyncConnection GetAsyncConnection(Database database)
        {
            var path = GetPath(database);
            return new SQLiteAsyncConnection(path);
        }


        private string GetPath(Database database)
        {
            var sqliteFilename = "TodoSQLite.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            if (database == Database.Write)
            {
                sqliteFilename = "TodoSQLite.db3";
            }
            else
            {
                sqliteFilename = "TodoSQLiteRead.db3";
            }
            return path;
        }

        public SQLiteConnection GetConnection(Database database)
        {
            var path = GetPath(database);
            var conn = new SQLite.SQLiteConnection(path);
            if (database == Database.Write)
            {
                try
                {
                    conn.CreateTable<SqliteEventStore.DocumentData>();
                }
                catch { }
                
            }
            else
            {
                try
                {
                    conn.CreateTable<TodoItemDTO>();
                    conn.CreateTable<TodoListDTO>();
                }
                catch(Exception ex) {
                    var ex1 = ex;
                }
            }

            // Return the database connection
            return conn;
        }

        public void ResetReadDatabase()
        {
            throw new NotImplementedException();
        }

        public void ResetWriteDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
