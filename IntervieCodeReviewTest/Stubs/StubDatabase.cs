using InterviewCodeReview.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervieCodeReviewTest.Stubs
{
    public class StubDatabase : IDatabase
    {
        public IDictionary<string, string> Parameters;
        public int RowsAffected = 1;
        public string Command = String.Empty;

        public int Execute(string command, IDictionary<string, string> parameters)
        {
            Command = command;
            Parameters = parameters;
            return RowsAffected;
        }
    }
}
