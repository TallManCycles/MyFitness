using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using MyFitness.Services;
using Xamarin.Forms;
using MyFitness.Model;

namespace MyFitness.Data
{
    class Sql
    {
        private SQLiteConnection database;

        public Sql()
        {
            database = DependencyService.Get<ISqLite>().GetConnection();
            CreateTable();
        }

        public void CreateTable()
        {
            database.CreateTable<FitnessModel>();
        }

        public FitnessModel GetFitnessItem(int id)
        {
            return database.Table<FitnessModel>().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<FitnessModel> GetFitnessItems()
        {
            return database.Table<FitnessModel>();
        }

        public IEnumerable<FitnessModel> GetFitnessRange(DateTime fromDate, DateTime toDate)
        {
            return database.Table<FitnessModel>().Where(x => x.Date > fromDate && x.Date < toDate);
        }

        public int InsertFitness(FitnessModel f)
        {
            return database.Insert(f);
        }        
    }
}
