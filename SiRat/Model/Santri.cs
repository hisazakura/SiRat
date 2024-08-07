using SiRat.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Model
{
    public class Santri
    {
        public string Name { get; }
        public List<ReportData> Reports { get; set; }

        public Santri(string name)
        {
            Name = name;
            Reports = new List<ReportData>();
        }

        public Santri(string name, List<ReportData> reports)
        {
            Name = name;
            Reports = reports;
        }
    }
}
