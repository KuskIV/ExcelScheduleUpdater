using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAbsence
{
    class Day
    {
        public int weekNumber;
        public string dayname;
        public Date date;
        public List<Lecture> lectures = new List<Lecture>();

        public Day() { }
    }
}
