using MyFitness.iOS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MyFitness.Services;
using SQLite;
using System.IO;

[assembly: Dependency (typeof(SqlLite_IOS))]
namespace MyFitness.iOS.Service
{
    public class SqlLite_IOS : ISqLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "MyFitness.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}