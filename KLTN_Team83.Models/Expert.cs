using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.Models
{
    public class Expert
    {
        public int id_Expert { get; set; }
        public string name { get; set; }
        public string? img { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public int id_TypeExpert { get; set; }
        public TypeExpert TypeExpert { get; set; }
    }
}
