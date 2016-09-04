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
using MyFitness.Model.Strava;
using MyFitness.Data;

[assembly: Xamarin.Forms.Dependency(typeof(Sql))]
namespace MyFitness.Data
{
    public class Sql
    {
        private SQLiteConnection database;

        public Sql()
        {
            database = DependencyService.Get<ISqLite>().GetConnection();
            CreateTable();
        }

        #region Fitness

        /// <summary>
        /// Gets a model containing the althetes fitness for a particular id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FitnessModel GetFitnessItem(int id)
        {
            return database.Table<FitnessModel>().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Gets all the saved fitness models for the logged in athlete.
        /// </summary>
        /// <returns>A list of fitness items.</returns>
        public IEnumerable<FitnessModel> GetFitnessItems()
        {
            return database.Table<FitnessModel>();
        }

        /// <summary>
        /// Gets the range of fitnesses for the athlete.
        /// </summary>
        /// <param name="fromDate">The date to get the activities from.</param>
        /// <param name="toDate">The date to get the activites to.</param>
        /// <returns>A list of fitness models.</returns>
        public IEnumerable<FitnessModel> GetFitnessRange(DateTime fromDate, DateTime toDate)
        {
            return database.Table<FitnessModel>().Where(x => x.Date > fromDate && x.Date < toDate);
        }

        public FitnessModel GetLatestFitness()
        {
            int id = database.Table<FitnessModel>().Max(x => x.Id);
            return database.Table<FitnessModel>().FirstOrDefault(y => y.Id == id);
        }

        /// <summary>
        /// Saves a fitness model.
        /// </summary>
        /// <param name="f"></param>
        /// <returns>The objects primary key.</returns>
        public int SaveFitness(FitnessModel f)
        {
            if (GetFitnessItem(f.Id) != null)
            {
                return database.Update(f);
            }
            else
            {
                return database.Insert(f);
            }
        }

        #endregion

        #region Activity

        public IEnumerable<ActivityModel> GetActivities()
        {
            return database.Table<ActivityModel>();
        }

        public ActivityModel GetActivity(int id)
        {
            return database.Table<ActivityModel>().Where(x => x.ActivityId == id).FirstOrDefault();
        }

        public int SaveActivity(ActivityModel activity)
        {
            if (GetActivity(activity.ActivityId) != null)
            {
                return database.Update(activity);
            }
            else
            {
                return database.Insert(activity);
            }
        }
        #endregion

        #region Athlete
        public Athlete GetAthlete()
        {
            return database.Table<Athlete>().FirstOrDefault();
        }

        public int SaveAthlete(Athlete a)
        {
            return database.Insert(a);
        }
        #endregion

        private void CreateTable()
        {
            database.CreateTable<FitnessModel>();
            database.CreateTable<ActivityModel>();
            database.CreateTable<Athlete>();
        }
    }
}
