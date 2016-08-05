using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using MyFitness.Services;
using MyFitness.Droid.Services;

[assembly: Dependency(typeof(SQLite_Droid))]
namespace MyFitness.Droid.Services
{
    public class SQLite_Droid : ISqLite
    {
        public SQLite_Droid() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "MyFitness.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = System.IO.Path.Combine(documentsPath, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}