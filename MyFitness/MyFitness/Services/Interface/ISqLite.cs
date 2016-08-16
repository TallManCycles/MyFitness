using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Services
{
    public interface ISqLite
    {
        SQLiteConnection GetConnection();
    }
}
