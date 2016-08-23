using MyFitness.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ILoginManager))]
namespace MyFitness.Services
{
    public interface ILoginManager
    {
        void ShowMainPage(); 

        void Logout();
    }
}
