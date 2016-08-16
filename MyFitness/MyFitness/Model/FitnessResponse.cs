using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.Model
{
    public class FitnessResponse
    {
        public FitnessResponse()
        {
            Errors = new List<string>();
        }
        public string Content { get; set; }

        public HttpStatusCode Status { get; set;}

        public bool HasError { get; set; }

        public List<string> Errors { get; set; }
    }
}
