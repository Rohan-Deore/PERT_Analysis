using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERT_Analyser
{
    public class Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EarliestStart  { get; set; }
        public int EarliestFinish { get; set; }
    }
}
