using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewCodeReview.Interfaces
{
    public interface IDatabase
    {
        int Execute(string command, IDictionary<string, string> parameters);
    }
}
